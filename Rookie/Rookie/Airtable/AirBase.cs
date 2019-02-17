using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Dwares.Dwarf;


namespace Dwares.Rookie.Airtable
{
	public class AirBase
	{
		//static ClassRef @class = new ClassRef(typeof(ABase));

		public AirBase(string apiKey, string baseId)
		{
			//Debug.EnableTracing(@class);
			Guard.ArgumentNotEmpty(apiKey, nameof(apiKey));
			Guard.ArgumentNotEmpty(baseId, nameof(baseId));

			ApiKey = apiKey;
			BaseId = baseId;
		}

		public virtual Task Initialize()
		{
			return Task.CompletedTask;
		}

		public string ApiKey { get; }
		public string BaseId { get; }

		//public async Task<Exception> ProbeTable(string table, bool throwErrors = true)
		//{
		//	Guard.ArgumentNotNull(table, nameof(table));

		//	try {
		//		await ListRecords<AirRecord>(table, 1);
		//		return null;
		//	}
		//	catch (Exception exc) {
		//		if (throwErrors)
		//			throw exc;
		//		return exc;
		//	}
		//}

		//public async Task<AirRecordList<TRecord>> ListRecords<TRecord>(string table, QyeryBuilder queryBuilder = null) where TRecord : AirRecord
		//{
		//	Guard.ArgumentNotNull(table, nameof(table));

		//	if (queryBuilder == null)
		//		queryBuilder = new QyeryBuilder { };

		//	var uri = queryBuilder.GetUri(BaseId, table);

		//	var response = await AirClient.GetAsync(ApiKey, uri);
		//	var recordList = JsonConvert.DeserializeObject<AirRecordList<TRecord>>(response.Body);

		//	recordList.GetFields();
		//	return recordList;
		//}

		//public async Task<AirRecordList<TRecord>> ListRecords<TRecord>(string table, int maxRecords) where TRecord : AirRecord
		//{
		//	Guard.ArgumentNotNull(table, nameof(table));
		//	Guard.ArgumentIsInRange(maxRecords, 1, AirClient.MAX_NUMBER_OF_RECORDS_IN_LIST, nameof(maxRecords));

		//	return await ListRecords<TRecord>(table ,new QyeryBuilder { MaxRecords = maxRecords });

		//}

		//async Task<TRecord> CUROperation<TRecord>(HttpMethod method, string table, string recordId, Dictionary<string, object> fields, bool typecast) where TRecord : AirRecord
		//{
		//	Guard.ArgumentNotNull(table, nameof(table));

		//	var uriStr = AirClient.RecordUri(BaseId, table, recordId);
		//	var fieldsAndTypecast = new FieldsAndTypecast(fields, typecast);

		//	var response = await AirClient.SendAsync(method, ApiKey, new Uri(uriStr), fieldsAndTypecast.ToJson());
		//	var record = JsonConvert.DeserializeObject<TRecord>(response.Body);

		//	record.CopyFieldsToProperties();
		//		return record;
		//}

		//public async Task<TRecord> CreateRecord<TRecord>(string table, Dictionary<string, object> fields, bool typecast = false) where TRecord : AirRecord
		//{
		//	return await CUROperation<TRecord>(HttpMethod.Post, table, null, fields, typecast);
		//}

		//public async Task CopyRecords<TRecord>(string table, AirBase destBase, string destTable, IEnumerable<string> fieldNames = null) where TRecord : AirRecord
		//{
		//	var list = await ListRecords<TRecord>(table);

		//	foreach (var record in list.Records) {
		//		var fields = record.GetFields(fieldNames);
		//		await destBase.CreateRecord<TRecord>(destTable, fields);
		//	}

		//}

		//public Task CopyRecords<TRecord>(string table, AirBase destBase, IEnumerable<string> fieldNames = null) where TRecord : AirRecord
		//	=> CopyRecords<TRecord>(table, destBase, table, fieldNames);

		//public Task CopyRecords<TRecord>(string table, AirBase destBase, params string[] fieldNames) where TRecord : AirRecord
		//	=> CopyRecords<TRecord>(table, destBase, table, fieldNames);
	}
}
