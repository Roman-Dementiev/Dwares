﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;


namespace Drive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPageEx
	{
		public MainPage()
		{
			try {
				InitializeComponent();
			} catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
		}
	}
}