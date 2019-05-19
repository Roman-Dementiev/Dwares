using System;
using System.Threading.Tasks;
using Dwares.Dwarf;


namespace Dwares.Drudge.Airtable
{
	public class AirBase
	{
		//static ClassRef @class = new ClassRef(typeof(ABase));

		public AirBase(string apiKey, string baseId)
		{
			//Debug.EnableTracing(@class);

			ApiKey = Guard.ArgumentNotEmpty(apiKey, nameof(apiKey));
			BaseId = Guard.ArgumentNotEmpty(baseId, nameof(baseId));
		}

		public virtual Task Initialize()
		{
			return Task.CompletedTask;
		}

		public string ApiKey { get; }
		public string BaseId { get; }


		//public virtual Exception Authenticate()
		//{
		//	return null;
		//}
	}
}
