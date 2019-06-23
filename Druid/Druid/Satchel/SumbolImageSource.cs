using System;
using Xamarin.Forms;

namespace Dwares.Druid.Satchel
{
	public class SumbolImageSource : ActionImageSource
	{
		public SumbolImageSource(SymbolEx symbol) :
			base(symbol.ToString(), ArtProvider.kGroupSymbol)
		{
			Symbol = symbol;
		}

		public SymbolEx Symbol { get; }

		protected override ImageSource GetImageSource(IArtProvider provider)
		{
			return provider.GetImageSource(Group, string.Format("{0:x4}", (int)Symbol));
		}
	}

}
