using System;
using Dwares.Dwarf;
using Dwares.Druid.UI;
using Xamarin.Forms;
using dotMorten.Xamarin.Forms;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Beylen.Controls
{
	public class AutoSuggestEntryEx : AutoSuggestBox, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(AutoCompleteEntryEx));

		public AutoSuggestEntryEx()
		{
			//Debug.EnableTracing(@class);
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());

			this.TextChanged += OnTextChanged;
			this.QuerySubmitted += (s,e) => {
				if (e.ChosenSuggestion == null) {
					OnQuerySubmitted(e.QueryText);
				} else {
					OnSuggestionChoosen(e.ChosenSuggestion);
				}
			};
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(AutoSuggestEntryEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is AutoSuggestEntryEx entry) {
						entry.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}


		public static readonly BindableProperty MinSuggestionLengthProperty =
			BindableProperty.Create(
				nameof(MinSuggestionLength),
				typeof(int),
				typeof(AutoSuggestEntryEx),
				0);

		public int MinSuggestionLength {
			set { SetValue(MinSuggestionLengthProperty, value); }
			get { return (int)GetValue(MinSuggestionLengthProperty); }
		}

		public static readonly BindableProperty SuggestionsSourceProperty =
			BindableProperty.Create(
				nameof(SuggestionsSource),
				typeof(IList),
				typeof(AutoSuggestEntryEx)
				//,propertyChanged: (bindable, oldValue, newValue) => {
				//	if (bindable is AutoSuggestEntryEx entry) {
				//		//
				//	}
				//}
				);

		public IList SuggestionsSource {
			set { SetValue(SuggestionsSourceProperty, value); }
			get { return (IList)GetValue(SuggestionsSourceProperty); }
		}

		public static readonly BindableProperty ChoosenSuggestionProperty =
			BindableProperty.Create(
				nameof(ChoosenSuggestion),
				typeof(object),
				typeof(AutoSuggestEntryEx)
				//,propertyChanged: (bindable, oldValue, newValue) => {
				//	if (bindable is AutoSuggestEntryEx entry) {
				//		//
				//	}
				//}
				);

		public object ChoosenSuggestion {
			set { SetValue(ChoosenSuggestionProperty, value); }
			get { return GetValue(ChoosenSuggestionProperty); }
		}


		protected virtual IList GetSuggestions(string text)
		{
			var source = SuggestionsSource;
			var suggestions = new List<object>();

			foreach (var item in source) {
				var s = item.ToString();
				if (s.StartsWith(text, StringComparison.InvariantCultureIgnoreCase))
					suggestions.Add(item);
			}

			return suggestions;
		}

		protected virtual Task<IList> GetSuggestionsAsync(string text)
		{
			var suggestions = GetSuggestions(text);
			return Task.FromResult(suggestions);
		}

		protected virtual async void OnTextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
		{
			//AutoSuggestBox box = (AutoSuggestBox)sender;
			Debug.Assert(sender == this);

			// Only get results when it was a user typing, 
			// otherwise assume the value got filled in by TextMemberPath 
			// or the handler for SuggestionChosen.
			if (e.Reason == AutoSuggestionBoxTextChangeReason.UserInput || e.Reason == AutoSuggestionBoxTextChangeReason.ProgrammaticChange) {
				ChoosenSuggestion = null;

				if (string.IsNullOrWhiteSpace(Text) || Text.Length < MinSuggestionLength)
					ItemsSource = null;
				else {
					ItemsSource = await GetSuggestionsAsync(Text);

					if (ItemsSource.Count == 1) {
						var suggestion = ItemsSource[0];
						if (suggestion.ToString().Equals(Text, StringComparison.InvariantCultureIgnoreCase)) {
							IsSuggestionListOpen = false;
							ChoosenSuggestion = suggestion;
							//OnSuggestionChoosen(suggestion);
						}
					}
				}
			}
		}

		protected virtual void OnQuerySubmitted(string queryText)
		{
			//Debug.Print($"AutoSuggestEntryEx.OnQuerySubmitted*(): queryText={queryText}");
		}

		protected virtual void OnSuggestionChoosen(object chosenSuggestion)
		{
			//Debug.Print($"AutoSuggestEntryEx.OnSuggestionChoosen*(): chosenSuggestion={chosenSuggestion}");
			ChoosenSuggestion = chosenSuggestion;
		}
	}
}
