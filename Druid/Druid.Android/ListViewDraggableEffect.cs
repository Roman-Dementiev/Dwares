using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xamarin.Forms;
using Dwares.Druid.UI;
using Dwares.Druid.Android;

using AListView = Android.Widget.ListView;
using XListView = Xamarin.Forms.ListView;


[assembly: ResolutionGroupName("Dwares.Druid.Effects")]
[assembly: ExportEffect(typeof(Dwares.Druid.Android.ListViewDraggableEffect), nameof(ListViewDraggableEffect))]

namespace Dwares.Druid.Android
{
	public class ListViewDraggableEffect : Xamarin.Forms.Platform.Android.PlatformEffect
	{
		DragListAdapter dragListAdapter = null;

		protected override void OnAttached()
		{
			if (Control is AListView listView && Element is XListView element) {
				dragListAdapter = new DragListAdapter(listView, element);
				listView.Adapter = dragListAdapter;
				listView.SetOnDragListener(dragListAdapter);
				listView.OnItemLongClickListener = dragListAdapter;
			}
		}

		protected override void OnDetached()
		{
			if (Control is AListView listView) {
				listView.Adapter = dragListAdapter.WrappedAdapter;

				// TODO: Remove the attached listeners
			}
		}

		protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
		{
			if (args.PropertyName == nameof(ListViewEx.IsDraggable)) {
				dragListAdapter.DragDropEnabled = (Element as ListViewEx)?.IsDraggable == true;
			}
		}
	}
}
