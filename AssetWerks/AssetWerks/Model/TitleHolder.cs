using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetWerks.Model
{
	public interface ITitleHolder
	{
		string Title { get; }
	}

	public class TitleHolder : NotifyPropertyChanged, ITitleHolder
	{
		public TitleHolder() { }
		public TitleHolder(string title)
		{
			Title = title;
		}

		string title;
		public string Title { 
			get => title; 
			set => SetProperty(ref title, value); 
		}

		public override string ToString() => Title;

		public static T ByTitle<T>(IList<T> list, string name) where T : ITitleHolder
		{
			foreach (var item in list) {
				if (item.Title == name)
					return item;
			}
			return default(T);
		}
	}
}
