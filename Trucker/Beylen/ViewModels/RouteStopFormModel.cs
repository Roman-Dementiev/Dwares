using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;
using Beylen.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Beylen.ViewModels
{
	[QueryProperty("StopOrder", "order")]
	public class RouteStopFormModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(RouteStopFormModel));

		public RouteStopFormModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Route Stop";

			Suggestions = new List<object>();
			BuildSuggestion();

			DoneCommand = new Command(Done);
			MoreCommand = new Command(More);
			CancelCommand = new Command(async () => await Shell.Current.Navigation.PopAsync());
		}

		void BuildSuggestion()
		{
			Suggestions.Clear();
			var customers = AppScope.Instance.Customers;
			var route = AppScope.Instance.Route;
			foreach (var customer in customers) {
				if (!route.HasCustomerStop(customer)) {
					Suggestions.Add(new CustomerSuggestion(customer));
				}
			}
		}

		public string StopOrder {
			set {
				try {
					int order = int.Parse(value);
					var stop = AppScope.Instance.Route[order];
					CodeName = stop.CodeName;
					FullName = stop.FullName;
					Address = stop.Address;
					return;
				}
				catch {
					CodeName = FullName = Address = string.Empty;
				}
			}
		}

		public string CodeName {
			get => codeName;
			set => SetProperty(ref codeName, value);
		}
		string codeName;

		public string FullName {
			get => fullName;
			set => SetProperty(ref fullName, value);
		}
		string fullName;

		public string Address {
			get => address;
			set => SetProperty(ref address, value);
		}
		string address;

		public List<object> Suggestions { get; }

		public object ChoosenSuggestion {
			get => choosenSuggestion;
			set {
				Debug.Print($"RouteStopFormModel: ChoosenSuggestion <= {value}");
				choosenSuggestion = value;
				if (value is CustomerSuggestion suggestion) {
					FullName = suggestion.Customer.FullName;
					Address = suggestion.Customer.Address;
				} else {
					FullName = Address = String.Empty;
				}
			}
		}
		object choosenSuggestion;

		public Command MoreCommand { get; }
		public Command DoneCommand { get; }
		public Command CancelCommand { get; }

		public async void Done()
		{
			if (string.IsNullOrWhiteSpace(CodeName) || await AddStop()) {
				await Shell.Current.Navigation.PopAsync();
			} else {
				CodeName = FullName = Address = string.Empty;
			}
		}

		public async void More()
		{
			if (!string.IsNullOrWhiteSpace(CodeName) && await AddStop()) {
				BuildSuggestion();
			}
			CodeName = FullName = Address = string.Empty;
		}

		async Task<bool> AddStop()
		{
			RouteStop stop;
			if (choosenSuggestion is CustomerSuggestion cs) {
				stop = new CustomerStop(cs.Customer);
			} else {
				var codeName = CodeName;
				var customer = AppScope.Instance.Customers.GetByName(codeName);
				if (customer == null) {
					await Alerts.ErrorAlert($"Unknown stop: \"{codeName}\"");
					return false;
				}
				if (AppScope.Instance.Route.HasCustomerStop(customer)) {
					await Alerts.ErrorAlert($"\"{codeName}\" already in the route");
					return false;
				}

				stop = new CustomerStop(customer);
			}

			var exc = await AppScope.Instance.Route.Add(stop);
			if (exc != null) {
				await Alerts.ErrorAlert(exc.Message);
				return false;
			}

			return true;
		}
	}

	internal struct CustomerSuggestion
	{
		public CustomerSuggestion(Customer customer)
		{
			Customer = customer;
		}

		public Customer Customer { get; }
		public override string ToString() => Customer.Name;
	}
}
