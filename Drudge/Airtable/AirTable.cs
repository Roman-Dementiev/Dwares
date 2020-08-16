using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Dwares.Dwarf;
using Dwares.Dwarf.Data;
using Dwares.Dwarf.Toolkit;
using System.Threading;

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

		public bool UsesUTC { get; set; } = false;


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
			var recordList = JsonConvert.DeserializeObject<AirRecordList<TRecord>>(response.Body);

			return recordList;
		}

		public async Task<AirRecordList<TRecord>> List<TRecord>(int maxRecords) where TRecord : AirRecord
		{
			Guard.ArgumentIsInRange(maxRecords, 1, AirClient.MAX_NUMBER_OF_RECORDS_IN_LIST, nameof(maxRecords));

			return await List<TRecord>(new QyeryBuilder { MaxRecords = maxRecords });
		}

		//public async Task<DataQueryResult> Query<TRecord>(object queryToken) where TRecord : AirRecord
		//{
		//	QyeryBuilder queryBuilder;
		//	if (queryToken is QyeryBuilder) {
		//		queryBuilder = (QyeryBuilder)queryToken;
		//	}
		//	else if (queryToken == null) {
		//		queryBuilder = new QyeryBuilder();
		//	}
		//	else {
		//		throw new ArgumentException("Invalid type of query token", queryToken.GetType().ToString());
		//	}

		//	var list = await List<TRecord>(queryBuilder);
		//	if (list != null && list.Records.Length > 0) {
		//		queryBuilder.Offset += list.Records.Length;

		//		return new DataQueryResult {
		//			Records = list.Records,
		//			Count = list.Records.Length,
		//			NextToken = queryBuilder
		//		};
		//	}
		//	else {
		//		return new DataQueryResult { Records = null, Count = 0, NextToken = null };
		//	}
		//}

		//public async Task<DataQueryResult> Filter<TRecord>(string formula, QyeryBuilder queryBuilder = null) where TRecord : AirRecord
		//{
		//	Guard.ArgumentNotEmpty(formula, nameof(formula));

		//	string savedFormula = null;
		//	if (queryBuilder == null) {
		//		queryBuilder = new QyeryBuilder { FilterByFormula = formula };
		//	}
		//	else {
		//		savedFormula = queryBuilder.FilterByFormula;
		//		queryBuilder.FilterByFormula = formula;
		//	}

		//	var result = await Query<TRecord>(queryBuilder);
		//	queryBuilder.FilterByFormula = savedFormula;
		//	return result;
		//}

		//public async Task<ICollection<IDataRecord>> GetAllRecords<TRecord>() where TRecord : AirRecord
		//{
		//	var records = new List<IDataRecord>();
		//	var queryBuilder = new QyeryBuilder();
		//	bool hasMore = true;
		//	do {
		//		var list = await List<TRecord>(queryBuilder);
		//		if (list != null && list.Records.Length > 0) {
		//			records.AddRange(list.Records);
		//			queryBuilder.Offset = list.Offset;
		//			hasMore = list.Offset != null;
		//		}
		//		else {
		//			hasMore = false;
		//		}
		//	}
		//	while (hasMore);

		//	return records;
		//}

		public async Task<TRecord> GetRecord<TRecord>(string recordId)
		{
			var uri = AirClient.RecordUri(Base.BaseId, Name, recordId);

			var response = await AirClient.GetAsync(Base.ApiKey, uri);

			var settings = new JsonSerializerSettings();
			settings.DateTimeZoneHandling = UsesUTC ? DateTimeZoneHandling.Utc : DateTimeZoneHandling.Local;

			var record = JsonConvert.DeserializeObject<TRecord>(response.Body, settings);

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

			var response = await AirClient.SendAsync(method, Base.ApiKey, uri, fieldsAndTypecast.ToJson());
			var record = JsonConvert.DeserializeObject<TRecord>(response.Body);

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

		//public Task<AirRecordList<TRecord>> List(QyeryBuilder queryBuilder = null) => base.List<TRecord>(queryBuilder);
		//public Task<AirRecordList<TRecord>> Filter(string formula, QyeryBuilder queryBuilder = null) => base.Filter<TRecord>(formula, queryBuilder);

		//public override async Task<Exception> Probe(bool throwErrors = true)
		//{
		//	try {
		//		await List<TRecord>(1);
		//		return null;
		//	}
		//	catch (Exception exc) {
		//		if (throwErrors)
		//			throw exc;
		//		return exc;
		//	}
		//}

		//public async Task<TRecord> GetRecord(string recordId)
		//{
		//	var uri = AirClient.RecordUri(Base.BaseId, Name, recordId);

		//	var response = await AirClient.GetAsync(Base.ApiKey, uri);
		//	var record = JsonConvert.DeserializeObject<TRecord>(response.Body);

		//	return record;
		//}

		public async Task<AirRecordList<TRecord>> List(QyeryBuilder queryBuilder)
		{
			if (queryBuilder == null)
				queryBuilder = new QyeryBuilder { };

			var uri = queryBuilder.GetUri(Base.BaseId, Name);

			var response = await AirClient.GetAsync(Base.ApiKey, uri);
			var recordList = JsonConvert.DeserializeObject<AirRecordList<TRecord>>(response.Body);

			return recordList;
		}

		public async Task<AirRecordList<TRecord>> List(int maxRecords, string sortField = null, bool sortDescending = false)
		{

			var queryBuilder = new QyeryBuilder();
			if (maxRecords > 0) {
				Guard.ArgumentIsInRange(maxRecords, 1, AirClient.MAX_NUMBER_OF_RECORDS_IN_LIST, nameof(maxRecords));
				queryBuilder.MaxRecords = maxRecords;
			}
			if (!string.IsNullOrEmpty(sortField)) {
				queryBuilder.Sort = new List<Sort> {
					new Sort {
						Field = sortField, 
						Direction = sortDescending ? SortDirection.Desc : SortDirection.Asc
					}
				};
			}
			return await List(queryBuilder);
		}

		public async Task<TRecord[]> ListRecords(int maxRecords = 0, string sortField = null, bool sortDescending = false)
		{
			var list = await List(maxRecords, sortField, sortDescending);
			return list?.Records;
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

			try {
				var result = await List<TRecord>(queryBuilder);
				return result;
			} finally {
				queryBuilder.FilterByFormula = savedFormula;
			}
		}


		//public Dictionary<string, object> GetRecordFields(TRecord record, string[] fieldNames)
		//{
		//	if (fieldNames == null || fieldNames.Length == 0)
		//		return record.Fields;

		//	var fields = new Dictionary<string, object>();
		//	foreach (var name in fieldNames) {
		//		if (!record.Fields.ContainsKey(name))
		//			throw new ArithmeticException(String.Format("Record does not has field {0}", name));

		//		fields.Add(name, record.Fields[name]);
		//	}
		//	return fields;
		//}

		//static Dictionary<string, object> FixDates(Dictionary<string, object> fields)
		//{
		//	var result = new Dictionary<string, object>();
		//	foreach (var key in fields.Keys) {
		//		var value = fields[key];
		//		if (value is DateOnly dateonly) {
		//			result[key] = dateonly.ToString();
		//		} else {
		//			result[key] = value;
		//		}
		//	}
		//	return result;
		//}


		//async Task<TRecord> CUROperation(HttpMethod method, string recordId, Dictionary<string, object> fields, bool typecast)
		//{
		//	// For JsonConvert
		//	fields = FixDates(fields);

		//	var uri = AirClient.RecordUri(Base.BaseId, Name, recordId);
		//	var fieldsAndTypecast = new FieldsAndTypecast(fields, typecast);

		//	var response = await AirClient.SendAsync(method, Base.ApiKey, uri, fieldsAndTypecast.ToJson());
		//	var record = JsonConvert.DeserializeObject<TRecord>(response.Body);

		//	return record;
		//}

		//public async Task<TRecord> CreateRecord(Dictionary<string, object> fields, bool typecast = false)
		//{
		//	return await CUROperation(HttpMethod.Post, null, fields, typecast);
		//}

		//public async Task<TRecord> CreateRecord(TRecord record, params string[] fieldNames)
		//{
		//	var fields = GetRecordFields(record, fieldNames);

		//	return await CreateRecord(fields);
		//}

		//public async Task<TRecord> UpdateRecord(TRecord record, Dictionary<string, object> fields, bool typecast = false)
		//{
		//	return await CUROperation(HttpMethod_Patch, record.Id, fields, typecast);
		//}

		//public async Task<TRecord> UpdateRecord(TRecord record, params string[] fieldNames)
		//{
		//	var fields = GetRecordFields(record, fieldNames);

		//	return await UpdateRecord(record, fields);
		//}

		//public async Task<bool> DeleteRecord(string recordId)
		//{
		//	var uri = AirClient.RecordUri(Base.BaseId, Name, recordId);
		//	try {
		//		await AirClient.DeleteAsync(Base.ApiKey, uri);
		//		return true;
		//	} catch (Exception exc) {
		//		Debug.ExceptionCaught(exc);
		//		return false;
		//	}
		//}

		//public async Task CopyRecords(AirTable<TRecord> destTable, IEnumerable<string> fieldNames = null)
		//{
		//	var list = await ListRecords();

		//	foreach (var record in list.Records) {
		//		var fields = record.GetFields(fieldNames);
		//		await destTable.CreateRecord(fields);
		//	}
		//}

		//public Task CopyRecords(AirTable<TRecord> destTable, params string[] fieldNames)
		//	=> CopyRecords(destTable, (IEnumerable<string>)fieldNames);

		//public async override Task<DataQueryResult> QueryRecords(object queryToken)
		//{
		//	QyeryBuilder queryBuilder;
		//	if (queryToken is QyeryBuilder) {
		//		queryBuilder = (QyeryBuilder)queryToken;
		//	} else if (queryToken == null) {
		//		queryBuilder = new QyeryBuilder();
		//	}  else {
		//		throw new ArgumentException("Invalid type of query token", queryToken.GetType().ToString());
		//	}

		//	var list = await ListRecords(queryBuilder);
		//	if (list != null && list.Records.Length > 0)
		//	{
		//		queryBuilder.Offset += list.Records.Length;

		//		return new DataQueryResult {
		//			Records = list.Records,
		//			Count = list.Records.Length,
		//			NextToken = queryBuilder
		//		};
		//	}
		//	else {
		//		return new DataQueryResult { Records = null, Count = 0, NextToken = null };
		//	}
		//}

	}
}
