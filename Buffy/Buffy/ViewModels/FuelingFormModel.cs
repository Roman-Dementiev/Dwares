using System;
using System.Threading.Tasks;
using Dwares.Druid;
using Dwares.Druid.ViewModels;
using Dwares.Dwarf;
using Buffy.Models;
using Xamarin.Forms;
using Dwares.Druid.UI;

namespace Buffy.ViewModels
{
	[QueryProperty("QueryId", "id")]
	public class FuelingFormModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(EditFuelingViewModel));

		public FuelingFormModel()
		{
			//Debug.EnableTracing(@class);

			//Title = "Fueling";

			SaveCommand = new Command(async () => await Save());
			EditCommand = new Command(() => IsReadOnly = false);
			DeleteCommand = new Command(async () => await Delete());
			//ChooseVendorCommand = new Command(ChooseVendor);
		}

		public Command SaveCommand { get; }
		public Command EditCommand { get; }
		public Command DeleteCommand { get; }

		//public Command ChooseVendorCommand { get; }

		public string QueryId {
			set {
				if (value == "new") {
					Source = null;
					IsReadOnly = false;
				} else {
					Source = App.GetFueling(value);
					IsReadOnly = true;
				}
				IsModified = false;
			}
		}

		public bool IsReadOnly {
			get => isReadOnly;
			set => SetPropertyEx(ref isReadOnly, value, nameof(IsReadOnly), nameof(IsEditing));
		}
		bool isReadOnly;

		public bool IsEditing {
			get => !IsReadOnly;
			set => IsReadOnly = !value;
		}

		public Fueling Source {
			get => source;
			private set {
				source = value;
				if (source == null) {
					Date = DateTime.Today;
					Vendor = string.Empty;
					State = string.Empty;
					Gallons = string.Empty;
					Price = string.Empty;
					Total = string.Empty;
				} else {
					Date = source.Date;
					Vendor = source.Vendor?.Name ?? string.Empty;
					State = source.State ?? string.Empty;
					Gallons = source.Gallons > 0 ? source.Gallons.ToString("N3") : string.Empty;
					Price = source.Price > 0 ? source.Price.ToString("N3") : string.Empty;
					Total = source.Total > 0 ? source.Total.ToString("N2") : string.Empty;
				}
			}
		}
		Fueling source;

		public DateTime Date {
			get => date;
			set => SetProperty(ref date, value);
		}
		DateTime date;

		public string Vendor {
			get => vendor;
			set => SetProperty(ref vendor, value);
		}
		string vendor;

		public string State {
			get => state;
			set => SetProperty(ref state, value);
		}
		string state;

		public string Gallons {
			get => gallons;
			set => SetProperty(ref gallons, value);
		}
		string gallons;

		public string Price {
			get => price;
			set => SetProperty(ref price, value);
		}
		string price;

		public string Total {
			get => total;
			set => SetProperty(ref total, value);
		}
		string total;

		public async Task<bool> CanGoBack()
		{
			if (IsModified) {
				bool proceed = await Alerts.ConfirmAlert("Do you want to leave without saving?");
				if (!proceed)
					return false;
			}

			return true;
		}

		async Task Save()
		{
			try {
				var fueling = Validate();
				if (Source == null) {
					await App.NewFueling(fueling);
				} else {
					await App.UpdateFueling(Source, fueling);
				}

				IsModified = false;
				await ShellPageEx.TryGoBack();
			}
			catch (Exception exc) {
				if (exc is UserError) {
					await Alerts.DisplayAlert(null, exc.Message);
				} else {
					await Alerts.ErrorAlert(exc.Message);
				}

			}
		}

		public Fueling Validate()
		{
			if (Date > DateTime.Today)
				throw new UserError("Please enter valid date, today or earlier");

			if (string.IsNullOrWhiteSpace(Vendor))
				throw new UserError("Please enter Vendor");

			if (!string.IsNullOrWhiteSpace(State)) {
				if (State.Length != 2 || !char.IsLetter(State[0]) || !char.IsLetter(State[1]))
					throw new UserError("Please enter 2 letter for State or leave empty");
			}

			decimal gallons = 0;
			if (!string.IsNullOrWhiteSpace(Gallons)) {
				if (!decimal.TryParse(Gallons, out gallons) || gallons <= 0)
					throw new UserError("Please enter positive number for Gallons or leave field empty");
			}
			decimal price = 0;
			if (!string.IsNullOrWhiteSpace(Price)) {
				if (!decimal.TryParse(Price, out price) || price <= 0)
					throw new UserError("Please enter positive number for Price or leave field empty");
			}

			//if (string.IsNullOrWhiteSpace(Total))
			//	return "Please enter Total";

			decimal total = 0;
			if (!decimal.TryParse(Total, out total) || total <= 0)
				throw new Exception("Please enter positive number for Total");

			var fueling = new Fueling();
			fueling.Date = Date;
			fueling.Vendor = App.GetVendor(Vendor);
			fueling.State = State;
			fueling.Gallons = gallons;
			fueling.Price = price;
			fueling.Total = total;

			return fueling;
		}

		async Task Delete()
		{
			await App.DeleteFueling(Source);
			await ShellPageEx.TryGoBack();
		}
	}
}
