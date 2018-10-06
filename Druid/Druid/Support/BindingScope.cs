using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;


namespace Dwares.Druid.Support
{
	public class BindingScope : CommandTarget, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public BindingScope(ICommandTarget chainOfCommand) : base(chainOfCommand) {}

		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (Object.Equals(storage, value))
				return false;

			storage = value;
			OnPropertyChanged(propertyName);
			return true;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public static partial class Extensions
	{
		public static void SetScope(this BindableObject obj, BindingScope scope)
		{
			obj.BindingContext = scope;
		}

		public static BindingScope GetScope(this BindableObject obj)
		{
			return obj.BindingContext as BindingScope;
		}

		public static TScope GetScope<TScope>(this BindableObject obj) where TScope : BindingScope
		{
			return obj.BindingContext as TScope;
		}
	}
}
