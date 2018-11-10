using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Toolkit
{
	public interface ICreateByName<T>
	{
		bool CanCreate(string name);

		T New(string name);
	}

	public class CreateByName<T> : ICreateByName<T> where T: class
	{
		Dictionary<string, Func<T>> dict = new Dictionary<string, Func<T>>();

		public void Add(string name, Func<T> factory)
		{
			dict.Add(name, factory);
		}

		public void Add<Type>(string name) where Type : T, new()
		{
			dict.Add(name, () => new Type());
		}

		public bool CanCreate(string name) => dict.ContainsKey(name);

		public T New(string name)
		{
			if (dict.ContainsKey(name)) {
				var factory = dict[name];
				return factory?.Invoke();
			} else {
				return null;
			}
		}	
	}
}
