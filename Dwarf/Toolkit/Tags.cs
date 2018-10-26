using System;
using System.Collections.Generic;


namespace Dwares.Dwarf.Toolkit
{
	//public class TagChangedEventArgs : EventArgs
	//{
	//	public TagChangedEventArgs(string tag, bool value)
	//	{
	//		Tag = tag;
	//		Value = value;
	//	}

	//	public string Tag { get; }
	//	public bool Value{ get; }
	//}

	//public delegate void TagsChangedEventHabdler(object sender, TagChangedEventArgs e);

	public class Tags
	{
		//public event TagsChangedEventHabdler TagChanged;
		Dictionary<string, object> dict = new Dictionary<string, object>();

		public ICollection<string> GetTags()
		{
			return dict.Keys;
		}

		public bool HasTag(string tag)
		{
			return dict.ContainsKey(tag);
		}

		public void AddTag(string tag)
		{
			if (!dict.ContainsKey(tag)) {
				dict.Add(tag, null);
			}
		}

		public bool RemoveTag(string tag)
		{
			return dict.Remove(tag);
		}
	
		public void SwitchTag(string tag, bool onOff)
		{
			if (onOff) {
				AddTag(tag);
			} else {
				RemoveTag(tag);
			}
		}

		public void SetTagValue<T>(string tag, T value)
		{
			dict[tag] = value;
		}

		public bool GetTagValue<T>(string tag, out T value)
		{
			if (dict.TryGetValue(tag, out object val)) {
				if (val is T) {
					value = (T)val;
					return true;
				}
			}

			value = default(T);
			return false;
		}

		public T GetTagValue<T>(string tag, T defaultValue=default(T))
		{
			if (dict.TryGetValue(tag, out object val)) {
				if (val is T) {
					return (T)val;
				}
			}

			return defaultValue;
		}

		//protected void RaiseTagChanged(string  tag, bool value)
		//{
		//	if (TagChanged != null) {
		//		var args = new TagChangedEventArgs(tag, value);
		//		TagChanged(this, args);
		//	}
		//}
	}
}
