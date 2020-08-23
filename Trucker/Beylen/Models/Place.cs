using System;
using Dwares.Dwarf;


namespace Beylen.Models
{
	public class Place : Model
	{
		//static ClassRef @class = new ClassRef(typeof(Place));

		public Place()
		{
			//Debug.EnableTracing(@class);
		}

		public string CodeName {
			get => codeName;
			set => SetProperty(ref codeName, value);
		}
		string codeName;

		public string RealName {
			get => realName;
			set => SetProperty(ref realName, value);
		}
		string realName;

		public string Alias {
			get => alias;
			set => SetProperty(ref alias, value);
		}
		string alias;

		public string DisplayName {
			get {
				if (Settings.UseRealNames) {
					return RealName;
				} else if (!string.IsNullOrEmpty(Alias)) {
					return $"{CodeName} ({Alias})";
				} else {
					return CodeName;
				}
			}
		}

		public string Address {
			get => address;
			set => SetProperty(ref address, value);
		}
		string address;

	}
}
