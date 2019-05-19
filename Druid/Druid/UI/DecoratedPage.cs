using System;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.UI
{
	public enum DecorationLayout
	{
		None,
		Center,
			FullScreen
	}

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

		public static readonly BindableProperty DecorationLayoutProperty =
			BindableProperty.Create(
				nameof(DecorationLayout),
				typeof(DecorationLayout),
				typeof(DecoratedPage),
				defaultValue: DecorationLayout.None,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is DecoratedPage page && newValue is DecorationLayout value) {
						page.LayoutDecorection();
					}
				});

		public DecorationLayout DecorationLayout {
			set { SetValue(DecorationLayoutProperty, value); }
			get { return (DecorationLayout)GetValue(DecorationLayoutProperty); }
		}

		protected abstract void LayoutDecorection();
	}
}
