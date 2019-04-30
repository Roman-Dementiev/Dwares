using System;
using System.Collections.Generic;
using System.Threading;


namespace Farest
{
	public class Preset
	{
		public string Name { get; set; }
		public decimal Flagfall { get; set; }
		public decimal MilesRate { get; set; }
		public decimal MinutesRate { get; set; }

		public override string ToString() => Name ?? base.ToString();

		static List<Preset> list;
		public static List<Preset> List {
			get => LazyInitializer.EnsureInitialized(ref list, () => {
				var list = new List<Preset>();
				list.Add(new Preset { Name = "Default", Flagfall = 7M, MilesRate = 2.5M, MinutesRate = 0.2M });
				list.Add(new Preset { Name = "Distance only", Flagfall = 7M, MilesRate = 3M, MinutesRate = 0M });
				return list;
			});
		} 
	}
}
