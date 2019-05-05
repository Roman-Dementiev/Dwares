using System;
using System.Collections.Generic;


namespace AssetWerks.Model
{
	public class IconList : List<Icon>, ITitleHolder
	{
		public IconList(string title)
		{
			Title = title;
		}

		public string Title { get; set; }
	}

}
