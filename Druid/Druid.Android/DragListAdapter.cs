using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Animation;
using Android.Database;

using Xamarin.Forms;
using Dwares.Dwarf.Collections;
using System.Collections;

using AWidget = Android.Widget;
using AViews = Android.Views;
using AListView = Android.Widget.ListView;
using XListView = Xamarin.Forms.ListView;

namespace Dwares.Druid.Android
{
	internal class DragListAdapter :
		AWidget.BaseAdapter,
		AWidget.IWrapperListAdapter,
		AViews.View.IOnDragListener,
		AWidget.AdapterView.IOnItemLongClickListener
	{
		XListView element;
		AListView listView;
		IListAdapter listAdapter;
		List<AViews.View> translatedItems = new List<AViews.View>();

		public DragListAdapter(AWidget.ListView listView, XListView element)
		{
			this.listView = listView;
			// NOTE: careful, the listAdapter might not always be an IWrapperListAdapter
			this.listAdapter = (listView.Adapter as IWrapperListAdapter)?.WrappedAdapter;
			this.element = element;
		}

		public bool DragDropEnabled { get; set; } = true;


		#region IWrapperListAdapter Members
		public AWidget.IListAdapter WrappedAdapter => listAdapter;
		public override int Count => WrappedAdapter.Count;
		public override bool HasStableIds => WrappedAdapter.HasStableIds;
		public override bool IsEmpty => WrappedAdapter.IsEmpty;
		public override int ViewTypeCount => WrappedAdapter.ViewTypeCount;
		public override bool AreAllItemsEnabled() => WrappedAdapter.AreAllItemsEnabled();
		public override Java.Lang.Object GetItem(int position) => WrappedAdapter.GetItem(position);
		public override long GetItemId(int position) => WrappedAdapter.GetItemId(position);
		public override int GetItemViewType(int position) => WrappedAdapter.GetItemViewType(position);
		public override bool IsEnabled(int position) => WrappedAdapter.IsEnabled(position);

		public override AViews.View GetView(int position, AViews.View convertView, AViews.ViewGroup parent)
		{
			var view = WrappedAdapter.GetView(position, convertView, parent);
			view.SetOnDragListener(this);
			return view;
		}

		public override void RegisterDataSetObserver(DataSetObserver observer)
		{
			base.RegisterDataSetObserver(observer);
			WrappedAdapter.RegisterDataSetObserver(observer);
		}

		public override void UnregisterDataSetObserver(DataSetObserver observer)
		{
			base.UnregisterDataSetObserver(observer);
			WrappedAdapter.UnregisterDataSetObserver(observer);
		}

		#endregion IWrapperListAdapter Members


		#region IOnItemLongClickListener Members
		public bool OnItemLongClick(AWidget.AdapterView parent, AViews.View view, int position, long id)
		{
			object selectedItem = null;
			//if (_element.ItemsSource.GetType().GetInterfaces().Any(x => x == typeof(IGroupedOrderable)))

			if (element.ItemsSource is IGroupedOrderableCollection grouped) {
				selectedItem = grouped.GetItemFromFlatIndex(id);
			} else {
				selectedItem = ((IList)element.ItemsSource)[(int)id];
			}

			// Creating drag state
			DragItem dragItem = new DragItem(NormalizeListPosition(position), view, selectedItem);

			// Creating a blank clip data object (we won't depend on this) 
			var data = ClipData.NewPlainText(string.Empty, string.Empty);

			// Creating the default drag shadow for the item (the translucent version of the view)
			// NOTE: Can create a custom view in order to change the dragged item view
			AViews.View.DragShadowBuilder shadowBuilder = new AViews.View.DragShadowBuilder(view);

			// Setting the original view cell to be invisible
			view.Visibility = AViews.ViewStates.Invisible;

			// NOTE: this method is introduced in Android 24, for earlier versions the StartDrag method should be used
			view.StartDragAndDrop(data, shadowBuilder, dragItem, 0);

			return true;
		}

		#endregion IWrapperListAdapter Members

		#region IOnDragListener Members
		public bool OnDrag(AViews.View view, AViews.DragEvent e)
		{
			switch (e.Action) {
			case AViews.DragAction.Started:
				break;
			case AViews.DragAction.Entered:
				System.Diagnostics.Debug.WriteLine($"DragAction.Entered from {view.GetType()}");

				if (!(view is AWidget.ListView)) {
					var dragItem = (DragItem)e.LocalState;

					var targetPosition = InsertOntoView(view, dragItem);

					dragItem.Index = targetPosition;

					// Keep a list of items that has translation so we can reset
					// them once the drag'n'drop is finished.
					translatedItems.Add(view);
					listView.Invalidate();
				}
				break;
			case AViews.DragAction.Location:
				break;
			case AViews.DragAction.Exited:
				System.Diagnostics.Debug.WriteLine($"DragAction.Entered from {view.GetType()}");

				if (!(view is AWidget.ListView)) {
					var positionEntered = GetListPositionForView(view);

					System.Diagnostics.Debug.WriteLine($"DragAction.Exited index {positionEntered}");
				}
				break;
			case AViews.DragAction.Drop:
				System.Diagnostics.Debug.WriteLine($"DragAction.Drop from {view.GetType()}");
				break;
			case AViews.DragAction.Ended:
				System.Diagnostics.Debug.WriteLine($"DragAction.Ended from {view.GetType()}");

				if (!(view is AWidget.ListView)) {
					return false;
				}

				var mobileItem = (DragItem)e.LocalState;

				mobileItem.View.Visibility = AViews.ViewStates.Visible;

				foreach (var v in translatedItems) {
					v.TranslationY = 0;
				}

				translatedItems.Clear();

				if (element.ItemsSource is IOrderableCollection orderable) {
					orderable.ChangeOrder(mobileItem.OriginalIndex, mobileItem.Index);
				}

				break;
			}

			return true;
		}
		#endregion IOnDragListener Members


		#region Private Members        
		private int InsertOntoView(AViews.View view, DragItem item)
		{
			var positionEntered = GetListPositionForView(view);
			var correctedPosition = positionEntered;

			// If the view already has a translation, we need to adjust the position
			// If the view has a positive translation, that means that the current position
			// is actually one index down then where it started.
			// If the view has a negative translation, that means it actually moved
			// up previous now we will need to move it down.
			if (view.TranslationY > 0) {
				correctedPosition += 1;
			} else if (view.TranslationY < 0) {
				correctedPosition -= 1;
			}

			// If the current index of the dragging item is bigger than the target
			// That means the dragging item is moving up, and the target view should
			// move down, and vice-versa
			var translationCoef = item.Index > correctedPosition ? 1 : -1;

			// We translate the item as much as the height of the drag item (up or down)
			var translationTarget = view.TranslationY + (translationCoef * item.View.Height);

			ObjectAnimator anim = ObjectAnimator.OfFloat(view, "TranslationY", view.TranslationY, translationTarget);
			anim.SetDuration(100);
			anim.Start();

			return correctedPosition;
		}
		private int GetListPositionForView(AViews.View view)
		{
			return NormalizeListPosition(listView.GetPositionForView(view));
		}
		private int NormalizeListPosition(int position)
		{
			// We do not want to count the headers into the item source index
			return position - listView.HeaderViewsCount;
		}
		#endregion
	}
}
