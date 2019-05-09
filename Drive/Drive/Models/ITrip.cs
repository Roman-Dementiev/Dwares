using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Models
{
	public interface ITrip
	{
		DateTime PickupTime { get; }
		string PickupAddress { get; }

		DateTime DropoffTime { get; }
		string DropoffAddress { get; }
	}
}
