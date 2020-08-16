using System;
using Dwares.Dwarf;
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
	}
}
