using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Toolkit
{
	public class GeneralName
	{
		string fullName;
		string[] nameParts;

		public GeneralName()
		{
			this.fullName = String.Empty;
			nameParts = null;
		}

		public GeneralName(string fullName, bool normalize = false)
		{
			this.fullName = fullName ?? String.Empty;
			nameParts = null;

			if (normalize) {
				Normalize();
			}
		}

		public override string ToString() => FullName;

		public string FullName {
			get => fullName ?? String.Empty;
			set => fullName = value;
		}

		public string[] NameParts {
			get {
				if (nameParts == null) {
					if (String.IsNullOrEmpty(FullName)) {
						return Strings.EmptyArray;
					}
					nameParts = FullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				}
				return nameParts;
			}
		}

		public void Normalize()
		{
			var nameParts = NameParts;
			for (int i = 0; i < nameParts.Length; i++) {
				nameParts[i] = Strings.CapitalizeFirstLetter(nameParts[i]);
			}
			fullName = String.Join(" ", nameParts);
		}

		public static implicit operator string(GeneralName name) => name.FullName;
		public static implicit operator GeneralName(string name) => new GeneralName(name);
	}
}
