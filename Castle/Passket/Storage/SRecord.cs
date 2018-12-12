using System;
using System.Runtime.Serialization;
using Passket.Models;

namespace Passket.Storage
{
	[DataContract]
	internal struct SEntry
	{
		[DataMember(Name = "Name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "Kind", EmitDefaultValue = false)]
		public string Kind { get; set; }

		[DataMember(Name = "Value", EmitDefaultValue = false)]
		public string Value { get; set; }
	}



	[DataContract]
	internal struct SRecord
	{
		[DataMember(Name = "Icon", EmitDefaultValue = false)]
		public string Icon { get; set; }

		[DataMember(Name = "Name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "Info", EmitDefaultValue = false)]
		public string Info { get; set; }

		[DataMember(Name = "Entries", EmitDefaultValue = false)]
		public SEntry[] Entries { get; set; }

		public static implicit operator SRecord(Record record)
		{

			int eCount = record.Entries.Count;

			var srec = new SRecord {
				Icon = record.Icon,
				Name = record.Name,
				Info = record.Info,
				Entries = new SEntry[eCount]
			};

			for (int i = 0; i < eCount; i++) {
				var entry = record.Entries[i];
				srec.Entries[i] = new SEntry {
					Name = entry.Name,
					Kind = entry.Kind.Key,
					Value = entry.Value?.ToString()
				};
			}

			return srec;
		}
	}
}
