using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Toolkit
{
	public class TagChangedEventArgs : EventArgs
	{
		public TagChangedEventArgs(string tag, bool value)
		{
			Tag = tag;
			Value = value;
		}

		public string Tag { get; }
		public bool Value{ get; }
	}

	public delegate void TagsChangedEventHabdler(object sender, TagChangedEventArgs e);

	public class Tags
	{
		public event TagsChangedEventHabdler TagChanged;
		HashSet<string> tags = new HashSet<string>();

		public bool HasTag(string tag)
		{
			return tags.Contains(tag);
		}

		public bool AddTag(string tag)
		{
			if (tags.Add(tag)) {
				RaiseTagChanged(tag, true);
				return true;
			} else {
				return false;
			}
		}

		public bool RemoveTag(string tag)
		{
			if (tags.Remove(tag)) {
				RaiseTagChanged(tag, false);
				return true;
			} else {
				return false;
			}
		}
	
		public void SetTag(string tag, bool value)
		{
			if (value) {
				AddTag(tag);
			} else {
				RemoveTag(tag);
			}
		}

		protected void RaiseTagChanged(string  tag, bool value)
		{
			if (TagChanged != null) {
				var args = new TagChangedEventArgs(tag, value);
				TagChanged(this, args);
			}
		}
	}
}
