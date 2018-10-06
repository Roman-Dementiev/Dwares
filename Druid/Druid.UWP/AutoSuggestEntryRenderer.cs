using System;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Controls;
using System.ComponentModel;
using Dwares.Druid.UI;


[assembly: ExportRenderer(typeof(Dwares.Druid.UI.AutoSuggestEntry), typeof(Dwares.Druid.UWP.AutoSuggestEntryRenderer))]

namespace Dwares.Druid.UWP
{
	class AutoSuggestEntryRenderer : ViewRenderer<AutoSuggestEntry, AutoSuggestBox>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<AutoSuggestEntry> args)
		{
			base.OnElementChanged(args);

			if (Control == null) {
				if (Element != null) {
					SetNativeControl(new AutoSuggestBox());
				} else
					return;
			}

			if (args.NewElement != null) {
				SetItemsSource();
				SetIntent();
				SetPlaceholder();
				SetText();

				//Control.QuerySubmitted += Control_QuerySubmitted;
				Control.TextChanged += Control_TextChanged;
				Control.SuggestionChosen += Control_SuggestionChosen;
			} else {
				//Control.QuerySubmitted -= Control_QuerySubmitted;
				Control.TextChanged -= Control_TextChanged;
				Control.SuggestionChosen -= Control_SuggestionChosen;
			}
		}

		//private void Control_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
		//{
		//	if (args.ChosenSuggestion != null) {
		//		//Debug.Print("QuerySubmitted: ChosenSuggestion={0}", args.ChosenSuggestion);
		//		// User selected an item from the suggestion list, take an action on it here.
		//	} else {
		//		//Debug.Print("QuerySubmitted: QueryText={0}", args.QueryText);
		//		// Use args.QueryText to determine what to do.
		//	}
		//}

		private void Control_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
		{
			Element.OnItemSelected(args.SelectedItem, AutoSuggestionSelectReason.Choosen);
		}

		private void Control_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
		{
			if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
				Element.Text = Control.Text;
			}

			if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput ||
				args.Reason == AutoSuggestionBoxTextChangeReason.ProgrammaticChange) {
				SetItemsSource();
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			//Debug.Print("AutoSuggestEntryRenderer.OnElementPropertyChanged() PropertyName={0}", args.PropertyName);

			base.OnElementPropertyChanged(sender, args);

			if (args.PropertyName == AutoSuggestEntry.SuggestionSourceProperty.PropertyName ||
				args.PropertyName == AutoSuggestEntry.MinSuggestionLengthProperty.PropertyName) {
				SetItemsSource();
			} else if (args.PropertyName == AutoSuggestEntry.PlaceholderProperty.PropertyName) {
				SetPlaceholder();
			} else if (args.PropertyName == AutoSuggestEntry.TextProperty.PropertyName) {
				SetText();
			} else if (args.PropertyName == AutoSuggestEntry.IntentProperty.PropertyName) {
				SetIntent();
			}
		}

		void SetItemsSource()
		{
			var input = Control.Text ?? String.Empty;
			var source = Element.SuggestionSource;
			if (source == null || input.Length < Element.MinSuggestionLength) {
				Control.ItemsSource = null;
				return;
			}

			var separators = Element.WordSeparators;
			if (String.IsNullOrEmpty(separators)) {
				separators = " ";
			}
			var suggestions = source.GetSuggestions(input, Element.MatchMode, separators);
			Control.ItemsSource = suggestions;
			Control.DisplayMemberPath = Control.TextMemberPath = Element.SuggestionSource.DisplayProperty ?? "";

			if (Element.SelectExactMatch && suggestions.Count == 1 && input == source.DisplayText(suggestions[0])) {
				Element.OnItemSelected(suggestions[0], AutoSuggestionSelectReason.ExactMatch);
			}
		}

		void SetPlaceholder()
		{
			//Debug.Print("SetPlaceholder: Element.SetPlaceholder={0}", Element.Placeholder);
			Control.PlaceholderText = Element.Placeholder ?? "";
		}

		void SetText()
		{
			//Debug.Print("SetText: Element.Text={0}", Element.Text);
			Control.Text = Element.Text ?? "";
		}

		void SetIntent()
		{
			switch (Element.Intent) {
			case AutoSuggestionIntent.Seacrh:
				Control.QueryIcon = new SymbolIcon(Symbol.Find);
				break;

			//case AutoSuggestionIntent.General:
			//	Control.QueryIcon = new /*SymbolIcon(Symbol.Download*/SymbolIcon(Symbol.More);
			//	break;

			default:
				Control.QueryIcon = null;
				break;
			}
		}
	}
}
