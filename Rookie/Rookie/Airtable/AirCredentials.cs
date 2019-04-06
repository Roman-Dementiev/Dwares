//using System;
//using Dwares.Dwarf;
//using Dwares.Dwarf.Data;
//using Dwares.Dwarf.Toolkit;


//namespace Dwares.Rookie.Airtable
//{
//	public struct AirCredentials : ICredentials
//	{
//		public AirCredentials(string apiKey, string baseId)
//		{
//			Guard.ArgumentNotEmpty(apiKey, nameof(apiKey));
//			Guard.ArgumentNotEmpty(baseId, nameof(baseId));

//			ApiKey = apiKey;
//			BaseId = baseId;
//		}

//		public string ApiKey { get; set; }
//		public string BaseId { get; set; }

//		public object AuthenticationScheme => AirClient.AUTHENTICATION_SCHEME;
//		public object AuthenticationKey => ApiKey;
//	}
//}
