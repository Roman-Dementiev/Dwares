using System;
using System.Collections.ObjectModel;
using System.Threading;
using Dwares.Dwarf;
using Passket.Models;


namespace Passket
{
	public class AppData
	{
		//static ClassRef @class = new ClassRef(typeof(AppData));

		static AppData instance;
		public static AppData Instance {
			get => LazyInitializer.EnsureInitialized(ref instance);
		}

		ObservableCollection<Group> groups;

		public AppData()
		{
			//Debug.EnableTracing(@class);

			var accounts = new Group { Icon = "account", Name = "Web Accounts" };
			accounts.Add(new Record() { Name = "Accounr 1" });
			accounts.Add(new Record() { Name = "Accounr 2" });
			accounts.Add(new Record() { Name = "Accounr 3" });

			groups = new ObservableCollection<Group>();
			groups.Add(accounts);
			groups.Add(new Group { Icon = "finances", Name = "Finances" });
			groups.Add(new Group { Icon = "wallet", Name = "Wallet" });
			groups.Add(new Group { Icon = "code", Name = "Codes" });
			groups.Add(new Group { Icon = "note", Name = "Notes" });
		}

		public static ObservableCollection<Group> Groups => Instance.groups;
	}
}
