using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AssetWerks
{
	public partial class ChoiceAutoSuggestBox : UserControl
	{
		public ChoiceAutoSuggestBox()
		{
			this.InitializeComponent();

			control.TextChanged += OnTextChanged;
			control.SuggestionChosen += OnSuggestionChosen;
		}

		public AutoSuggestBox Control => control;
		public string Input { get; private set; }
		public object Selected { get; private set; }

		IList choices;
		public IList Choices {
			get => choices;
			set {
				if (value != choices) {
					choices = value;
					Suggestions.Clear();
				}
			}
		}

		//public ObservableCollection<string> Suggestions { get; } = new ObservableCollection<string>();
		public ObservableCollection<object> Suggestions { get; } = new ObservableCollection<object>();

		void AddSuggestions(string input = null)
		{
			if (Choices == null)
				return;

			input = input?.ToLower();

			foreach (var choice in Choices) {
				if (input == null || choice.ToString().ToLower().Contains(input)) {
					Suggestions.Add(choice);
				}
			}
		}

		void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
		{
			Input = sender.Text;
			Selected = null;

			UpdateSuggestions(args.Reason == AutoSuggestionBoxTextChangeReason.UserInput ? Input : null);
		}

		protected void UpdateSuggestions(string input)
		{
			if (input != null) {
				Suggestions.Clear();
				if (input == "?") {
					AddSuggestions();
				} else {
					AddSuggestions(input);
				}
			}
			control.ItemsSource = Suggestions;
		}


	void OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
		{
			Selected = args.SelectedItem;
		}
	}
}
