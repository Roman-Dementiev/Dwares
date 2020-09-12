using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid.Services;
using DraggableListView.Models;


namespace DraggableListView.Services
{
	public interface IDataStore
	{
		Task LoadSquad(Squad squad);
		Task LoadMarines(ICollection<Marine> collection);
	}

	public static class DataStore
	{
		public static IDataStore Instance {
			get => DependencyService<IDataStore>.GetInstance(ref instance);
			set => DependencyService<IDataStore>.SetInstance(ref instance, value);
		}
		static DependencyService<IDataStore> instance;
	}
}

