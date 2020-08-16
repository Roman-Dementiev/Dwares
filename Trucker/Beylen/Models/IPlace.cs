using System;
using System.Collections.Generic;
using System.Text;

namespace Beylen.Models
{
	public interface IPlace
	{
		string CodeName { get; }
		string FullName { get; }
		string Address { get; }
	}
}
