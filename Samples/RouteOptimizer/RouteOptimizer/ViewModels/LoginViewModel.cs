﻿using System;
using Dwares.Druid.ViewModels;
using Xamarin.Forms;
using RouteOptimizer.Views;


namespace RouteOptimizer.ViewModels
{
	public class LoginViewModel : ViewModel
	{
		public Command LoginCommand { get; }

		public LoginViewModel()
		{
			LoginCommand = new Command(OnLoginClicked);
		}

		private async void OnLoginClicked(object obj)
		{
			// Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
			await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
		}
	}
}
