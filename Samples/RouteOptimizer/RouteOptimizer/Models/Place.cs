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

		public string Id => Ids.PlaceId(this);

		public string Name {
			get => name;
			set => SetPropertyEx(ref name, value, nameof(Name), nameof(Id));
		}
		string name = string.Empty;

		public string Icon {
			get => icon;
			private set => SetProperty(ref icon, value);
		}
		string icon;

		public Category Category {
			get => category;
			private set => SetProperty(ref category, value);
		}
		Category category;

		public string Address {
			get => address;
			set => SetPropertyEx(ref address, value, nameof(Address), nameof(Id));
		}
		string address = string.Empty;

		public string Phone {
			get => phone;
			set => SetPropertyEx(ref phone, value);
		}
		string phone = string.Empty;

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
