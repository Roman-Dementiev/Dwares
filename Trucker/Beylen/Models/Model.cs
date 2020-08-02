using System;
using Dwares.Dwarf;
using Dwares.Druid;
using System.Collections.Generic;
using System.Linq;

namespace Beylen.Models
{
	public class Model : Dwares.Druid.Model
	{
		//static ClassRef @class = new ClassRef(typeof(Model));

		public Model()
		{
			//Debug.EnableTracing(@class);
		}

		public List<string> TagsList { get; private set; }

		public string Tags {
			get => TagsToString();
			set {
				TagsList?.Clear();
				AddTagsFromString(value);
			}
		}

		public bool HasTag(string tag)
		{
			return TagsList?.Contains(tag) == true;
		}

		public void AddTag(string tag)
		{
			if (TagsList == null) {
				TagsList = new List<string>();
			} else if (TagsList.Contains(tag)) {
				return;
			}

			TagsList.Add(tag);
		}

		public bool RemoveTag(string tag)
		{
			return TagsList?.Remove(tag) == true;
		}

		public void ClearTags()
		{
			TagsList = null;
		}

		public string TagsToString()
		{
			if (TagsList == null || TagsList.Count == 0)
				return string.Empty;

			return string.Join(";", TagsList);
		}

		public void AddTagsFromString(string str)
		{
			if (string.IsNullOrEmpty(str))
				return;

			var split = str.Split(new char[] { ';'}, StringSplitOptions.RemoveEmptyEntries);
			if (split.Length == 0)
				return;

			if (TagsList == null)
				TagsList = new List<string>();

			foreach (var tag in split) {
				if (!TagsList.Contains(tag))
					TagsList.Add(tag);
			}
		}
	}
}
