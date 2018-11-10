using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Toolkit
{
	public class Suspender
	{
		uint isSuspended = 0;
		bool hasRequests = false;
		Action action;

		public Suspender(Action action)
		{
			this.action = action;
		}

		public void Suspend()
		{
			isSuspended++;
		}

		public bool Resume()
		{
			if (isSuspended > 0) {
				isSuspended--;
				if (isSuspended == 0) {
					action?.Invoke();
					hasRequests = false;
					return true;
				}
			}
			return false;
		}

		public bool RequestAction()
		{
			if (isSuspended > 0) {
				hasRequests = true;
				return false;
			}

			action?.Invoke();
			return false;
		}
	}
}
