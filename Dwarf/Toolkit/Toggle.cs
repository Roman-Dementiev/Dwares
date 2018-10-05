//using System;


//namespace Dwares.Dwarf.Toolkit
//{
//	public interface IToggle
//	{
//		bool? ToggleState { get; set; }
//	}

//	public struct Toggle: IToggle
//	{
//		public bool? ToggleState { get; set; }
//	}

//	public static class Extensions
//	{
//		public static bool IsOn(this IToggle toggle) => toggle.ToggleState == true;
//		public static bool IsOff(this IToggle toggle) => toggle.ToggleState == false;
//		public static bool IsUndefined(this IToggle toggle) => toggle.ToggleState == null;

//		public static bool IsOn(this IToggle toggle, Func<bool> @delegate)
//		{
//			if (toggle.ToggleState != null) {
//				return (bool)toggle.ToggleState;
//			} else if (@delegate != null) {
//				return @delegate();
//			} else {
//				return false;
//			}
//		}

//		public static bool IsOff(this IToggle toggle, Func<bool> @delegate = null)
//		{
//			if (toggle.ToggleState != null) {
//				return !(bool)toggle.ToggleState;
//			} else if (@delegate != null) {
//				return !@delegate();
//			} else {
//				return false;
//			}
//		}

//		public static bool IsOn<T>(this T toggle, Func<T, T> @delegate, bool @default = false) where T : IToggle
//		{
//			while (toggle != null) {
//				if (toggle.ToggleState != null)
//					return (bool)toggle.ToggleState;

//				if (@delegate != null) {
//					toggle = @delegate(toggle);
//				} else
//					break;
//			}
			
//			return @default;
//		}

//		public static bool IsOff(this IToggle toggle, Func<IToggle, IToggle> @delegate = null, bool @default = false)
//		{
//			while (toggle != null) {
//				if (toggle.ToggleState != null)
//					return !(bool)toggle.ToggleState;

//				if (@delegate != null) {
//					toggle = @delegate(toggle);
//				} else
//					break;
//			}

//			return @default;
//		}

//		public static bool IsUndefined(this IToggle toggle, Func<IToggle, IToggle> @delegate = null)
//		{
//			while (toggle != null) {
//				if (toggle.ToggleState != null)
//					return false;

//				if (@delegate != null) {
//					toggle = @delegate(toggle);
//				} else
//					break;
//			}

//			return true;
//		}
//	}
//}
