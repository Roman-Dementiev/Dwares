using System;
using Xamarin.Forms;


namespace Dwares.Druid.Support
{
	public static class Bind
	{
		public static readonly BindableProperty UidProperty = BindableProperty.CreateAttached(
			"Uid",
			typeof(string),
			typeof(Bind),
			null
		);
		public static void SetUid(BindableObject obj, string uid) => obj.SetValue(UidProperty, uid);
		public static string GetUid(BindableObject obj) => (string)obj.GetValue(UidProperty);
	}
}
