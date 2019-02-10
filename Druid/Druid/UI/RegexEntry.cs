//using System;
//using System.Text.RegularExpressions;
//using Xamarin.Forms;
//using Dwares.Dwarf;
//using Dwares.Dwarf.Toolkit;


//namespace Dwares.Druid.UI
//{
//	public class RegexEntry : Entry
//	{
//		//static ClassRef @class = new ClassRef(typeof(RegExEntry));

//		public RegexEntry()
//		{
//			//Debug.EnableTracing(@class);

//			this.TextChanged += OnTextChanged;
//		}

//		bool reverting = false;
//		private void OnTextChanged(object sender, TextChangedEventArgs e)
//		{
//			if (reverting || Regex.IsMatch(e.NewTextValue))
//				return;

//			reverting = true;
//			Text = e.OldTextValue;
//			reverting = false;
//		}

//		public static readonly BindableProperty PatternProperty =
//			BindableProperty.Create(
//				nameof(Pattern),
//				typeof(string),
//				typeof(RegexEntry),
//				propertyChanged: (bindable, oldValue, newValue) => {
//					if (bindable is RegexEntry entry && newValue is string pattern) {
//						entry.Regex = new Regex(pattern);
//					}
//				},
//				validateValue: (bindable, value) => {
//					Debug.Print("RegexEntry.RegexProperty.validateValue(): value={0}", value);
//					return RegEx.IsValidPattern(value as string);
//				},
//				defaultValue: null
//				);

//		public string Pattern {
//			set { SetValue(PatternProperty, value); }
//			get { return (string)GetValue(PatternProperty); }
//		}

//		public Regex Regex { get; protected set; }
//	}
//}
