using System;
using System.Threading.Tasks;


namespace Drive.Storage
{
	public interface IAppStorage : IDisposable
	{
		Task Initialize();

		Task LoadData();
	}
}
