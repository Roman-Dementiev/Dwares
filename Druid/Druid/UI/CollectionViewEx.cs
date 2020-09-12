using System;
using System.Linq;
using Dwares.Dwarf;
using Dwares.Druid.Effects;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class CollectionViewEx : CollectionView, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(CollectionViewEx));

		public CollectionViewEx()
		{
			//Debug.EnableTracing(@class);

			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(CollectionViewEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is CollectionViewEx collectionView) {
						collectionView.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}

		//public static readonly BindableProperty IsDraggableProperty =
		//	BindableProperty.Create(
		//		nameof(IsDraggable),
		//		typeof(bool),
		//		typeof(CollectionViewEx),
		//		propertyChanged: (bindable, oldValue, newValue) => {
		//			if (bindable is CollectionViewEx collectionView && newValue is bool draggable) {
		//				collectionView.SetDraggableEffect(draggable);
		//			}
		//		});

		//public bool IsDraggable {
		//	set { SetValue(IsDraggableProperty, value); }
		//	get { return (bool)GetValue(IsDraggableProperty); }
		//}

		//void SetDraggableEffect(bool draggable)
		//{
		//	var effect = Effects.FirstOrDefault(item => item is ListViewDraggableEffect);
		//	if (draggable) {
		//		if (effect == null) {
		//			Effects.Add(new ListViewDraggableEffect());
		//		}
		//	} else {
		//		// No need to remove, dragging will be disabled by Platform's CollectionViewDraggableEffect.OnElementPropertyChanged()
		//		//
		//		//if (effect != null) {
		//		//	Effects.Remove(effect);
		//		//}
		//	}
		//}
	}
}
