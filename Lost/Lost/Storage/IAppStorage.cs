using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Lost.Models;


namespace Lost.Storage
{
	public interface IAppStorage
	{
		Task<Shift> Initialize();

		Task NewShift(Shift shift);
		Task StartShift();
		Task FirstPickup();
		Task LastDropoff();
		Task EndShift();

		Task ListShifts(ICollection<PeriodInfo> list);
	}
}
