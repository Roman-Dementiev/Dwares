using System;


namespace Dwares.Druid.Essential
{
	public class SumbolImageSource : ActionImageSourceBase
	{
		public static readonly OnPlatform<string> DefaultFilenameFormat = new OnPlatform<string> {
			Default = "{0:g}.png",
			Android = "ic_action_{0:g}.png",
			iOS = "{0:g}.png",
			UWP = "Images/{1:x4}.png"
		};

		public SumbolImageSource(SymbolEx symbol) :
			base(DefaultFilenameFormat)
		{
			Symbol = symbol;
		}

		public SymbolEx Symbol { get; }
		public override string Action => Symbol.Name();
		public override string Filename => String.Format(FilenameFormat, Symbol, (int)Symbol);
	}

}
