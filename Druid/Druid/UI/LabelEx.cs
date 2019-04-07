using System;
using Dwares.Dwarf;
using Dwares.Druid.UI;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class LabelEx : Label
	{
		//static ClassRef @class = new ClassRef(typeof(LabelEx));

		public LabelEx() : this(typeof(LabelEx)) { }

 		protected LabelEx(Type type)
		{
			//Debug.EnableTracing(@class);

			this.ApplyTheme(type);
			UITheme.CurrentThemeChanged += OnCurrentUIhemeChanged;
		}

		private void OnCurrentUIhemeChanged(object sender, EventArgs e)
		{
			this.ApplyTheme();
		}
	}

	public class StaticText : LabelEx
	{
		public StaticText() : base(typeof(StaticText)) { }
	}
}
