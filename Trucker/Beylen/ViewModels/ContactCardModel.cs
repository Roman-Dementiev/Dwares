using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Collections;
using Dwares.Druid.Services;
using Xamarin.Forms;
using Beylen.Models;


namespace Beylen.ViewModels
{
	public class ContactCardModel<TContact> : CardViewModel<TContact> where TContact : IContact
	{
		//static ClassRef @class = new ClassRef(typeof(ContactCardModel));

		public ContactCardModel(TContact source) :
			base(source)
		{
			//Debug.EnableTracing(@class);

			UpdateFromSource();
		}

		public string Name {
			get => name;
			set => SetProperty(ref name, value);
		}
		string name;

		public PhoneNumber Phone {
			get => phone;
			set => SetPropertyEx(ref phone, value, nameof(Phone), nameof(CallNamber), nameof(HasCallNumber), nameof(ContactString));
		}
		PhoneNumber phone;

		public string Info {
			get => info;
			set => SetPropertyEx(ref info, value, nameof(Info), nameof(HasInfo));
		}
		string info;

		public bool HasInfo => !string.IsNullOrEmpty(info);

		public virtual PhoneNumber CallNamber => Phone;
		public bool HasCallNumber => CallNamber.IsValid;
		public virtual string ContactString => Phone.ToString();

		protected override void UpdateFromSource()
		{
			Name = Source.Name;
			Phone = Source.Phone;
			Info = Source.Info;
		}

		static Command callCommand;
		public Command CallCommand { 
			get => callCommand ??= new Command(Call, CanCall);
		}

		public static void Call(object number)
		{
			var phoneNumber = PhoneNumber.Parse(number);
			Debug.Print($"ContactCardModel.Call(): Phone={phoneNumber}");

			PhoneDialer.TryDial(phoneNumber);
		}

		public static bool CanCall(object number)
		{
			var phoneNumber = PhoneNumber.Parse(number);
			return phoneNumber.IsValid;
		}

	}

	public class ContactCardModel : ContactCardModel<Contact>
	{
		public ContactCardModel(Contact source) : base(source) { }

		public Contact Contact => Source;

		public static ObservableCollection<ContactCardModel> CreateCollection()
		{
			return new ShadowCollection<ContactCardModel, Contact>(
				AppScope.Instance.Contacts,
				(contact) => new ContactCardModel(contact)
				);
		}
	}
}
