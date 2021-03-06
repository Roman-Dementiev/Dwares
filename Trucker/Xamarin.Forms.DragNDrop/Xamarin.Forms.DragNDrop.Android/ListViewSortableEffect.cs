﻿using AWidget = Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.DragNDrop.Droid;

[assembly: ResolutionGroupName("Xamarin.Forms.DragNDrop")]
[assembly: ExportEffect(typeof(ListViewSortableEffect), nameof(ListViewSortableEffect))]
namespace Xamarin.Forms.DragNDrop.Droid
{
    public class ListViewSortableEffect : Xamarin.Forms.Platform.Android.PlatformEffect
    {
        private DragListAdapter _dragListAdapter = null;

        protected override void OnAttached()
        {
            var element = Element as ListView;

            if (Control is AWidget.ListView listView)
            {
                _dragListAdapter = new DragListAdapter(listView, element);
                listView.Adapter = _dragListAdapter;
                listView.SetOnDragListener(_dragListAdapter);
                listView.OnItemLongClickListener = _dragListAdapter;
            }
        }

        protected override void OnDetached()
        {
            if (Control is AWidget.ListView listView)
            {
                listView.Adapter = _dragListAdapter.WrappedAdapter;

                // TODO: Remove the attached listeners
            }
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            if (args.PropertyName == Sorting.IsSortableProperty.PropertyName)
            {
                _dragListAdapter.DragDropEnabled = Sorting.GetIsSortable(Element);
            }
        }
    }
}
