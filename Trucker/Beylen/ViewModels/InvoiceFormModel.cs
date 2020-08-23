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

namespace Beylen.ViewModels
{
	[QueryProperty("QueryNumber", "number")]
	public class InvoiceFormModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(InvoiceFormModel));

		public InvoiceFormModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Invoice";

			//GoBackCommand = new Command(async () => {
			//	bool done = await GoBack();
			//	if (done) {
			//		IsBackwardNavAllowed = true;
			//	}
			//});
			//GoBackCommand = new Command(async () => await ExecuteGoBack());
			SaveCommand = new Command(Save);
			AddArticleCommand = new Command(AddArticle);

			CustomerSuggestions = Suggestions.Customers();
			ProduceSuggestions = Suggestions.Produce();
		}

		//public Command GoBackCommand { get; }

		public Command SaveCommand { get; }
		public Command AddArticleCommand { get; }

		public List<object> CustomerSuggestions { get; }
		public List<object> ProduceSuggestions { get; }

		public Invoice Source {
			get => source;
			set {
				if (SetProperty(ref source, value)) {
					if (Source != null) {
						Date = Source.Date;
						Number = Source.Number;
						Notes = Source.Notes;
					} else {
						Date = new DateOnly();
						Number = Notes = null;
					}
					IsModified = false;
				}
			}
		}
		Invoice source;

		public DateTime Date {
			get => date;
			set => SetProperty(ref date, value);
		}
		DateTime date;

		public string Number {
			get => number;
			set => SetProperty(ref number, value);
		}
		string number;

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

		public string QueryNumber {
			set {
				if (value == "new") {
					Source = null;
					Date = AppScope.Instance.OrderingDate;
					Number = string.Format("{0,2:D2}{1,2:D2}{2,2:D2}", Date.Month, Date.Day, AppScope.Instance.OrderingLast + 1);
					Notes = string.Empty;
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
					//Debug.Print($"InvoiceFormModel: ChoosenCustomer <= {value}");
					//if (value is CustomerSuggestion suggestion) {
					//	//ChoosenCustomer = suggestion.Customer;
					//} else {
					//	//ChoosenCustomer = null;
					//}
					IsModified = true;
				}
			}
		}
		object choosenCustomerSuggestion;


		//public Customer ChoosenCustomer { get; private set;}


		//public async Task<bool> GoBack()
		//{
		//	Debug.Print("InvoiceFormModel.GoBack()");

		//	if (IsModified) {
		//		bool proceed = await Alerts.ConfirmAlert("Invoice is not saved.\nDo you want to leave without saving?");
		//		if (!proceed)
		//			return false;
		//	}

		//	try {
		//	#if ISSUE_FIXED__GOTO_ASYNC_BACK
		//		await Shell.Current.GoToAsync("..");
		//	#else
		//		await Shell.Current.Navigation.PopAsync();
		//	#endif
		//		return true;
		//	}
		//	catch (Exception exc) {
		//		Debug.ExceptionCaught(exc);
		//		return false;
		//	}
		//}

		public async Task<bool> CanGoBack()
		{
			Debug.Print("InvoiceFormModel.CanGoBack()");

			if (IsModified) {
				bool proceed = await Alerts.ConfirmAlert("Invoice is not saved.\nDo you want to leave without saving?");
				if (!proceed)
					return false;
			}

			return true;
		}

		void AddArticle()
		{

		}

		void Save()
		{

		}

	}
}
