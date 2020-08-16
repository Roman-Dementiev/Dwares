using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Beylen.ViewModels;
using dotMorten.Xamarin.Forms;


namespace Beylen.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RouteStopForm : ContentPageEx
	{
		public RouteStopForm()
		{
			InitializeComponent();
		}

        private void SuggestBox_TextChanged(object sender, AutoSuggestBoxTextChangedEventArgs e)
        {
            AutoSuggestBox box = (AutoSuggestBox)sender;
            // Filter the list based on text input
            box.ItemsSource = GetSuggestions(box.Text);
        }

        private void SuggestBox_QuerySubmitted(object sender, AutoSuggestBoxQuerySubmittedEventArgs e)
        {
			if (e.ChosenSuggestion == null)
				Debug.Print("Query submitted: " + e.QueryText);
			else
                Debug.Print("Suggestion chosen: " + e.ChosenSuggestion);
		}

        List<string> suggestions = new List<string> {
            "A suggestion",
            "AAA suggestion",
            "aaaa suggestion",
            "aa suggestion",
            "BB",
            "bbbbb"
		};

        private List<string> GetSuggestions(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? null : suggestions.Where(s => s.StartsWith(text, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }
    }
}