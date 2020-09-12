using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dwares.Druid.UWP;


[assembly: ResolutionGroupName("Dwares.Druid.Effects")]
[assembly: ExportEffect(typeof(CollectionViewDraggableEffect), nameof(CollectionViewDraggableEffect))]

namespace Dwares.Druid.UWP
{
	public class CollectionViewDraggableEffect : Xamarin.Forms.Platform.UWP.PlatformEffect
	{
		protected override void OnAttached()
		{
			// TODO
		}

		protected override void OnDetached()
		{
			// TODO
		}
	}
}
