using System;
using Xamarin.Forms;
using Dwares.Druid.Support;


namespace Dwares.Druid.UI
{
	// TODO
	public enum AutoSuggestionIntent
	{
		None,
		//General,
		Seacrh
	};

	public enum AutoSuggestionSelectReason
	{
		Choosen,
		ExactMatch
	};

	public class AutoSuggestionSelectedEventArgs : EventArgs
	{
		public object SelectedItem { get; set; }
		public AutoSuggestionSelectReason Reason { get; set; }
	}

	public delegate void AutoSuggestionSelectedEventHandler(object sender, AutoSuggestionSelectedEventArgs e);


	public class AutoSuggestEntry : View
	{
		public event AutoSuggestionSelectedEventHandler AutoSuggestionSelected;

		public AutoSuggestEntry()
		{
			//Source = new AutoSuggestionStrings() {
			//	Suggestions = new List<string>() {
			//		 "None", "Test 1", "Test 2", "Another Test"
			//	}
			//};
		}

		public static readonly BindableProperty PlaceholderProperty =
			BindableProperty.Create(
				nameof(Placeholder),
				typeof(string),
				typeof(AutoSuggestEntry),
				null);

		public string Placeholder {
			set { SetValue(PlaceholderProperty, value); }
			get { return (string)GetValue(PlaceholderProperty); }
		}

		public static readonly BindableProperty TextProperty =
			BindableProperty.Create(
				nameof(Text),
				typeof(string),
				typeof(AutoSuggestEntry),
				"",
				defaultBindingMode: BindingMode.TwoWay);

		public string Text {
			set { SetValue(TextProperty, value); }
			get { return (string)GetValue(TextProperty); }
		}

		public static readonly BindableProperty SuggestionSourceProperty =
			BindableProperty.Create(
				nameof(SuggestionSource),
				typeof(IAutoSuggestions),
				typeof(AutoSuggestEntry),
				null);

		public IAutoSuggestions SuggestionSource {
			set { SetValue(SuggestionSourceProperty, value); }
			get { return (IAutoSuggestions)GetValue(SuggestionSourceProperty); }
		}

		public static readonly BindableProperty MinSuggestionLengthProperty =
			BindableProperty.Create(
				nameof(MinSuggestionLength),
				typeof(int),
				typeof(AutoSuggestEntry),
				0);

		public int MinSuggestionLength {
			set { SetValue(MinSuggestionLengthProperty, value); }
			get { return (int)GetValue(MinSuggestionLengthProperty); }
		}

		public static readonly BindableProperty IntentProperty =
			BindableProperty.Create(
				nameof(Intent),
				typeof(AutoSuggestionIntent),
				typeof(AutoSuggestEntry),
				AutoSuggestionIntent.None);

		public AutoSuggestionIntent Intent {
			set { SetValue(IntentProperty, value); }
			get { return (AutoSuggestionIntent)GetValue(IntentProperty); }
		}

		public static readonly BindableProperty MatchModeProperty =
			BindableProperty.Create(
				nameof(MatchMode),
				typeof(AutoSuggestionsMatchMode),
				typeof(AutoSuggestEntry),
				AutoSuggestionsMatchMode.Default);

		public AutoSuggestionsMatchMode MatchMode {
			set { SetValue(MatchModeProperty, value); }
			get { return (AutoSuggestionsMatchMode)GetValue(MatchModeProperty); }
		}

		public static readonly BindableProperty WordSeparatorsProperty =
			BindableProperty.Create(
				nameof(WordSeparators),
				typeof(string),
				typeof(AutoSuggestEntry),
				" ");

		public string WordSeparators {
			set { SetValue(WordSeparatorsProperty, value); }
			get { return (string)GetValue(WordSeparatorsProperty); }
		}


		public static readonly BindableProperty SelectExactMatchProperty =
			BindableProperty.Create(
				nameof(SelectExactMatch),
				typeof(bool),
				typeof(AutoSuggestEntry),
				true);

		public bool SelectExactMatch {
			set { SetValue(SelectExactMatchProperty, value); }
			get { return (bool)GetValue(SelectExactMatchProperty); }
		}

		public void OnItemSelected(object item, AutoSuggestionSelectReason reason)
		{
			if (AutoSuggestionSelected != null) {
				var args = new AutoSuggestionSelectedEventArgs() { SelectedItem = item, Reason = reason };
				AutoSuggestionSelected.Invoke(this, args);
			}
		}
	}
}
