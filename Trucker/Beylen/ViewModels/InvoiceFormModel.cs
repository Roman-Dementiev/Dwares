using System;
using System.Collections.Generic;
using Dwares.Dwarf;
using Dwares.Druid;
using Xamarin.Forms;
using Dwares.Dwarf.Collections;
using Beylen.Models;
using Dwares.Dwarf.Toolkit;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Dwares.Druid.Painting;
using Dwares.Druid.UI;

namespace Beylen.ViewModels
{
	[QueryProperty("QueryNumber", "number")]
	public class InvoiceFormModel : CollectionViewModel<ArticleCardModel>
	{
		//static ClassRef @class = new ClassRef(typeof(InvoiceFormModel));

		public InvoiceFormModel() :
			base(AppScope.Instance, new ShadowCollection<ArticleCardModel, Article>())
		{
			//Debug.EnableTracing(@class);

			Title = "Invoice";
			Cards = Items as ShadowCollection<ArticleCardModel, Article>;
			Debug.AssertNotNull(Cards);

			SaveCommand = new Command(async () => await Save());

			CustomerSuggestions = Suggestions.Customers;
		}

		//public Command GoBackCommand { get; }

		public Command SaveCommand { get; }
		public List<object> CustomerSuggestions { get; }

		public Invoice Source {
			get => source;
			set {
				var appScope = AppScope.Instance;
				var ivoice = value ?? new Invoice(true);

				if (SetProperty(ref source, ivoice)) {
					Date = Source.Date;
					//Number = Source.Number;
					Notes = Source.Notes;

					Cards.Clear();
					Cards.SetSource(ivoice.Articles, (a) => new ArticleCardModel(a));
					Cards.Add(Placeholder);
					IsModified = false;
					IsNewInvoice = value == null;
				}
			}
		}
		Invoice source;

		public bool IsNewInvoice { get; set; }
		public ArticleCardModel Placeholder { get; } = new ArticleCardModel(null);

		public DateTime Date {
			get => date;
			set => SetProperty(ref date, value);
		}
		DateTime date;

		//public string Number {
		//	get => number;
		//	set => SetProperty(ref number, value);
		//}
		//string number;

		public string Notes {
			get => notes;
			set => SetProperty(ref notes, value);
		}
		string notes;

		public string CustomerName {
			get => customerName;
			set => SetProperty(ref customerName, value);
		}
		string customerName;

		public ShadowCollection<ArticleCardModel, Article> Cards { get; }

		public string QueryNumber {
			set {
				if (value == "new") {
					Source = null;
				} else {
					Source = AppScope.Instance.Invoices.Lookup((inv) => inv.Number == value);
				}
				IsModified = false;
			}
		}

		public object ChoosenCustomerSuggestion {
			get => choosenCustomerSuggestion;
			set {
				if (SetProperty(ref choosenCustomerSuggestion, value)) {
					IsModified = true;
				}
			}
		}
		object choosenCustomerSuggestion;


		bool CheckModified()
		{
			if (IsModified)
				return true;

			foreach (var card in Cards) {
				if (card.Source != null && card.IsModified)
					return true;
			}

			return false;
		}

		public async Task<bool> CanGoBack()
		{
			Debug.Print("InvoiceFormModel.CanGoBack()");

			IsModified = CheckModified();
			if (IsModified) {
				bool proceed = await Alerts.ConfirmAlert("Invoice is not saved.\nDo you want to leave without saving?");
				if (!proceed)
					return false;
			}

			return true;
		}

		void AddArticle()
		{
			var article = new Article();
			var source = Cards.Source;
			Cards.Remove(Placeholder);
			Cards.Source.Add(article);
			Cards.Add(Placeholder);
			IsModified = true;

			SelectedItem = Cards[Cards.Count-2];
		}

		async Task Save()
		{
			try {
				string message = Validate();
				if (!string.IsNullOrEmpty(message)) {
					await Alerts.DisplayAlert(null, message);
					return;
				}

				UpdateSource();

				if (IsNewInvoice) {
					await AppScope.Instance.NewInvoice(Source);
				} else {
					await AppScope.Instance.UpdateInvoice(Source);
				}

				await ShellPageEx.TryGoBack();
			}
			catch (Exception exc) {
				await Alerts.ErrorAlert(exc.Message);
			}
		}

		public string Validate()
		{
			//Invoice invoice = Source;

			if (string.IsNullOrEmpty(CustomerName))
				return "Please enter Customer name";

			var customer = AppScope.GetCustomer(CustomerName);
			if (customer == null)
				return $"Unknown Customer: {Dw.ToString(CustomerName)}";


			if (Date < DateTime.Today)
				return $"Please enter correct date (Today or next day)";

			for (int i = 0; i < Cards.Count-1; i++) {
				var cardModel = Cards[i];
				Guard.ArgumentNotNull(cardModel, nameof(cardModel));

				if (string.IsNullOrEmpty(cardModel.ProduceName))
					return $"Please enter Produce name for position {i+1}";
				
				var produce = AppScope.GetProduce(cardModel.ProduceName);
				if (produce == null)
					return $"Unknown Produce: {Dw.ToString(cardModel.ProduceName)}";

				if (cardModel.Quantity <= 0)
					return $"Please enter positive quantity for position {i+1}";
			}

			return null;
		}


		public static bool UpdatingSource { get; private set; }

		void UpdateSource()
		{
			try {
				UpdatingSource = true;

				var invoice = Source;
				Guard.ArgumentNotNull(invoice, nameof(invoice));

				invoice.Customer = AppScope.GetCustomer(CustomerName);
				invoice.Date = Date;
				invoice.Notes = Notes;

				for (int i = 0; i < Cards.Count - 1; i++) {
					var cardModel = Cards[i];
					Guard.ArgumentNotNull(cardModel, nameof(cardModel));

					var article = cardModel.Source;
					Guard.ArgumentNotNull(article, nameof(article));

					article.Produce = AppScope.GetProduce(cardModel.ProduceName);
					article.Quantity = cardModel.Quantity;
					article.Unit = cardModel.IsCounts ? "ct" : cardModel.PackingName;
					article.Note = cardModel.Note;

					cardModel.IsModified = false;
				}

				IsModified = false;
			}
			finally {
				UpdatingSource = false;
			}
		}

		public static InvoiceFormModel Current { get; set; }
		public static Command AddArticleCommand { get; } = new Command(() => Current?.AddArticle());

	}
}
