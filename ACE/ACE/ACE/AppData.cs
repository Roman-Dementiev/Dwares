using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using ACE.Models;


namespace ACE
{
	public static partial class AppData
	{
		static ClassRef @class = new ClassRef(typeof(AppData));

		static ContactList contacts = null;
		public static ContactList Contacts => LazyInitializer.EnsureInitialized(ref contacts);

		static Schedule schedule = null;
		public static Schedule Schedule => LazyInitializer.EnsureInitialized(ref schedule);

		public static Route Route => Schedule.Route;
	}
}
