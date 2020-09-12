using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.Effects
{
	public class ListViewDraggableEffect : RoutingEffect
	{
		//static ClassRef @class = new ClassRef(typeof(CollectionViewDraggableEffect));

		public ListViewDraggableEffect() :
			base($"Dwares.Druid.Effects.{nameof(ListViewDraggableEffect)}")
		{
			//Debug.EnableTracing(@class);
		}
	}
}
