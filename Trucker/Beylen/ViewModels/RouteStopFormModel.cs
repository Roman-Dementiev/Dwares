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
	[QueryProperty("QueryStopOrder", "order")]
	public class RouteStopFormModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(RouteStopFormModel));

		public RouteStopFormModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Route Stop";

			CustomerSuggestions = new List<object>();
			BuildSuggestion();

			DoneCommand = new Command(Done);
			MoreCommand = new Command(More);
			CancelCommand = new Command(async () => await Shell.Current.Navigation.PopAsync());

			CanCreateOrder = true;
		}

		void BuildSuggestion()
		{
			CustomerSuggestions.Clear();
			Suggestions.CollectCustomers(CustomerSuggestions, (customer) => !AppScope.Instance.Route.HasCustomerStop(customer));
		}

		public string QueryStopOrder {
			set {
				try {
					int order = int.Parse(value);
					var stop = AppScope.Instance.Route.Stops[order];
					CodeName = stop.CodeName;
					RealName = stop.RealName;
					Address = stop.Address;
				}
				catch {
					CodeName = RealName = Address = string.Empty;
				}

				CanCreateOrder = false;
			}
		}


		public Customer Customer {
			get => customer;
			set => SetProperty(ref customer, value);
		}
		Customer customer;

		public string CodeName {
			get => codeName;
			set {
				if (SetProperty(ref codeName, value)) {
					SetCustomer(codeName, null);
					choosenSuggestion = null;
				}
			}
		}
		string codeName;

		public string RealName {
			get => realName;
			set => SetProperty(ref realName, value);
		}
		string realName;

		public string Address {
			get => address;
			set => SetProperty(ref address, value);
		}
		string address;

		public bool CreateOrder {
			get => createOrder;
			set => SetProperty(ref createOrder, value);
		}
		bool createOrder;

		public bool CanCreateOrder {
			get => canCreateOrder;
			set => SetProperty(ref canCreateOrder, value);
		}
		bool canCreateOrder;

		void SetCustomer(string codeName, Customer customer)
		{
			Customer = customer ?? AppScope.GetCustomer(codeName);

			if (Customer != null) {
				if (codeName == null)
					CodeName = Customer.CodeName;
				RealName = Customer.RealName;
				Address = Customer.Address;
				CreateOrder = CanCreateOrder = true;
			} else {
				Clear(codeName == null);
			}
		}

		void Clear(bool clearCodeName)
		{
			if (clearCodeName)
				CodeName = string.Empty;
			RealName = Address = string.Empty;
			CreateOrder = CanCreateOrder = false;
			Customer = null;
			choosenSuggestion = null;
		}

		public List<object> CustomerSuggestions { get; }

		public object ChoosenSuggestion {
			get => choosenSuggestion;
			set {
				//Debug.Print($"RouteStopFormModel: ChoosenSuggestion <= {value}");
				if (value is CustomerSuggestion suggestion) {
					SetCustomer(null, suggestion.Customer);
					choosenSuggestion = value;
				} else {
					Clear(false);
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
				Clear(true);
			}
		}

		public async void More()
		{
			if (!string.IsNullOrWhiteSpace(CodeName) && await AddStop()) {
				BuildSuggestion();
			}
			Clear(true);
		}

		async Task<bool> AddStop()
		{
			if (Customer == null) {
				await Alerts.ErrorAlert($"Unknown stop: \"{CodeName}\"");
				return false;
			}
			if (AppScope.Instance.Route.HasCustomerStop(Customer)) {
				await Alerts.ErrorAlert($"\"{CodeName}\" already in the route");
				return false;
			}

			try {
				var stop = new CustomerStop(customer);
				await AppScope.Instance.Route.AddNew(stop);
				return true;
			} 
			catch (Exception exc) {
				await Alerts.ExceptionAlert(exc);
				return false;
			}
		}
	}

}
