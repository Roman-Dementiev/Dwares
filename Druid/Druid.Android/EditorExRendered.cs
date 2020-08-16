using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Dwares.Druid.UI;

//using AndroidResources = Android.Content.Res.Resources;


[assembly: ExportRenderer(typeof(Dwares.Druid.UI.EditorEx), typeof(Dwares.Druid.Android.EditorExRendered))]

namespace Dwares.Druid.Android
{
	class EditorExRendered : EditorRenderer
	{
		public EditorExRendered(Context context) : base(context) { }

		protected override void OnElementChanged(ElementChangedEventArgs<Editor> args)
		{
			base.OnElementChanged(args);

			if (args.NewElement != null) {
				//var element = e.NewElement as EditorEx;

				//this.Control.Background = Resources.GetDrawable(Resource.Drawable.EditorExBorder);
			}
		}
	}
}