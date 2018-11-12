using System;
using System.ComponentModel;


namespace Dwares.Dwarf.Toolkit
{
	public class ShadowIten<T> : SelectableItem where T : INotifyPropertyChanged
	{
		public ShadowIten(T source)
		{
			SetSource(source);
		}

		T source;
		public T Source {
			get => source;
			set => SetSource(source);
		}

		public virtual void SetSource(T newSource)
		{
			if (Equals(newSource, source))
				return;

			if (source != null) {
				source.PropertyChanged -= Source_PropertyChanged;
			}

			if (newSource != null) {
				newSource.PropertyChanged += Source_PropertyChanged;
			}

			source = newSource;
			FirePropertyChanged(nameof(Source));
		}

		void Source_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnSourcePropertyChanged(e.PropertyName);
		}

		protected virtual void OnSourcePropertyChanged(string propertyName)
		{
			FirePropertyChanged(propertyName);
		}
	}
}
