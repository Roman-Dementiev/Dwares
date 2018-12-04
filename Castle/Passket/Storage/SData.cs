using System;
using System.Runtime.Serialization;


namespace Passket.Storage
{
	[DataContract]
	internal struct SData
	{
		public const string cVersion1 = "1.0";

		[DataMember(Name = "Version", EmitDefaultValue = false)]
		public string Version { get; set; }

		[DataMember(Name = "Patterns", EmitDefaultValue = false)]
		public SPattern[] Patterns { get; set; }

		public SData(string version)
		{
			Version = version;
			
			var patterns = AppData.Patterns;
			Patterns = new SPattern[patterns.Count];
			for (int i = 0; i < patterns.Count; i++) {
				Patterns[i] = patterns[i];
			}
		}
	}
}
