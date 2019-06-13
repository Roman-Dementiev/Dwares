using System;
using System.Collections.Generic;
using Dwares.Dwarf;


namespace Drive.Models
{
	public class Tags : List<Tag>
	{
		//static ClassRef @class = new ClassRef(typeof(Tags));

		public Tags()
		{
			//Debug.EnableTracing(@class);
		}

		public bool HasTags(string name)
			=> GetTag(name) != null;

		public Tag GetTag(string name)
		{
			foreach (var tag in this) {
				if (tag.Name == name)
					return tag;
			}
			return null;
		}

		public Tags GetTags(IEnumerable<string> names, bool knownOnly)
		{
			return GetTags(names, this, knownOnly);
		}

		public static Tags GetTags(IEnumerable<string> names, Tags knownTags, bool knownOnly)
		{
			var tags = new Tags();
			foreach (var name in names) {
				var tag = knownTags?.GetTag(name);
				if (tag == null) {
					if (knownOnly) {
						Debug.Print($"AirStorage.GetTags(): Unknown tag '{name}'");
						continue;
					}

					tag = new Tag(name);
				}
				tags.Add(tag);
			}
			return tags;
		}

		public static Tags NewTags(IEnumerable<string> names)
		{
			return GetTags(names, null, false);
		}
	}

	public class Tag
	{
		public Tag(string name, IEnumerable<string> applyTo = null)
		{
			Guard.ArgumentNotEmpty(name, nameof(name));

			Name = name;
			if (applyTo != null) {
				ApplyTo = new List<string>(applyTo);
			}
		}

		public string Name { get; }
		public List<string> ApplyTo { get; }

		public bool ApplicableTo(string name)
		{
			if (ApplyTo == null)
				return true;

			foreach (var value in ApplyTo) {
				if (value == name)
					return true;
			}
			return false;
		}

		public bool ApplicableToBases => ApplicableTo("Bases");
		public bool ApplicableToPhones => ApplicableTo("Phones");
		public bool ApplicableToPlaces => ApplicableTo("Places");
	}
}
