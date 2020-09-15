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
			new KnownTag("restaurant", typeof(Place)),
			new KnownTag("market", typeof(Place)),
			new KnownTag("store", typeof(Place)),
			new KnownTag("pharmacy", typeof(Place)),
			new KnownTag("convenience", typeof(Place)),
			new KnownTag("gas", typeof(Place)),
			new KnownTag("hospital", typeof(Place)),
			new KnownTag("doctors", typeof(Place)),
			new KnownTag("daycare", typeof(Place)),
			new KnownTag("adult-daycare", typeof(Place)),
			new KnownTag("residential", typeof(Place)),
		};

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
