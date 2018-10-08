using System;
using System.Collections.Generic;


namespace Dwares.Dwarf.Collections
{
	public class Disposables: IDisposable
	{
		List<IDisposable> list = new List<IDisposable>();

		public void Dispose()
		{
			if (list != null) {
				foreach (var item in list) {
					try {
						item?.Dispose();
					}
					catch (Exception ex) {
						Debug.ExceptionCaught(ex);
					}
				}

				list = null;
			}
		}

		public void Attach(IDisposable item) => list.Add(item);
		public void Detach(IDisposable item) => list.Remove(item);
	}
}

