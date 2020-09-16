using System;
using System.Collections.Generic;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;


namespace RouteOptimizer.Models
{
	public class Category
	{
		//public Category(string tag, string title, params Type[] types) : this(tag, title, null, types) {}

		public Category(string tag, string icon, string title, params Type[] types)
		{
			Guard.ArgumentNotEmpty(tag, nameof(tag));

			Tag = tag;
			Title = title ?? tag;
			Icon = icon;
			Types = types;
		}

		public string Tag { get; }
		public string Title { get; }
		public string Icon { get; }
		public Type[] Types { get; }
	}

	public static class Categories
	{
		public static readonly List<Category> All = new List<Category>() {
			new Category("adult-daycare", "ic_face", "Adult Day Care", typeof(Place)),
			new Category("airport", "ic_airport", "Airport", typeof(Place)),
			new Category("bar", "ic_bar", "Bar", typeof(Place)),
			new Category("cafe", "ic_cafe", "Cafe", typeof(Place)),
			new Category("community-center", "ic_library", "Community Center", typeof(Place)),
			new Category("convenience", "ic_store", "Convenience Store", typeof(Place)),
			new Category("daycare", "ic_child_care", "Day Care", typeof(Place)),
			new Category("dining", "ic_dining", "Dining facility", typeof(Place)),
			new Category("doctors", "ic_hospital", "Doctors office", typeof(Place)),
			new Category("gas", "ic_gas_station", "Gas station", typeof(Place)),
			new Category("grocery", "ic_grocery", "Grocery store", typeof(Place)),
			new Category("hospital", "ic_hospital", "Hospital", typeof(Place)),
			new Category("hotel", "ic_hotel", "Hotel", typeof(Place)),
			new Category("library", "ic_library", "Library", typeof(Place)),
			new Category("mall", "ic_mall", "Mall", typeof(Place)),
			new Category("market", "ic_store", "Store", typeof(Place)),
			new Category("movies", "ic_movies", "Movie Theatre", typeof(Place)),
			new Category("parking", "ic_parking", "Parking", typeof(Place)),
			new Category("pharmacy", "ic_pharmacy", "Pharmacy", typeof(Place)),
			new Category("pizza", "ic_pizza", "Pizza", typeof(Place)),
			new Category("residential", "ic_home", "Residential", typeof(Place)),
			new Category("restaurant", "ic_restaurant", "Restaurant", typeof(Place)),
			new Category("store", "ic_store", "Store", typeof(Place))
		};

		public static Category GetCategory(string tag)
		{
			foreach (var knownTag in All) {
				if (knownTag.Tag == tag)
					return knownTag;
			}
			return null;
		}

		public static List<Category> GetForType(Type type)
		{
			var list = new List<Category>();
			foreach (var tag in All) {
				if (tag.Types.Contains(type))
					list.Add(tag);
			}
			return list;
		}

		public static List<string> GetTagsForType(Type type)
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
