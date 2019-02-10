//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web;
//using Newtonsoft.Json;
//using Dwares.Dwarf;

//namespace Dwares.Rookie.Airtable
//{
//	public class AirTable
//	{
//		//static ClassRef @class = new ClassRef(typeof(AirTable));

//		public AirTable(AirBase airBase, string tableName)
//		{
//			//Debug.EnableTracing(@class);

//			Base = airBase ?? throw new ArgumentNullException(nameof(airBase));
//			Name = Strings.IfNotEmpty(tableName, nameof(tableName));
//		}

//		public AirBase Base { get; }
//		public string Name { get; }

//		public async Task<AirRecordList> ListRecords(QyeryBuilder queryBuilder)
//		{
//		//	AirRecordList recordList = null;
//			var uri = queryBuilder.GetUri(Base.BaseId, Name);
			
//			var response = await AirClient.GetAsync(Base.ApiKey, uri);
//			var recordList = JsonConvert.DeserializeObject<AirRecordList>(response.Body);
//			return recordList;
//		}

//		//private Uri BuildUriForListRecords(
//		//	string offset,
//		//	IEnumerable<string> fields,
//		//	string filterByFormula,
//		//	int? maxRecords,
//		//	int? pageSize,
//		//	IEnumerable<AirSort> sort,
//		//	string view)
//		//{
//		//	var uriBuilder = new UriBuilder(AirClient.AIRTABLE_API_URL + Base.BaseId + "/" + Name);

//		//	if (!string.IsNullOrEmpty(offset)) {
//		//		AddParametersToQuery(ref uriBuilder, $"offset={HttpUtility.UrlEncode(offset)}");
//		//	}

//		//	if (fields != null) {
//		//		string flattenFieldsParam = QueryParamHelper.FlattenFieldsParam(fields);
//		//		AddParametersToQuery(ref uriBuilder, flattenFieldsParam);
//		//	}

//		//	if (!string.IsNullOrEmpty(filterByFormula)) {
//		//		AddParametersToQuery(ref uriBuilder, $"filterByFormula={HttpUtility.UrlEncode(filterByFormula)}");
//		//	}

//		//	if (sort != null) {
//		//		string flattenSortParam = QueryParamHelper.FlattenSortParam(sort);
//		//		AddParametersToQuery(ref uriBuilder, flattenSortParam);
//		//	}

//		//	if (!string.IsNullOrEmpty(view)) {
//		//		AddParametersToQuery(ref uriBuilder, $"view={HttpUtility.UrlEncode(view)}");
//		//	}

//		//	if (maxRecords != null) {
//		//		if (maxRecords <= 0) {
//		//			throw new ArgumentException("Maximum Number of Records must be > 0", "maxRecords");
//		//		}
//		//		AddParametersToQuery(ref uriBuilder, $"maxRecords={maxRecords}");
//		//	}

//		//	if (pageSize != null) {
//		//		if (pageSize <= 0 || pageSize > MAX_PAGE_SIZE) {
//		//			throw new ArgumentException("Page Size must be > 0 and <= 100", "pageSize");
//		//		}
//		//		AddParametersToQuery(ref uriBuilder, $"pageSize={pageSize}");
//		//	}
//		//	return uriBuilder.Uri;
//		//}

//		//private async Task<AirException> CheckForAirtableException(HttpResponseMessage response)
//		//{
//		//	switch (response.StatusCode)
//		//	{
//		//	case System.Net.HttpStatusCode.OK:
//		//		return null;

//		//	case System.Net.HttpStatusCode.BadRequest:
//		//		return (new AirtableBadRequestException());

//		//	case System.Net.HttpStatusCode.Forbidden:
//		//		return (new AirtableForbiddenException());

//		//	case System.Net.HttpStatusCode.NotFound:
//		//		return (new AirtableNotFoundException());

//		//	case System.Net.HttpStatusCode.PaymentRequired:
//		//		return (new AirtablePaymentRequiredException());

//		//	case System.Net.HttpStatusCode.Unauthorized:
//		//		return (new AirtableUnauthorizedException());

//		//	case System.Net.HttpStatusCode.RequestEntityTooLarge:
//		//		return (new AirtableRequestEntityTooLargeException());

//		//	case (System.Net.HttpStatusCode)422:        // There is no HttpStatusCode.InvalidRequest defined in HttpStatusCode Enumeration.
//		//		var error = await ReadResponseErrorMessage(response);
//		//		return (new AirtableInvalidRequestException(error));

//		//	default:
//		//		throw new AirtableUnrecognizedException(response.StatusCode);
//		//	}
//		//}
//	}

//	//public struct ListParameters
//	//{
//	//	public string Offset { get; set; }
//	//	public IEnumerable<string> Fields { get; set; }
//	//	public string FilterByFormula { get; set; }
//	//	public int? MaxRecords { get; set; }
//	//	public int? PageSize { get; set; } 
//	//	public IEnumerable<QueryParameters> Sort { get; set; }
//	//	public string View { get; set; }

//	//	public Uri GetUri()
//	//	{
//	//		var uriBuilder = new UriBuilder(AirClient.AIRTABLE_API_URL + Base.BaseId + "/" + Name);

//	//		if (!string.IsNullOrEmpty(Offset)) {
//	//			AddParametersToQuery(uriBuilder, "offset", Offset);
//	//		}

//	//		if (Fields != null) {
//	//			string flattenFieldsParam = QueryParamHelper.FlattenFieldsParam(fields);
//	//			AddParametersToQuery(uriBuilder, flattenFieldsParam);
//	//		}

//	//		if (!string.IsNullOrEmpty(FilterByFormula)) {
//	//			AddParametersToQuery(uriBuilder, "filterByFormula", FilterByFormula);
//	//		}

//	//		if (Sort != null) {
//	//			string flattenSortParam = QueryParamHelper.FlattenSortParam(Sort);
//	//			AddParametersToQuery(uriBuilder, flattenSortParam);
//	//		}

//	//		if (!string.IsNullOrEmpty(View)) {
//	//			AddParametersToQuery(uriBuilder, "view", View);
//	//		}

//	//		if (MaxRecords != null) {
//	//			if (MaxRecords <= 0) {
//	//				throw new ArgumentException("Maximum Number of Records must be > 0", nameof(MaxRecords));
//	//			}
//	//			AddParametersToQuery(ref uriBuilder, "maxRecords", MaxRecords, encode:false);
//	//		}

//	//		if (PageSize != null) {
//	//			if (PageSize <= 0 || PageSize > MAX_PAGE_SIZE) {
//	//				throw new ArgumentException("Page Size must be > 0 and <= 100", nameof(PageSize));
//	//			}
//	//			AddParametersToQuery(ref uriBuilder, "pageSize", PageSize, encode:false);
//	//		}

//	//		return uriBuilder.Uri;

//	//	}

//	//	void AddParametersToQuery(UriBuilder uri, string queryToAppend)
//	//	{
//	//		if (uri.Query != null && uri.Query.Length > 1) {
//	//			uri.Query = uri.Query.Substring(1) + "&" + queryToAppend;
//	//		} else {
//	//			uri.Query = queryToAppend;
//	//		}
//	//	}

//	//	void AddParametersToQuery(UriBuilder uri, string paramName, object value, bool encode = true)
//	//	{
//	//		var valueStr = value.ToString();
//	//		if (encode) {
//	//			valueStr = HttpUtility.UrlEncode(valueStr);
//	//		}

//	//		AddParametersToQuery(uri, string.Format("{0}={1}", paramName, valueStr));
// //		}
//	//}
//}
