using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Dwarf.Toolkit;

namespace RouteOptimizer.Models
{
	public class Place : Model
	{
		//static ClassRef @class = new ClassRef(typeof(DwarfClass1));

		public Place()
		{
			//Debug.EnableTracing(@class);
		}

		public string Id {
			get => id;
			set => SetProperty(ref id, value);
		}
		string id;

		public Category Category {
			get => category;
			private set => SetProperty(ref category, value);
		}
		Category category;

		public string Name {
			get => name;
			set => SetProperty(ref name, value);
		}
		string name = string.Empty;

		public string Icon {
			get => icon;
			private set => SetProperty(ref icon, value);
		}
		string icon;

		public string Note {
			get => note;
			set => SetProperty(ref note, value);
		}
		string note = string.Empty;

		public string Phone {
			get => phone;
			set => SetProperty(ref phone, value);
		}
		string phone = string.Empty;


		public string Address {
			get => address;
			set => SetProperty(ref address, value);
		}
		string address = string.Empty;

		public TagsList TagsList { get; } = new TagsList();

		public string Tags {
			get => TagsList.ToString();
			set {
				TagsList.FromString(value);
				UpdateCategoryAndIcon();
			}
		}

		void UpdateCategoryAndIcon()
		{
			Category = null;
			Icon = null;

			var categories = Categories.GetForType(typeof(Place));

			foreach (var cat in categories)
			{
				if (TagsList.HasTag(cat.Tag)) {
					if (Category == null) {
						Category = cat;
					}
					if (!string.IsNullOrEmpty(cat.Icon)) {
						Icon = cat.Icon;
						break;
					}
				}
			}
		}
	}
}
