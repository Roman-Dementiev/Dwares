using System;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Druid.Satchel
{
	public interface ITitleHolder
	{
		string Title { get; }
	}

	public class TitleHolder : PropertyNotifier, ITitleHolder
	{
		string title;
		public string Title {
			get => title;
			set => SetProperty(ref title, value);
		}
	}
}
