using System;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.UI
{
	public class CheckMark : ToggleGlyph
	{
		public const string defaultCheckedGlyph = StdGlyph.CheckedBallot;
		public const string defaultUncheckedGlyph = StdGlyph.UncheckedBallot;

		public CheckMark() : this(defaultCheckedGlyph) { }

		public CheckMark(string checkedGlyph, string uncheckedGlyph = defaultUncheckedGlyph) : 
			base(checkedGlyph, uncheckedGlyph)
		{ }
	}

	public class BoldCheckMark : CheckMark
	{
		public BoldCheckMark() : base(StdGlyph.BoldCheckedBallot) { }
	}

	public class HeavyCheckMark : CheckMark
	{
		public HeavyCheckMark() : base(StdGlyph.HeavyCheckedBallot) { }
	}

	public class BallotMark : CheckMark
	{
		public BallotMark() : 
			base(StdGlyph.WhitePointBallot, StdGlyph.WhiteBallot)
		{ }
		
		public BallotMark(string checkedGlyph, string uncheckedGlyph= StdGlyph.WhiteBallot) :
			base(checkedGlyph, uncheckedGlyph)
		{ }
	}
}
