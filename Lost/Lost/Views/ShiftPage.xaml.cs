﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Lost.ViewModels;


namespace Lost.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShiftPage : ContentPageEx
	{
		public ShiftPage()
		{
			InitializeComponent();

			BindingContext = ShiftViewModel.Instance;
		}
	}
}