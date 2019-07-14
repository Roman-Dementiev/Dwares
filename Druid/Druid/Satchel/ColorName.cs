using System;
using Dwares.Dwarf;


namespace Dwares.Druid.Satchel
{
	public struct ColorName
	{
		public ColorName(string name)
		{
			string variant;
			SplitName(ref name, out variant);
			Name = name;
			Variant = variant;
		}

		static void SplitName(ref string name, out string variant)
		{
			variant = null;
			if (!string.IsNullOrEmpty(name)) {
				int sep = name.IndexOf(':');
				if (sep >= 0) {
					if (sep < name.Length) {
						variant = name.Substring(sep+1);
					}
					name = name.Substring(0, sep);
				}
			}
		}

		public string Name { get; set; }
		public string Variant { get; set; }
		public bool IsValid => !string.IsNullOrEmpty(Name);

		public override string ToString()
		{
			if (IsValid && !string.IsNullOrEmpty(Variant)) {
				return $"{Name}:{Variant}";
			} else {
				return Name;
			}
		}

		public static implicit operator string(ColorName name)
			=> name.ToString();
	}
}
