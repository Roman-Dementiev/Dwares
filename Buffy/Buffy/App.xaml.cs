﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Buffy
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			MainPage = new AppShell();
		}

		protected override async void OnStart()
		{
			await InitData();
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
