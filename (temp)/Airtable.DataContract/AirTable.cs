using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Drudge.Airtable
{
	public class AirTable
	{
		public static HttpMethod HttpMethod_Patch = new HttpMethod("PATCH");

		public AirTable(AirBase airBase, string tableName)
		{
			//Debug.EnableTracing(@class);
			Base = Guard.ArgumentNotNull(airBase, nameof(airBase));
			Name = Guard.ArgumentNotEmpty(tableName, nameof(tableName));
		}

		public AirBase Base { get; }
		public string Name { get; }
		public object TableId => Name;


		public virtual Task Initialize()
		{
			return Task.CompletedTask;
		}

		public virtual async Task Probe()
		{
			await List<AirRecord>(1);
		}

		public async Task<AirRecordList<TRecord>> List<TRecord>(QyeryBuilder queryBuilder = null) where TRecord : AirRecord
		{
			if (queryBuilder == null)
				queryBuilder = new QyeryBuilder { };

			var uri = queryBuilder.GetUri(Base.BaseId, Name);

			var response = await AirClient.GetAsync(Base.ApiKey, uri);
			var recordList = Serialization.DeserializeJson<AirRecordList<TRecord>>(response.Body);

			return recordList;
		}

		public async Task<AirRecordList<TRecord>> List<TRecord>(int maxRecords) where TRecord : AirRecord
		{
			Guard.ArgumentIsInRange(maxRecords, 1, AirClient.MAX_NUMBER_OF_RECORDS_IN_LIST, nameof(maxRecords));

			return await List<TRecord>(new QyeryBuilder { MaxRecords = maxRecords });
		}

		public async Task<TRecord> GetRecord<TRecord>(string recordId) where TRecord : AirRecord
		{
			var uri = AirClient.RecordUri(Base.BaseId, Name, recordId);

			var response = await AirClient.GetAsync(Base.ApiKey, uri);
			var record = Serialization.DeserializeJson<TRecord>(response.Body);

			return record;
		}

		static Dictionary<string, object> FixDates(Dictionary<string, object> fields)
		{
			var result = new Dictionary<string, object>();
			foreach (var key in fields.Keys) {
				var value = fields[key];
				if (value is DateOnly dateonly) {
					result[key] = dateonly.ToString();
				}
				else {
					result[key] = value;
				}
			}
			return result;
		}


		public Dictionary<string, object> GetRecordFields(AirRecord record, IEnumerable<string> fieldNames)
		{
			if (fieldNames != null) {
				var fields = new Dictionary<string, object>();
				foreach (var name in fieldNames) {
					if (!record.Fields.ContainsKey(name))
						throw new ArithmeticException(String.Format("Record does not has field {0}", name));

					fields.Add(name, record.Fields[name]);
				}
				if (fields.Count > 0)
					return fields;
			}
			return record.Fields;
		}

		async Task<TRecord> CUROperation<TRecord>(HttpMethod method, string recordId, Dictionary<string, object> fields, bool typecast) where TRecord : AirRecord
		{
			// For JsonConvert
			fields = FixDates(fields);

			var uri = AirClient.RecordUri(Base.BaseId, Name, recordId);
			var fieldsAndTypecast = new FieldsAndTypecast(fields, typecast);
			var requestContent = Serialization.SerializeToJson(fieldsAndTypecast);

			var response = await AirClient.SendAsync(method, Base.ApiKey, uri, requestContent);
			var record = Serialization.DeserializeJson<TRecord>(response.Body);

			return record;
		}

		public async Task<TRecord> CreateRecord<TRecord>(Dictionary<string, object> fields, bool typecast = false) where TRecord : AirRecord
		{
			return await CUROperation<TRecord>(HttpMethod.Post, null, fields, typecast);
		}

		public async Task<TRecord> CreateRecord<TRecord>(TRecord record, params string[] fieldNames) where TRecord : AirRecord
		{
			var fields = GetRecordFields(record, fieldNames);

			return await CreateRecord<TRecord>(fields);
		}

		public async Task<TRecord> UpdateRecord<TRecord>(TRecord record, Dictionary<string, object> fields, bool typecast = false) where TRecord : AirRecord
		{
			return await CUROperation<TRecord>(HttpMethod_Patch, record.Id, fields, typecast);
		}

		public async Task<TRecord> UpdateRecord<TRecord>(TRecord record, params string[] fieldNames) where TRecord : AirRecord
		{
			var fields = GetRecordFields(record, fieldNames);

			return await UpdateRecord(record, fields);
		}

		public async Task<bool> DeleteRecord(string recordId)
		{
			var uri = AirClient.RecordUri(Base.BaseId, Name, recordId);
			try {
				await AirClient.DeleteAsync(Base.ApiKey, uri);
				return true;
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				return false;
			}
		}

		public async Task CopyRecords<TRecord>(AirTable<TRecord> destTable, IEnumerable<string> fieldNames = null) where TRecord : AirRecord
		{
			var list = await List<TRecord>();

			foreach (var record in list.Records) {
				var fields = record.GetFields(fieldNames);
				await destTable.CreateRecord<TRecord>(fields);
			}
		}

		public Task CopyRecords<TRecord>(AirTable<TRecord> destTable, params string[] fieldNames) where TRecord : AirRecord
			=> CopyRecords(destTable, (IEnumerable<string>)fieldNames);

	}

	public class AirTable<TRecord> : AirTable where TRecord : AirRecord
	{
		//static ClassRef @class = new ClassRef(typeof(AirTable));

		public AirTable(AirBase airBase, string tableName) : base(airBase, tableName) { }

		public Task<TRecord> GetRecord(string recordId)
		{
			return base.GetRecord<TRecord>(recordId);
		}

		public async Task<AirRecordList<TRecord>> ListRecords(QyeryBuilder queryBuilder = null)
		{
			if (queryBuilder == null)
				queryBuilder = new QyeryBuilder { };

			var uri = queryBuilder.GetUri(Base.BaseId, Name);

			//var response = await AirClient.GetAsync(Base.ApiKey, uri);
			var client = AirClient.Instance;
			var apiKey = Base.ApiKey;
			var response = await client.SendRequestAsync(HttpMethod.Get, apiKey, uri, null, true);
			var recordList = Serialization.DeserializeJson<AirRecordList<TRecord>>(response.Body);

			return recordList;
		}

		public async Task<AirRecordList<TRecord>> ListRecords(int maxRecords)
		{
			Guard.ArgumentIsInRange(maxRecords, 1, AirClient.MAX_NUMBER_OF_RECORDS_IN_LIST, nameof(maxRecords));

			return await ListRecords(new QyeryBuilder { MaxRecords = maxRecords });
		}

		public async Task<AirRecordList<TRecord>> FilterRecords(string formula, QyeryBuilder queryBuilder = null)
		{
			Guard.ArgumentNotEmpty(formula, nameof(formula));

			string savedFormula = null;
			if (queryBuilder == null) {
				queryBuilder = new QyeryBuilder { FilterByFormula = formula };
			}
			else {
				savedFormula = queryBuilder.FilterByFormula;
				queryBuilder.FilterByFormula = formula;
			}

			var result = await List<TRecord>(queryBuilder);
			queryBuilder.FilterByFormula = savedFormula;
			return result;
		}

	}
}
