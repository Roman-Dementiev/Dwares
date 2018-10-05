using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Toolkit
{
	public interface IWire
	{
		void Send(string message);
		void Send(object sender, object message);
	}

	public abstract class Wire: IWire
	{
		public abstract void Send(string message);

		public virtual void Send(object sender, object message)
		{
			var format = GetFormat(sender, message);
			var text = String.Format(format, sender, message);
			Send(text);
		}

		public virtual string GetFormat(object sender, object message)
		{
			if (Strings.IsNullOrEmptyString(sender))
				return "{1}";

			if (Strings.IsNullOrEmptyString(message))
				return "[{0}]";

			return "[{0}] {1}";
		}
	}
}
