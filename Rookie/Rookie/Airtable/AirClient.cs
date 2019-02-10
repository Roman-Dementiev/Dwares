using System;
using Dwares.Dwarf;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Threading;
using System.Text;

namespace Dwares.Rookie.Airtable
{
	public class AirClient : HttpClient
	{
		//static ClassRef @class = new ClassRef(typeof(AirClient));

		public const string AIRTABLE_API_URL = "https://api.airtable.com/v0/";
		public const string AUTHENTICATION_SCHEME = "Bearer";

		static AirClient instance;
		public static AirClient Instance {
			get => LazyInitializer.EnsureInitialized(ref instance);
			set {
				instance?.Dispose();
				instance = value;
			}
		}

		public AirClient()
		{
			//Debug.EnableTracing(@class);
		}

		public AirClient(DelegatingHandler delegatingHandler) :
			base(delegatingHandler)
		{
			//Debug.EnableTracing(@class);
		}

		public async Task<AirResponse> SendRequestAsync(HttpMethod method, string apiKey, Uri uri, string contentJson, bool throwError)
		{
			var request = new HttpRequestMessage(method, uri);
			request.Headers.Authorization = new AuthenticationHeaderValue(AUTHENTICATION_SCHEME, apiKey);
			if (contentJson != null) {
				request.Content = new StringContent(contentJson, Encoding.UTF8, "application/json");
			}

			var response = await base.SendAsync(request);
			var error = await AirException.CheckStatus(response);
			if (error != null) {
				if (throwError) {
					throw error;
				} else {
					return new AirResponse(error);
				}
			}

			var responseBody = await response.Content.ReadAsStringAsync();
			return new AirResponse(responseBody);
		}

		public static Task<AirResponse> SendAsync(HttpMethod method, string apiKey, Uri uri, string contentJson, bool throwError = true)
			=> Instance.SendRequestAsync(method, apiKey, uri, contentJson, throwError);

		public static Task<AirResponse> GetAsync(string apiKey, Uri uri, bool throwError = true)
			=> Instance.SendRequestAsync(HttpMethod.Get, apiKey, uri, null, throwError);

		public static Task<AirResponse> DeleteAsync(string apiKey, Uri uri, bool throwError = true) 
			=> Instance.SendRequestAsync(HttpMethod.Delete, apiKey, uri, null, throwError);

		public static Task<AirResponse> PostAsync(string apiKey, Uri uri, string contentJson, bool throwError = true)
			=> Instance.SendRequestAsync(HttpMethod.Post, apiKey, uri, contentJson, throwError);

		public static string TableUri(string baseId, string table)
		{
			return AIRTABLE_API_URL + baseId + "/" + table;
		}

		public static string RecordUri(string baseId, string table, string recordId)
		{
			var uri = TableUri(baseId, table) + "/";
			if (!string.IsNullOrEmpty(recordId)) {
				uri += recordId + "/";
			}
			return uri;
		}
	}
}
