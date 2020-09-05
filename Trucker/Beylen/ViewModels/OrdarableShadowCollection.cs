using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Xamarin.Forms.DragNDrop;


namespace Beylen.ViewModels
{
	public class OrdarableShadowCollection<ShadowItem, SourceItem> : ShadowCollection<ShadowItem, SourceItem>, IOrderable
		where ShadowItem : class
	{
		//static ClassRef @class = new ClassRef(typeof(OrdarableShadowCollection));

		public event EventHandler OrderChanged;

		public OrdarableShadowCollection(ObservableCollection<SourceItem> source, Func<SourceItem, ShadowItem> itemFactory) :
			base(source, itemFactory)
		{
			//Debug.EnableTracing(@class);
		}

		public virtual void ChangeOrdinal(int oldIndex, int newIndex)
		{
			Debug.AssertNotNull(Source);

#if false
			var priorIndex = oldIndex;
			var latterIndex = newIndex;

			var changedItem = Items[oldIndex];
			if (newIndex < oldIndex) {
				// add one to where we delete, because we're increasing the index by inserting
				priorIndex += 1;
			} else {
				// add one to where we insert, because we haven't deleted the original yet
				latterIndex += 1;
			}

			Items.Insert(latterIndex, changedItem);
			Items.RemoveAt(priorIndex);

			OrderChanged?.Invoke(this, EventArgs.Empty);

			OnCollectionChanged(
				new NotifyCollectionChangedEventArgs(
					NotifyCollectionChangedAction.Move,
					changedItem,
					newIndex,
					oldIndex));
#else
			Source.Move(oldIndex, newIndex);
			FireOrderChanged();
#endif
		}

		protected void FireOrderChanged()
		{
			OrderChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
