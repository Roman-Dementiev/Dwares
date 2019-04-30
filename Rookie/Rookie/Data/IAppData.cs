using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Rookie.Models;


namespace Dwares.Rookie.Data
{
	public interface IAppData
	{
		Task Initialize();
		void Reset();

		Task PutProperty(string properyName, Object value);

		Task<IWorkPeriod> GetLastPeriod(string periodId);
		Task<IWorkPeriod> StartPeriod(DateTime time, int milage);
		Task<IWorkPeriod> FinishPeriod(IWorkPeriod period, DateTime time, int milage);
	}
}
