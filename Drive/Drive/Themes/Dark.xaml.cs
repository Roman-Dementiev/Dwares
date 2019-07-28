﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;


namespace Drive.Themes
{
	public class DarkTheme : UITheme
	{
		public DarkTheme() : base(new DarkThemeResources()) { }
	}

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DarkThemeResources : ResourceDictionary
	{
		public DarkThemeResources()
		{
			InitializeComponent();
		}
	}
}