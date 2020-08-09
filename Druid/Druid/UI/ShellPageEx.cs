using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Xamarin.Forms;
using System.Windows.Input;


namespace Dwares.Druid.UI
{
	public class ShellPageEx : ContentPageEx
	{
		//static ClassRef @class = new ClassRef(typeof(ShellPageEx));

		public ShellPageEx()
		{
			//Debug.EnableTracing(@class);

			//var bbb = Shell.GetBackButtonBehavior(this);
		}

		// TODO: Bindable properties

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
					if (value && bbb.Command == null) {
						bbb.Command = ShellEx.GoToMainCommand;
					}
				} else {
					bbb = new BackButtonBehavior { IsEnabled = value, Command = ShellEx.GoToMainCommand };
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

	}
}
