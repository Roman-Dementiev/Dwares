using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Dwarf.Toolkit;
using Beylen.Models;
using System.Collections.ObjectModel;
using Dwares.Dwarf.Collections;
using Xamarin.Forms;

namespace Beylen.ViewModels
{
	public class ContactCardModel<TContact> : CardViewModel<TContact> where TContact : Contact
	{
		//static ClassRef @class = new ClassRef(typeof(ContactCardModel));

		public ContactCardModel(TContact source) :
			base(source)
		{
			//Debug.EnableTracing(@class);

			CallCommand = new Command(Call, CanCall);

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

		public virtual PhoneNumber CallNamber => Phone;
		public bool HasCallNumber => CallNamber.IsValid;
		public virtual string ContactString => Phone.ToString();

		protected override void UpdateFromSource()
		{
			Name = Source.Name;
			Phone = Source.Phone;
		}

		public Command CallCommand { get; set; }
		public void Call()
		{
			Debug.Print($"ContactCardModel.Call(): Phone={CallNamber}");
		}

		public bool CanCall() => HasCallNumber;

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
