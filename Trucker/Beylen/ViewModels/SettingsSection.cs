using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Beylen.ViewModels
{
	public class SettingsSection : PropertyNotifier
	{
		//static ClassRef @class = new ClassRef(typeof(SettingsSection));

		public SettingsSection()
		{
			//Debug.EnableTracing(@class);
		}

		public string IconSource { get; set; }
		public string Title { get; set; }
		//public string Value { get; set; }

		string _value;
		public string Value {
			get => _value;
			set => SetProperty(ref _value, value);
		}

		public Func<Page, SettingsSection, Task> Action { get; set; }
	}
}
