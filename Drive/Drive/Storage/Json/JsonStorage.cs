//using System;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;
//using System.Threading.Tasks;
//using Drive.Models;


//namespace Drive.Storage.Json
//{
//	public class JsonStorage : IAppStorage
//	{
//		WeakReference<Json> jsonRef;

//		Json GetJson()
//		{
//			Json json;
//			if (!jsonRef.TryGetTarget(out json)) {
//				var ser = new DataContractJsonSerializer(typeof(Json));
//				json = ser.ReadObject(responseStream) as Json;
//			}
//			return json;
//		}

//		public Task LoadContacts(IList<Contact> contacts)
//		{

//		}

//		public Task SaveContacts(IList<Contact> contacts)
//		{

//		}

//		public Task LoadSchedule(IList<ScheduleTrip> schedule)
//		{

//		}

//		public  Task SaveSchedule(IList<ScheduleTrip> schedule)
//		{

//		}
//	}


//	[DataContract]
//	internal class Json
//	{
//		[DataMember(EmitDefaultValue = false)]
//		List<SContact> Contacts { get; set; }

//		[DataMember(EmitDefaultValue = false)]
//		List<STrip> Trips { get; set; }
//	}
//}
