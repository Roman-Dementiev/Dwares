using System;
using System.Collections.Generic;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public interface IToolbarHolder
	{
		IList<ToolbarItem> ToolbarItems { get; }
	}
}
