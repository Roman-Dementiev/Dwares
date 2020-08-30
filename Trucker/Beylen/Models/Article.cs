using System;
using Dwares.Druid;
using Dwares.Dwarf;


namespace Beylen.Models
{
	public class Article : Model
	{
		//static ClassRef @class = new ClassRef(typeof(Article));

		public Article()
		{
			//Debug.EnableTracing(@class);
		}

		//public int Seq { get; set; }
		public Produce Produce {
			get => produce;
			set => SetProperty(ref produce, value);
		}
		Produce produce;

		public decimal Quantity {
			get => quantity;
			set => SetProperty(ref quantity, value);
		}
		decimal quantity;
		
		public string Unit {
			get => unit;
			set => SetProperty(ref unit, value);
		}
		string unit;

		public decimal UnitPrice {
			get => unitPrice;
			set => SetProperty(ref unitPrice, value);
		}
		decimal unitPrice;

		public decimal TotalPrice {
			get => totalPrice;
			set => SetProperty(ref totalPrice, value);
		}
		decimal totalPrice;

		public string Note {
			get => note;
			set => SetProperty(ref note, value);
		}
		string note;
	}
}
