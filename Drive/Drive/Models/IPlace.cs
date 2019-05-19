using System;
using Dwares.Druid.Satchel;

namespace Drive.Models
{
	public interface IPlace : ITitleHolder
	{
		string Address { get; set; }
	}
}
