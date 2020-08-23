using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Toolkit;
using Beylen.Models;


namespace Beylen.ViewModels
{
	public class CustomerCardModel : ContactCardModel<Customer>
	{
		//static ClassRef @class = new ClassRef(typeof(CustomerCardModel));

		public CustomerCardModel(Customer source) :
			base(source)
		{
			//Debug.EnableTracing(@class);

			UpdateFromSource();
		}

		public Customer Customer => Source;

		public string Icon {
			get => icon;
			set => SetProperty(ref icon, value);
		}
		string icon;

		public string Address {
			get => address;
			set => SetPropertyEx(ref address, value, nameof(Address), nameof(HasAddress));
		}
		string address;

		public bool HasAddress => !string.IsNullOrEmpty(Address);

		public string ContactInfo {
			get => contactInfo;
			set => SetPropertyEx(ref contactInfo, value, nameof(ContactInfo), nameof(ContactString));
		}
		string contactInfo;

		public PhoneNumber ContactPhone {
			get => contactPhone;
			set => SetPropertyEx(ref contactPhone, value, nameof(ContactPhone), 
				nameof(CallNamber), nameof(HasCallNumber), nameof(ContactString));
		}
		PhoneNumber contactPhone;

		public override PhoneNumber CallNamber => ContactPhone;
		public override string ContactString {
			get {
				if (ContactPhone.IsValid) {
					if (string.IsNullOrEmpty(ContactInfo)) {
						return ContactPhone.ToString();
					} else {
						return $"{ContactPhone} ({ContactInfo})";
					}
				} else {
					return string.Empty;
				}
			}
		}

		protected override void UpdateFromSource()
		{
			Icon = PickIcon();
			Name = Source.DisplayName;
			Phone = Source.Phone;
			Address = Source.Address;

			if (Source.Contact != null) {
				ContactPhone = Source.Contact.Phone;
				ContactInfo = Source.Contact.Info;
			} else {
				ContactInfo = ContactPhone = string.Empty;
			}
		}

		string PickIcon()
		{
			if (Source.HasTag("pizza"))
				return "ic_pizza";
			if (Source.HasTag("cafe"))
				return "ic_cafe";
			if (Source.HasTag("restaurant"))
				return "ic_restaurant";
			if (Source.HasTag("adult_daycare"))
				return "ic_face";
			if (Source.HasTag("daycare"))
				return "ic_child_care";

			return "ic_store";
		}


		public static ObservableCollection<CustomerCardModel> CreateCollection()
		{
			return new ShadowCollection<CustomerCardModel, Customer>(
				AppScope.Instance.Customers,
				(customer) => new CustomerCardModel(customer)
				);
		}
	}
}
