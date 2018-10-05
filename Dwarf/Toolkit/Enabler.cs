//using System;
//using System.Collections.Generic;
//using System.Threading;


//namespace Dwares.Dwarf.Toolkit
//{
//	public class Enabler<T>
//	{
//		bool? allEnabled = null;
//		Dictionary<T, bool> dict = new Dictionary<T, bool>();

//		public Enabler() : this(false) { }

//		public Enabler(bool exclusive)
//		{
//			Exclusive = exclusive;
//		}

//		public bool Exclusive { get; }

//		public bool AreAllEnabled() => allEnabled == true;
//		public bool AreAllDisabled() => allEnabled == false;
//		public void SetAllEnabled(bool? enabled) => allEnabled = enabled;
//		public void EnableAll() => SetAllEnabled(true);
//		public void DisableAll() => SetAllEnabled(false);
//		public void ResetAll() => SetAllEnabled(null);

//		public void SetEnabled(T value, bool? enabled)
//		{
//			if (enabled == null) {
//				dict.Remove(value);
//			} else if (dict.ContainsKey(value)) {
//				dict[value] = (bool)enabled;
//			} else {
//				dict.Add(value, (bool)enabled);
//			}
//		}

//		public void Enable(T value) => SetEnabled(value, true);
//		public void Disable(T value) => SetEnabled(value, false);
//		public void Remove(T value) => SetEnabled(value, null);

//		public bool IsEnabled(T value)
//		{
//			if (allEnabled != null && !Exclusive)
//				return (bool)allEnabled;

//			bool isEnabled = false;
//			if (dict.ContainsKey(value)) {
//				isEnabled = dict[value];
//			} else if (Exclusive) {
//				return false;
//			}

//			return allEnabled ?? isEnabled;
//		}
//	}
//}
