using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Druid.Satchel;
using System.Windows.Input;

namespace Dwares.Rookie.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PicButton : ContentView, ICommandHolder
	{
		WritMixin wmix;
		//public event EventHandler Tapped;

		public PicButton ()
		{
			wmix = new WritMixin(this);

			InitializeComponent();
		}

		public ImageSource ImageSource {
			get => image.Source;
			set => image.Source = value;
		}

		public string LabelText {
			get => label.Text;
			set => label.Text = value;
		}

		public ICommand Command { get; set; }

		public WritCommand WritCommand {
			get => wmix.WritCommand;
			set => wmix.WritCommand = value;
		}

		public string Writ {
			get => wmix.Writ;
			set => wmix.Writ = value;
		}

		// TapGestureRecognizer handler.
		void OnTapped(object sender, EventArgs args)
		{
			//if (IsEnabled) {
			//	Tapped?.Invoke(sender, args);
			//}

			if (IsEnabled && Command != null && Command.CanExecute(null)) {
				Command.Execute(null);
			}
		}
	}
}
