using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Druid
{
	public class Command : Xamarin.Forms.Command
	{
		public Command(string uid, Action<object> execute) : base(execute) { Uid = uid; }
		public Command(string uid, Action execute) : base(execute) { Uid = uid; }
		public Command(string uid, Action<object> execute, Func<object, bool> canExecute) : base(execute) { Uid = uid; }
		public Command(string uid, Action execute, Func<bool> canExecute) : base(execute) { Uid = uid; }

		public String Uid { get; set; }
	}
}
