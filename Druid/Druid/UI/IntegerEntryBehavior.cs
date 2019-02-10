//using System;
//using Xamarin.Forms;
//using Dwares.Dwarf;

//namespace Dwares.Druid.UI
//{
//	public class IntegerEntryBehavior : EntryBehavior
//	{
//		//static ClassRef @class = new ClassRef(typeof(NumberEntryBehavior));

//		public IntegerEntryBehavior()
//		{
//			//Debug.EnableTracing(@class);
//		}

//		public static readonly BindableProperty ValueProperty =
//			BindableProperty.Create(
//				nameof(Value),
//				typeof(int?),
//				typeof(IntegerEntryBehavior),
//				//propertyChanged: (bindable, oldValue, newValue) => {
//				//	if (bindable is IntegerEntryBehavior behavior && newValue is int minValue) {
//				//		// ???
//				//	}
//				//},
//				defaultValue: null
//				);

//		public int? Value {
//			set { SetValue(ValueProperty, value); }
//			get { return (int?)GetValue(ValueProperty); }
//		}

//		public static readonly BindableProperty MinValueProperty =
//			BindableProperty.Create(
//				nameof(MinValue),
//				typeof(int?),
//				typeof(IntegerEntryBehavior),
//				//propertyChanged: (bindable, oldValue, newValue) => {
//				//	if (bindable is IntegerEntryBehavior behavior && newValue is int minValue) {
//				//		// ???
//				//	}
//				//},
//				defaultValue: null
//				);

//		public int? MinValue {
//			set { SetValue(MinValueProperty, value); }
//			get { return (int?)GetValue(MinValueProperty); }
//		}

//		public static readonly BindableProperty MaxValueProperty =
//			BindableProperty.Create(
//				nameof(MaxValue),
//				typeof(int?),
//				typeof(IntegerEntryBehavior),
//				//propertyChanged: (bindable, oldValue, newValue) => {
//				//	if (bindable is IntegerEntryBehavior behavior && newValue is int maxValue) {
//				//		// ???
//				//	}
//				//},
//				defaultValue: null
//				);

//		public int? MaxValue {
//			set { SetValue(MaxValueProperty, value); }
//			get { return (int?)GetValue(MaxValueProperty); }
//		}

//		protected override void OnTextChanged(Entry entry, string newTextValue, string oldTextValue)
//		{
//			int newValue;
//			if (Validate(newTextValue, out newValue)) {
//				Value = newValue;
//			} else {
//				entry.Text = oldTextValue;
//			}
//		}

//		protected bool Validate(string newTextValue, out int newValue)
//		{
//			if (!int.TryParse(newTextValue, out newValue))
//				return false;


//			if (MinValue != null && newValue < MinValue)
//				return false;
//			if (MaxValue != null && newValue > MaxValue)
//				return false;

//			return true;
//		}

//	}
//}
