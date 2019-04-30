using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Rookie.Models
{
	public interface IWorkPeriod
	{
		string Id { get; }

		DateTime StartTime { get; set; }
		DateTime EndTime { get; set; }
		int StartMileage { get; set; }
		int EndMileage { get; set; }
		decimal Distance { get; set; }
		decimal Cash { get; set; }
		decimal Credit { get; set; }
		decimal Lease { get; set; }
		decimal Gas { get; set; }
		decimal Expenses { get; set; }
	}
}
