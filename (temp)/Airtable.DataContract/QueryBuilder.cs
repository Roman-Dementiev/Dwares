using System;
using System.Collections.Generic;
using System.Web;
using Dwares.Dwarf;


namespace Dwares.Drudge.Airtable
{
	public class QyeryBuilder
	{
		//static ClassRef @class = new ClassRef(typeof(QyeryBuilder));

		public const int MAX_PAGE_SIZE = 100;

		public string Offset { get; set; }
		public IEnumerable<string> Fields { get; set; }
		public string FilterByFormula { get; set; }
		public int? MaxRecords { get; set; }
		public int? PageSize { get; set; }
		public IEnumerable<Sort> Sort { get; set; }
		public string View { get; set; }


		public QyeryBuilder()
		{
			//Debug.EnableTracing(@class);
		}

		public Uri GetUri(string baseId, string tableName)
		{
			var uriBuilder = new UriBuilder(AirClient.TableUri(baseId, tableName));

			if (!string.IsNullOrEmpty(Offset)) {
				AddParametersToQuery(uriBuilder, "offset", Offset);
			}

			if (Fields != null) {
				string flattenFieldsParam = FlattenFieldsParam(Fields);
				AddParametersToQuery(uriBuilder, flattenFieldsParam);
			}

			if (!string.IsNullOrEmpty(FilterByFormula)) {
				AddParametersToQuery(uriBuilder, "filterByFormula", FilterByFormula);
			}

			if (Sort != null) {
				string flattenSortParam = FlattenSortParam(Sort);
				AddParametersToQuery(uriBuilder, flattenSortParam);
			}

			if (!string.IsNullOrEmpty(View)) {
				AddParametersToQuery(uriBuilder, "view", View);
			}

			if (MaxRecords != null) {
				if (MaxRecords <= 0) {
					throw new ArgumentException("Maximum Number of Records must be > 0", nameof(MaxRecords));
				}
				AddParametersToQuery(uriBuilder, "maxRecords", MaxRecords, encode: false);
			}

			if (PageSize != null) {
				if (PageSize <= 0 || PageSize > MAX_PAGE_SIZE) {
					throw new ArgumentException("Page Size must be > 0 and <= 100", nameof(PageSize));
				}
				AddParametersToQuery(uriBuilder, "pageSize", PageSize, encode: false);
			}

			return uriBuilder.Uri;

		}

		static void AddParametersToQuery(UriBuilder uri, string queryToAppend)
		{
			if (uri.Query != null && uri.Query.Length > 1) {
				uri.Query = uri.Query.Substring(1) + "&" + queryToAppend;
			}
			else {
				uri.Query = queryToAppend;
			}
		}

		static void AddParametersToQuery(UriBuilder uri, string paramName, object value, bool encode = true)
		{
			var valueStr = value.ToString();
			if (encode) {
				valueStr = HttpUtility.UrlEncode(valueStr);
			}

			AddParametersToQuery(uri, string.Format("{0}={1}", paramName, valueStr));
		}

		static internal string FlattenSortParam(IEnumerable<Sort> sort)
		{
			int i = 0;
			string flattenSortParam = string.Empty;
			string toInsert = string.Empty;
			foreach (var sortItem in sort) {
				if (string.IsNullOrEmpty(toInsert) && i > 0) {
					toInsert = "&";
				}

				// Name of fields to be sorted
				string param = $"sort[{i}][field]";
				flattenSortParam += $"{toInsert}{HttpUtility.UrlEncode(param)}={HttpUtility.UrlEncode(sortItem.Fields)}";

				// Direction for sorting
				param = $"sort[{i}][direction]";
				flattenSortParam += $"&{HttpUtility.UrlEncode(param)}={HttpUtility.UrlEncode(sortItem.Direction.ToString().ToLower())}";
				i++;
			}
			return flattenSortParam;
		}


		static internal string FlattenFieldsParam(IEnumerable<string> fields)
		{
			int i = 0;
			string flattenFieldsParam = string.Empty;
			string toInsert = string.Empty;
			foreach (var fieldName in fields) {
				if (string.IsNullOrEmpty(toInsert) && i > 0) {
					toInsert = "&";
				}
				string param = $"fields[{i}]";
				flattenFieldsParam += $"{toInsert}{HttpUtility.UrlEncode(param)}={HttpUtility.UrlEncode(fieldName)}";
				i++;
			}
			return flattenFieldsParam;
		}

	}
}
