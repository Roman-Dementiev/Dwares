using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using SkiaSharp;

namespace AssetWerks.Model
{
	public abstract class Badge : TitleHolder
	{
		protected Badge(string name) : this(name, 0) { }

		protected Badge(string name, float iconInset) : 
			this(name, new SKRect(iconInset, iconInset, iconInset, iconInset))
		{
		}

		protected Badge(string name, SKRect iconInset) : 
			base(name)
		{
			IconInset = iconInset;
		}

		public SKRect IconInset { get; set; }

		public abstract void Draw(SKCanvas canvas, SKRect rect, SKColor? color);

		static ObservableCollection<Badge> list;
		public static ObservableCollection<Badge> List {
			get => LazyInitializer.EnsureInitialized(ref list, () => {
				var list = new ObservableCollection<Badge>();
				list.Add(new NoBadge());
				list.Add(new ImageBadge());
				list.Add(new CircleBadge());
				return list;
			});
		}

		public static Badge ByName(string name) => ByTitle(List, name);
	}

	public class NoBadge : Badge
	{
		public NoBadge() : base("None") { }

		public override void Draw(SKCanvas canvas, SKRect rect, SKColor? color) { }
	}
}
