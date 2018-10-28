using System;
using System.Collections.Generic;
using System.Text;
using Dwares.Druid.Support;


namespace Dwares.Druid.UI
{
	public class CheckBoxGlyph : ToggleGlyph
	{
		//public const string uncheckedBallot = "\u2610";
		//public const string boldBallot = "\U0001F5F9";
		//public const string coloredBallot = "\u2611";
		//public const string greenBallot = "\u2705";
		public const string defaultCheckedBallot = StdGlyph.CheckedBallot;

		public CheckBoxGlyph() : this(defaultCheckedBallot) { }

		public CheckBoxGlyph(string checkedGlyph, string uncheckedGlyph = StdGlyph.UncheckedBallot) : 
			base(checkedGlyph, uncheckedGlyph)
		{ }
	}

	public class BoldCheckBoxGlyph : CheckBoxGlyph
	{
		public BoldCheckBoxGlyph() : base(StdGlyph.BoldCheckedBallot) { }
	}

	public class HeavyCheckBoxGlyph : CheckBoxGlyph
	{
		public HeavyCheckBoxGlyph() : base(StdGlyph.HeavyCheckedBallot) { }
	}

}
