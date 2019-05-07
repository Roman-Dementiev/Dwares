using System;
using System.Collections.Generic;


namespace AssetWerks.Model
{
	public class List : List<Icon>, ITitleHolder
	{
		public List(string title)
		{
			Title = title;
		}

		public string Title { get; set; }
	}

}
