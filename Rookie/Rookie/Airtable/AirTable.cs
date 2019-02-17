using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Dwares.Dwarf;

namespace Dwares.Rookie.Airtable
{
	public class AirTable<TRecord> where TRecord : AirRecord
	{
		//static ClassRef @class = new ClassRef(typeof(AirTable));

		static HttpMethod HttpMethod_Patch = new HttpMethod("PATCH");

		public AirTable(AirBase airBase, string tableName)
		{
			//Debug.EnableTracing(@class);
			Guard.ArgumentNotNull(airBase, nameof(airBase));
			Guard.ArgumentNotEmpty(tableName, nameof(tableName));

			Base = airBase;
			Name = tableName;
		}

		public virtual Task Initialize()
		{
			return Task.CompletedTask;
		}

		public AirBase Base { get; }
		public string Name { get; }

		public virtual async Task<Exception> Probe(bool throwErrors = true)
		{
			try {
				await ListRecords(1);
				return null;
			}
			catch (Exception exc) {
				if (throwErrors)
					throw exc;
				return exc;
			}
		}

		public async Task<AirRecordList<TRecord>> ListRecords(QyeryBuilder queryBuilder = null)
		{
			if (queryBuilder == null)
				queryBuilder = new QyeryBuilder { };

			var uri = queryBuilder.GetUri(Base.BaseId, Name);

			var response = await AirClient.GetAsync(Base.ApiKey, uri);
			var recordList = JsonConvert.DeserializeObject<AirRecordList<TRecord>>(response.Body);

			recordList.CopyFieldsToProperties();
			return recordList;
		}

		public async Task<AirRecordList<TRecord>> ListRecords(int maxRecords)
		{
			Guard.ArgumentIsInRange(maxRecords, 1, AirClient.MAX_NUMBER_OF_RECORDS_IN_LIST, nameof(maxRecords));

			return await ListRecords(new QyeryBuilder { MaxRecords = maxRecords });
		}


		public Dictionary<string, object> GetRecordFields(TRecord record, string[] fieldNames)
		{
			if (fieldNames == null || fieldNames.Length == 0)
				return record.Fields;

			var fields = new Dictionary<string, object>();
			foreach (var name in fieldNames) {
				if (!record.Fields.ContainsKey(name))
					throw new ArithmeticException(String.Format("Record does not has filed {0}", name));

				fields.Add(name, record.Fields[name]);
			}
			return fields;
		}

		async Task<TRecord> CUROperation(HttpMethod method, string recordId, Dictionary<string, object> fields, bool typecast)
		{
			var uriStr = AirClient.RecordUri(Base.BaseId, Name, recordId);
			var fieldsAndTypecast = new FieldsAndTypecast(fields, typecast);

			var response = await AirClient.SendAsync(method, Base.ApiKey, new Uri(uriStr), fieldsAndTypecast.ToJson());
			var record = JsonConvert.DeserializeObject<TRecord>(response.Body);

			record.CopyFieldsToProperties();
			return record;
		}

		public async Task<TRecord> CreateRecord(Dictionary<string, object> fields, bool typecast = false)
		{
			return await CUROperation(HttpMethod.Post, null, fields, typecast);
		}

		public async Task<TRecord> CreateRecord(TRecord record, params string[] fieldNames)
		{
			var fields = GetRecordFields(record, fieldNames);

			return await CreateRecord(fields);
		}

		public async Task<TRecord> UpdateRecord(TRecord record, Dictionary<string, object> fields, bool typecast = false)
		{
			return await CUROperation(HttpMethod_Patch, record.Id, fields, typecast);
		}

		public async Task<TRecord> UpdateRecord(TRecord record, params string[] fieldNames)
		{
			var fields = GetRecordFields(record, fieldNames);

			return await UpdateRecord(record, fields);
		}

		public async Task CopyRecords(AirTable<TRecord> destTable, IEnumerable<string> fieldNames = null)
		{
			var list = await ListRecords();

			foreach (var record in list.Records) {
				var fields = record.GetFields(fieldNames);
				await destTable.CreateRecord(fields);
			}
		}

		public Task CopyRecords(AirTable<TRecord> destTable, params string[] fieldNames)
			=> CopyRecords(destTable, (IEnumerable<string>)fieldNames);
	}
}
