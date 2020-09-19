using System;
using System.Threading.Tasks;


namespace Dwares.Druid.Services
{
	public interface IWritExecutor
	{
		Task ExecuteWrit(string writ);
		bool CanExecuteWrit(string writ);
	}

	public interface IWritService
	{
		IWritExecutor GetExecutor(string writ);
	}

	public static class WritService
	{
		public static IWritService Instance {
			get => DependencyService.GetInstance<IWritService, ReflectionWritSvc>(ref instance);
			set => DependencyService.SetInstance(ref instance, value);
		}
		static IWritService instance;

		public static IWritExecutor GetExecutor(string writ)
		{
			return Instance?.GetExecutor(writ);
		}

		public static async Task ExecuteWrit(string writ)
		{
			var executor = GetExecutor(writ);
			if (executor?.CanExecuteWrit(writ)==true) {
				await executor.ExecuteWrit(writ);
			}
		}
	}

}
