// Moved to UI.ContentViewEx
//
//using System;
//using System.Collections.Generic;
//using Xamarin.Forms;
//using Dwares.Druid.UI;
//using Dwares.Druid.Satchel;


//namespace Dwares.Druid.Forms
//{
//	public class FormView : ContentView, ITitleHolder, IToolbarHolder
//	{
//		public FormView() { }

//		public static readonly BindableProperty TitleProperty =
//			BindableProperty.Create(
//				nameof(Title),
//				typeof(string),
//				typeof(FormView),
//				defaultValue: string.Empty
//				);

//		public string Title {
//			set { SetValue(TitleProperty, value); }
//			get { return (string)GetValue(TitleProperty); }
//		}


//		public IList<ToolbarItem> ToolbarItems { get; } = new List<ToolbarItem>();
//	}
//}
