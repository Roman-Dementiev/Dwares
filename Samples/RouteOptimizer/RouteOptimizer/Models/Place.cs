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

		//public string Tags {
		//	get => tags;
		//	set => SetProperty(ref tags, value);
		//}
		//string tags = string.Empty;
		public TagsList Tags {
			get => tags;
			set {
				if (SetProperty(ref tags, value)) {
					icon = null;
				}
			}
		}
		TagsList tags = new TagsList();

		public string Address {
			get => address;
			set => SetPropertyEx(ref address, value, nameof(Address), nameof(Id));
		}
		string address = string.Empty;

		public string Icon {
			get {
				if (icon == null) {
					icon = string.Empty;
					var list = KnownTags.GetTagsListForType(typeof(Place));

					foreach (var tag in Tags) {
						if (list.Contains(tag)) {
							var knownTag = KnownTags.Get(tag);
							if (!string.IsNullOrEmpty(knownTag.Icon)) {
								icon = knownTag.Icon;
								break;
							}
						}
					}
				}
				return icon;
			}
		}
		string icon;
	}
}
