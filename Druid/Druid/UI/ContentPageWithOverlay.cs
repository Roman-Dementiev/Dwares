using System;
using Dwares.Druid.Satchel;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	[ContentProperty("ContentView")]
	public class ContentPageWithOverlay : ContentPage
	{
		//static ClassRef @class = new ClassRef(typeof(ContentPageWithOverlay));

		public ContentPageWithOverlay()
		{
			//Debug.EnableTracing(@class);

			Root = new AbsoluteLayout();

			base.Content = Root;
		}

		protected AbsoluteLayout Root { get; }

		public new View Content {
			get => ContentView;
			set => ContentView = value;
		}

		public View ContentView {
			get => contentView;
			set {
				if (value != contentView) {
					ResetChildren(value, OverlayView);
					contentView = value;
					OnPropertyChanged();
				}
			}
		}
		View contentView;

		public View OverlayView {
			get => overlayView;
			set {
				if (value != overlayView) {
					ResetChildren(ContentView, value, overlayOnly: true);
					overlayView = value;
					OnPropertyChanged();
				}
			}
		}
		View overlayView;

		protected AbsoluteLayoutFlags ContentLayoutFlags {
			get => contentLayoutFlags;
			set {
				if (value != contentLayoutFlags) {
					contentLayoutFlags = value;
					ResetChildren(ContentView, OverlayView);
					OnPropertyChanged();
				}
			}
		}
		AbsoluteLayoutFlags contentLayoutFlags = AbsoluteLayoutFlags.All;


		void ResetChildren(View newContentView, View newOverlayView, bool overlayOnly = false)
		{
			RemoveChild(overlayView);
			if (!overlayOnly) {
				RemoveChild(contentView);
				AddChild(newContentView);
			}
			AddChild(newOverlayView);
		}

		void AddChild(View childView)
		{
			if (childView != null) {
				Root.Children.Add(childView);
				AbsoluteLayout.SetLayoutBounds(childView, new Rectangle(0, 0, 1, 1));
				AbsoluteLayout.SetLayoutFlags(childView, ContentLayoutFlags);
			}
		}

		void RemoveChild(View childView)
		{
			if (childView != null) {
				Root.Children.Remove(childView);
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (BindingContext is IActivatable activatable) {
				activatable.Activate();
			}
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			if (BindingContext is IActivatable activatable) {
				activatable.Deactivate();
			}
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(ContentPageEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ContentPageEx page) {
						page.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}

	}
}
