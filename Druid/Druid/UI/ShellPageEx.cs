﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Dwares.Dwarf;
using Dwares.Druid;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class ShellPageEx : ContentPageWithOverlay, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(ShellPageEx));

		public ShellPageEx()
		{
			//Debug.EnableTracing(@class);
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		//public static readonly BindableProperty FlavorProperty =
		//	BindableProperty.Create(
		//		nameof(Flavor),
		//		typeof(string),
		//		typeof(ShellPageEx),
		//		propertyChanged: (bindable, oldValue, newValue) => {
		//			if (bindable is ShellPageEx page) {
		//				page.ApplyFlavor();
		//			}
		//		});

		//public string Flavor {
		//	set { SetValue(FlavorProperty, value); }
		//	get { return (string)GetValue(FlavorProperty); }
		//}

		public BackButtonBehavior BackButtonBehavior {
			get => Shell.GetBackButtonBehavior(this);
			set => Shell.SetBackButtonBehavior(this, value);
		}

		public bool BackButtonIsEnabled {
			get {
				var bbb = Shell.GetBackButtonBehavior(this);
				return bbb?.IsEnabled == true;
			}
			set {
				var bbb = Shell.GetBackButtonBehavior(this);
				if (bbb != null) {
					bbb.IsEnabled = value;
				} else {
					bbb = new BackButtonBehavior { IsEnabled = value };
					Shell.SetBackButtonBehavior(this, bbb);
				}
			}
		}

		public ICommand BackButtonCommand {
			get {
				var bbb = Shell.GetBackButtonBehavior(this);
				return bbb?.Command;
			}
			set {
				var bbb = Shell.GetBackButtonBehavior(this);
				if (bbb != null) {
					bbb.Command = value;
				} else {
					bbb = new BackButtonBehavior { Command = value };
					Shell.SetBackButtonBehavior(this, bbb);
				}
			}
		}

		public ImageSource BackButtonIconOverride {
			get {
				var bbb = Shell.GetBackButtonBehavior(this);
				return bbb?.IconOverride;
			}
			set {
				var bbb = Shell.GetBackButtonBehavior(this);
				if (bbb != null) {
					bbb.IconOverride = value;
				} else {
					bbb = new BackButtonBehavior { IconOverride = value };
					Shell.SetBackButtonBehavior(this, bbb);
				}
			}
		}

		public string BackButtonTextOverride {
			get {
				var bbb = Shell.GetBackButtonBehavior(this);
				return bbb?.TextOverride;
			}
			set {
				var bbb = Shell.GetBackButtonBehavior(this);
				if (bbb != null) {
					bbb.TextOverride = value;
				} else {
					bbb = new BackButtonBehavior { TextOverride = value };
					Shell.SetBackButtonBehavior(this, bbb);
				}
			}
		}


		public static ShellPageEx Current { get; private set; }

		protected override void OnAppearing()
		{
			base.OnAppearing();
			Current = this;

			Shell.Current.Navigating += Current_Navigating;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			Current = null;
			
			Shell.Current.Navigating -= Current_Navigating;
		}

		private async void Current_Navigating(object sender, ShellNavigatingEventArgs e)
		{
			if (CustomGoBack && Device.RuntimePlatform == Device.Android)
			{
				if (e.CanCancel) {
					e.Cancel();

					await ExecuteGoBack();
				}
			}
		}

		async Task ExecuteGoBack()
		{
			if (CustomGoBack)
			{
				Shell.Current.Navigating -= Current_Navigating;

				bool done = await GoBack();
				if (!done) {
					Shell.Current.Navigating += Current_Navigating;
				}
			}
		}

		public async Task<bool> GoBack()
		{
			if (CanGoBack != null) {
				bool proceed = await CanGoBack();
				if (!proceed)
					return false;
			}


			try {
				if (GoBackAction != null) {
					await GoBackAction();
				} else {
#if ISSUE_FIXED__GOTO_ASYNC_BACK
					wait Shell.Current.GoToAsync("..");
#else
					await Shell.Current.Navigation.PopAsync();
				}
#endif
				return true;
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				return false;
			}
		}

		public Func<Task<bool>> CanGoBack {
			get => canGoBack;
			set {
				if (value != canGoBack) {
					canGoBack = value;
					if (value == null && GoBackAction == null) {
						CustomGoBack = false;
						BackButtonCommand = null;
					} else {
						CustomGoBack = true;
						BackButtonCommand = CustomBackButtonCommand;
					}
				}
			}
		}
		Func<Task<bool>> canGoBack;

		public Func<Task> GoBackAction {
			get => goBackAction;
			set {
				if (value != goBackAction) {
					goBackAction = value;
					if (value == null && CanGoBack == null) {
						CustomGoBack = false;
						BackButtonCommand = null;
					} else {
						CustomGoBack = true;
						BackButtonCommand = CustomBackButtonCommand;
					}
				}
			}
		}
		Func<Task> goBackAction;


		public bool CustomGoBack { get; private set; }
		
		public Command CustomBackButtonCommand {
			get => customBackButtonCommand ??= new Command(async () => await ExecuteGoBack());
		}
		Command customBackButtonCommand;


		public static async Task TryGoBack()
		{
			if (Current != null) {
				await Current.GoBack();
			}
		}

	}
}
