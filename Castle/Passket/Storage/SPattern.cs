using System;
using System.Runtime.Serialization;
using Passket.Models;


namespace Passket.Storage
{
	[DataContract]
	internal struct SField
	{
		[DataMember(Name = "Name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "Kind", EmitDefaultValue = false)]
		public string Kind { get; set; }
	}

	[DataContract]
	internal struct SPattern
	{
		[DataMember(Name = "Fields", EmitDefaultValue = false)]
		public SField[] Fields { get; set; }

		//public SPattern(Pattern pattern)
		//{
		//	int fCount = pattern.Fields.Count;
		//	Fields = new SField[fCount];

		//	for (int i = 0; i < fCount; i++) {
		//		var field = pattern.Fields[i];
		//		Fields[i] = new SField { 
		//			Name = field.Name,
		//			Kind = field.Kind.Key
		//		};
		//	}
		//}

		public static implicit operator SPattern(Pattern pattern)
		{

			int fCount = pattern.Fields.Count;

			var spat  = new SPattern {
				Fields = new SField[fCount]
			};

			for (int i = 0; i < fCount; i++) {
				var field = pattern.Fields[i];
				spat.Fields[i] = new SField {
					Name = field.Name,
					Kind = field.Kind.Key
				};
			}

			return spat;
		}

		public static implicit operator Pattern(SPattern spat)
		{
			var pattern = new Pattern();

			for (int i = 0; i < spat.Fields.Length; i++) {
				var sf = spat.Fields[i];
				var field = new Pattern.Field {
					Name = sf.Name,
					Kind = EntryKind.ForKey(sf.Kind)
				};
				pattern.Fields.Add(field);
			}

			return pattern;
		}
	}
}
