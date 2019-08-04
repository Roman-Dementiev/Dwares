﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;


namespace Drive.Themes
{
	public class BaseTheme : UITheme
	{
		public BaseTheme() : base(new BaseThemeResources()) { }
	}

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BaseThemeResources : ResourceDictionary
	{
		public BaseThemeResources()
		{
			InitializeComponent();
		}	
	}
}