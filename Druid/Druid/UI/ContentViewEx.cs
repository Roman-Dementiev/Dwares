﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Druid.Satchel;
using Dwares.Dwarf;

namespace Dwares.Druid.UI
{
	public class ContentViewEx : ContentView, ITitleHolder, IToolbarHolder, ITargeting
	{
		public ContentViewEx()
		{
			//UITheme.CurrentThemeChanged += UITheme_CurrentThemeChanged;
		}

		//private void UITheme_CurrentThemeChanged(object sender, EventArgs e)
		//{

		//}

		public static readonly BindableProperty TitleProperty =
			BindableProperty.Create(
				nameof(Title),
				typeof(string),
				typeof(ContentViewEx),
				defaultValue: string.Empty
				);

		public string Title {
			set { SetValue(TitleProperty, value); }
			get { return (string)GetValue(TitleProperty); }
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (BindingContext is ITitleHolder titleHolder) {
				Title = titleHolder.Title;
			}
		}


		public IList<ToolbarItem> ToolbarItems { get; } = new List<ToolbarItem>();

		public virtual Element GetTargetElement()
		{
			return Content;
		}
	}
}