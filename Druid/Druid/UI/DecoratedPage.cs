using System;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.UI
{
	[ContentProperty("ContentView")]
	public abstract class DecoratedPage : ContentPageEx
	{
		//static ClassRef @class = new ClassRef(typeof(DecoratedPage));

		public DecoratedPage()
		{
			//Debug.EnableTracing(@class);
		}

		public abstract IContentHolder Decoration { get; set; }

		protected override View GetContentView() => Decoration?.ContentView;

		protected override void ChangeContentView(View newContentView)
		{
			if (Decoration != null) {
				Decoration.ContentView = newContentView;
			}
		}

		//public override View ContentView {
		//	get => Decoration?.ContentView;

		//	set {
		//		if (Decoration == null)
		//			return;

		//		if (value != Decoration.ContentView) {
		//			OnPropertyChanging();
		//			Decoration.ContentView = value;
		//			OnPropertyChanged();
		//		}
		//	}
		//}
	}
}
