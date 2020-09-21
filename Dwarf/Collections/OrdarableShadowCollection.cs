using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;

namespace Dwares.Dwarf.Collections
{
	public class OrdarableShadowCollection<ShadowItem, SourceItem> : ShadowCollection<ShadowItem, SourceItem>, IOrderableCollection
		//where SourceItem : class
		where ShadowItem : class
	{
		//static ClassRef @class = new ClassRef(typeof(OrdarableShadowCollection));

		public event EventHandler OrderChanged;

		public OrdarableShadowCollection(ObservableCollection<SourceItem> source, Func<SourceItem, ShadowItem> itemFactory) :
			base(source, itemFactory)
		{
			//Debug.EnableTracing(@class);

			this.CollectionChanged += (s,e) => ResetOrdinals();
		}


		public virtual void ChangeOrder(int oldIndex, int newIndex)
		{
			Debug.AssertNotNull(Source);

			Source.Move(oldIndex, newIndex);
			FireOrderChanged();
		}

		protected void FireOrderChanged()
		{
			OrderChanged?.Invoke(this, EventArgs.Empty);
		}

		public void ResetOrdinals(OrdinalType ordinalType = OrdinalType.Default)
		{
			OrderableCollection<ShadowItem>.ResetOrdinals(Items, ordinalType, 1);
		}
	}
}
