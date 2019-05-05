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
		protected Badge(string name) : base(name) { }

		public abstract void Draw(SKCanvas canvas, SKRect rect);

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

		public override void Draw(SKCanvas canvas, SKRect rect) { }
	}
}
