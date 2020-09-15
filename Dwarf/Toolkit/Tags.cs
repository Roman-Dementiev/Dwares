using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Dwares.Dwarf.Toolkit
{
	public interface ITags
	{
		bool HasTag(string tag);
		void AddTag(string tag);
		bool RemoveTag(string tag);
		void Clear();

		void FromString(string str);

		List<string> GetList();
	}


	public class TagsSet : ITags
	{
		HashSet<string> tags = new HashSet<string>();

		public TagsSet() { }

		public TagsSet(string str)
		{
			this.Parse(str);
		}

		public bool HasTag(string tag)
		{
			tag = Tags.NormalizeTag(tag);
			return tags.Contains(tag);
		}

		public void AddTag(string tag)
		{
			tag = Tags.NormalizeTag(tag);
			tags.Add(tag);
		}

		public bool RemoveTag(string tag)
		{
			tag = Tags.NormalizeTag(tag);
			return tags.Remove(tag);
		}
	
		public void Clear()
		{
			tags.Clear();
		}

		public void FromString(string str)
		{
			Clear();
			this.Parse(str);
		}

		public override string ToString()
		{
			return Tags.ToString(tags);
		}

		public List<string> GetList()
		{
			var list = new List<string>();
			foreach (var tag in tags) {
				list.Add(tag);
			}

			return list;
		}

		public static implicit operator string (TagsSet tags) => tags.ToString();
		public static explicit operator TagsSet(string str) => new TagsSet(str);
	}

	public class TagsList : ITags
	{
		List<string> tags = new List<string>();

		public TagsList() { }

		public TagsList(string str)
		{
			this.Parse(str);
		}

		public bool HasTag(string tag)
		{
			tag = Tags.NormalizeTag(tag);
			return tags.Contains(tag);
		}

		public void AddTag(string tag)
		{
			tag = Tags.NormalizeTag(tag);
			if (!tags.Contains(tag))
				tags.Add(tag);
		}

		public bool RemoveTag(string tag)
		{
			tag = Tags.NormalizeTag(tag);
			return tags.Remove(tag);
		}

		public void Clear()
		{
			tags.Clear();
		}
		public void FromString(string str)
		{
			Clear();
			this.Parse(str);
		}
		public override string ToString()
		{
			return Tags.ToString(tags);
		}

		public List<string> GetList()
		{
			return tags;
		}


		public static implicit operator string(TagsList tags) => tags.ToString();
		public static explicit operator TagsList(string str) => new TagsList(str);
	}

	public static class Tags
	{
		static char[] separators = new char[] {' ', '\r', '\n', '\t' };
		static char replaceBlank = '-';

		public static string NormalizeTag(string tag)
		{
			if (tag.IndexOfAny(separators) < 0)
				return tag;

			string result = string.Empty;

			for (int i = 0; i < tag.Length; i++)
			{
				char c = tag[i];
				if (char.IsWhiteSpace(c)) {
					while (i < tag.Length-1 && char.IsWhiteSpace(tag[i+1]))
						i++;
					result += replaceBlank;
				}
				else {
					result += c;
				}
			}

			return result;
		}

		public static void Parse(this ITags tags, string str)
		{
			var split = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			foreach (var tag in split) {
				tags.AddTag(tag);
			}
		}

		public static string ToString(IEnumerable<string> tags)
		{
			string str = string.Empty;
			foreach (var tag in tags) {
				if (str.Length > 0)
					str += " ";
				str += tag;
			}
			return str;
		}

		public static void SetTag(this ITags tags, string tag, bool onOff)
		{
			if (onOff) {
				tags.AddTag(tag);
			} else {
				tags.RemoveTag(tag);
			}
		}

	}
}
