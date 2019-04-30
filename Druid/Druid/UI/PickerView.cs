using System;
using Xamarin.Forms;
using Dwares.Dwarf;
using System.Collections;


namespace Dwares.Druid.UI
{
	public class PickerView : StackLayout
	{
		//static ClassRef @class = new ClassRef(typeof(PickerView));

		Label label;
		Picker picker;

		public PickerView()
		{
			//Debug.EnableTracing(@class);

			//Orientation = StackOrientation.Vertical;

			label = new LabelEx();
			Children.Add(label);

			picker = new Picker();
			Children.Add(picker);
		}

		public static readonly BindableProperty TitleProperty =
			BindableProperty.Create(
				nameof(Title),
				typeof(string),
				typeof(PickerView),
				defaultValue: string.Empty,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PickerView view && newValue is string title) {
						view.SetTitle(title);
					}
				});

		public string Title {
			set { SetValue(TitleProperty, value); }
			get { return (string)GetValue(TitleProperty); }
		}

		void SetTitle(string title)
		{
			label.Text = title;
			if (Device.RuntimePlatform == Device.Android) {
				picker.Title = title;
			}
		}

		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(
				nameof(ItemsSource),
				typeof(IList),
				typeof(PickerView),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PickerView view && newValue is IList itemSource) {
						view.picker.ItemsSource = itemSource;
					}
				});

		public IList ItemsSource {
			set { SetValue(ItemsSourceProperty, value); }
			get { return (IList)GetValue(ItemsSourceProperty); }
		}

		public static readonly BindableProperty SelectedIndexProperty =
			BindableProperty.Create(
				nameof(SelectedIndex),
				typeof(int),
				typeof(PickerView),
				defaultValue: -1,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PickerView view && newValue is int index) {
						view.picker.SelectedIndex = index;
					}
				});

		public int SelectedIndex {
			set { SetValue(SelectedIndexProperty, value); }
			get { return (int)GetValue(SelectedIndexProperty); }
		}

		public static readonly BindableProperty SelectedItemProperty =
			BindableProperty.Create(
				nameof(SelectedItem),
				typeof(object),
				typeof(PickerView),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is PickerView view) {
						view.picker.SelectedItem = newValue;
					}
				});

		public object SelectedItem {
			set { SetValue(SelectedItemProperty, value); }
			get { return GetValue(SelectedItemProperty); }
		}
	}

}
