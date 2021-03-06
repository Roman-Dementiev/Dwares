﻿using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Beylen.Models;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Beylen.ViewModels
{
	public class InvoicesViewModel : CollectionViewModel<InvoiceCardModel>
	{
		//static ClassRef @class = new ClassRef(typeof(InvoicesViewModel));

		public static ObservableCollection<InvoiceCardModel> CreateCollection()
		{
			return new ShadowCollection<InvoiceCardModel, Invoice>(
				AppScope.Instance.Invoices,
				(invoice) => new InvoiceCardModel(invoice)
				);
		}
			
		public InvoicesViewModel() :
			base(CreateCollection())
		{
			//Debug.EnableTracing(@class);

			Title = "Invoices";
			AddCommand = new Command(async () => await Shell.Current.GoToAsync($"invoice?number=new"));
		}

		public Command AddCommand { get; }

	}
}
