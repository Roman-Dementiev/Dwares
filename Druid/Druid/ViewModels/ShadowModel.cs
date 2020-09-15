using System;
using System.Collections.Generic;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;

namespace Dwares.Druid.ViewModels
{
	public class ShadowModel<TSource> : PropertyNotifier, ISelectable where TSource : class, IModel
	{
		//static ClassRef @class = new ClassRef(typeof(ShadowModel));

		public ShadowModel()
		{
			//Debug.EnableTracing(@class);
		}

		public ShadowModel(TSource source)
		{
			//Debug.EnableTracing(@class);
			Source = source;
		}

		public TSource Source {
			get => source;
			set {
				//if (Object.Equals(value, source))
				//	return;
				if (value == source)
					return;

				if (source != null) {
					source.ModelChanged -= OnSourceChanged;
				}

				source = value;

				if (source != null) {
					source.ModelChanged += OnSourceChanged;
				}

				IsModified = false;
				FirePropertyChanged();
			}
		}
		TSource source;

		public bool IsSelected {
			get => isSelected;
			set {
				if (SetProperty(ref isSelected, value, setModified: false)) {
					OnSelectedChanged();
				}
			}
		}
		bool isSelected;

		protected virtual void OnSourceChanged(object sender, ModelChangedEventArgs e)
		{
			FirePropertiesChanged(e.ChangedProperties);
		}

		protected virtual void OnSelectedChanged() { }

	}
}
