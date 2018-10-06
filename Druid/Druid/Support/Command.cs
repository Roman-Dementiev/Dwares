using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Druid.Support
{
	public class CommandEx: Xamarin.Forms.Command
	{
		public CommandEx(string uid, Action<object> execute) : base(execute) { Uid = uid; }
		public CommandEx(string uid, Action execute) : base(execute) { Uid = uid; }
		public CommandEx(string uid, Action<object> execute, Func<object, bool> canExecute) : base(execute) { Uid = uid; }
		public CommandEx(string uid, Action execute, Func<bool> canExecute) : base(execute) { Uid = uid; }

		public String Uid { get; set; }
	}
}
