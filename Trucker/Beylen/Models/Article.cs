using System;
using Dwares.Dwarf;


namespace Beylen.Models
{
	public class Article
	{
		//static ClassRef @class = new ClassRef(typeof(Article));

		public Article()
		{
			//Debug.EnableTracing(@class);
		}

		//public int Seq { get; set; }
		public Produce Produce { get; set; }
		public int Quantity { get; set; }
		public string Unit { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal TotalPrice { get; set; }
		public string Note { get; set; }
	}
}
