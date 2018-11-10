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

		public SumbolImageSource(SymbolEx symbol, string filenameFormat = null) :
			base(filenameFormat)
		{
			Symbol = symbol;
		}

		public SymbolEx Symbol { get; }
		//public override string Icon => Symbol.Name();

		public override string Filename {
			get {
				var format = FilenameFormat ?? DefaultFilenameFormat;
				return String.Format(format, Symbol, (int)Symbol);
			}
		}
	}

}
