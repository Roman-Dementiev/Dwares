using System;
using System.Collections.Generic;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;


namespace RouteOptimizer.Models
{
	public class KnownTag
	{
		public KnownTag(string tag, params Type[] types) : this(tag, null, types) {}

		public KnownTag(string tag, string icon, params Type[] types)
		{
			Guard.ArgumentNotEmpty(tag, nameof(tag));

			Tag = tag;
			Icon = icon;
			Types = types;
		}

		public string Tag { get; }
		public string Icon { get; }
		public Type[] Types { get; }
	}

	public static class KnownTags
	{
		public static readonly List<KnownTag> All = new List<KnownTag>() {
			new KnownTag("airport", "ic_airport", typeof(Place)),
			new KnownTag("bar", "ic_bar", typeof(Place)),
			new KnownTag("cafe", "ic_cafe", typeof(Place)),
			new KnownTag("convenience", "ic_store", typeof(Place)),
			new KnownTag("daycare", "ic_child_care", typeof(Place)),
			new KnownTag("dining", "ic_dining", typeof(Place)),
			new KnownTag("doctors", "ic_hospital", typeof(Place)),
			new KnownTag("gas", "ic_gas_station", typeof(Place)),
			new KnownTag("grocery", "ic_grocery", typeof(Place)),
			new KnownTag("hospital", "ic_hospital", typeof(Place)),
			new KnownTag("hotel", "ic_hotel", typeof(Place)),
			new KnownTag("library", "ic_library", typeof(Place)),
			new KnownTag("mall", "ic_mall", typeof(Place)),
			new KnownTag("market", "ic_store", typeof(Place)),
			new KnownTag("movies", "ic_bar", typeof(Place)),
			new KnownTag("parking", "ic_parking", typeof(Place)),
			new KnownTag("pharmacy", "ic_pharmacy", typeof(Place)),
			new KnownTag("pizza", "ic_pizza", typeof(Place)),
			new KnownTag("residential", "ic_home", typeof(Place)),
			new KnownTag("restaurant", "ic_restaurant", typeof(Place)),
			new KnownTag("store", "ic_store", typeof(Place)),
			new KnownTag("adult-daycare", typeof(Place)),
		};

		public static KnownTag Get(string tag)
		{
			foreach (var knownTag in All) {
				if (knownTag.Tag == tag)
					return knownTag;
			}
			return null;
		}

		public static List<KnownTag> GetForType(Type type)
		{
			var list = new List<KnownTag>();
			foreach (var tag in All) {
				if (tag.Types.Contains(type))
					list.Add(tag);
			}
			return list;
		}

		public static List<string> GetTagsListForType(Type type)
		{
			var list = new List<string>();
			foreach (var tag in All) {
				if (tag.Types.Contains(type))
					list.Add(tag.Tag);
			}
			return list;
		}
	}
}
