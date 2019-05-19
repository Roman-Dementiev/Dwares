using System;
using System.Runtime.Serialization;


namespace Drive.Storage.Json
{
	[DataContract]
	internal struct SContact
	{
		[DataMember(EmitDefaultValue = false)]
		public string ContactType { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string PhoneNumber { get; set; }

		[DataMember(EmitDefaultValue = false)]
		public string Address { get; set; }
	}
}
