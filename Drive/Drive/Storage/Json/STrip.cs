using System;
using System.Runtime.Serialization;


namespace Drive.Storage.Json
{
	[DataContract]
	internal struct STrip
	{
		[DataMember(EmitDefaultValue = false)]
		public string TripType { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string ClientPhone { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public DateTime PickupTime { get; set; }
	}
}
