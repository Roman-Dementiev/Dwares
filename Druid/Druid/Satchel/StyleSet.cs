//using System;
//using System.Collections.Generic;
//using System.Threading;
//using Dwares.Dwarf;
//using Dwares.Druid.UI;
//using Xamarin.Forms;


//namespace Dwares.Druid.Satchel
//{
//	public class StyleSet
//	{
//		//static ClassRef @class = new ClassRef(typeof(StyleSet));
//		Dictionary<string, StyleRec> dict = new Dictionary<string, StyleRec>();

//		public StyleSet()
//		{
//			//Debug.EnableTracing(@class);
//		}

//		public void Add(string name, string flavor)
//		{
//			dict[name] = new StyleRec(name, flavor);
//		}

//		public Style Get(string name)
//		{
//			Style style;
//			if (dict.ContainsKey(name)) {
//				var rec = dict[name];
//				style = rec.Style;
//				if (style == null) {
//					style = rec.Update();
//				}
//			} else {
//				style = Empty;
//			}
//			return style;
//		}

//		public void Update()
//		{
//			foreach (var pair in dict) {
//				pair.Value.Update();
//			}
//		}

//		class StyleRec
//		{
//			public StyleRec(string name, string flavor)
//			{
//				Name = name;
//				Flavor = flavor;
//				Style = null;
//			}

//			public string Name { get; }
//			public string Flavor { get; }
//			public Style Style { get; set; }

//			public Style Update()
//			{
//				Style = UITheme.Current.GetStyle(Flavor);
//				if (Style == null) {
//					Style = Empty;
//				}
//				return Style;
//			}
//		}

//		static Style empty;
//		static Style Empty {
//			get => LazyInitializer.EnsureInitialized(ref empty, () => {
//				return new Style(typeof(VisualElement));
//			});
//		}
//	}
//}
