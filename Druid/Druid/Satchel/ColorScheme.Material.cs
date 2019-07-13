using System;
using Dwares.Dwarf;
using Xamarin.Forms;

// https://material.io/design/color/the-color-system.html#


namespace Dwares.Druid.Satchel
{
	public class MaterialColorScheme : ColorScheme
	{
		//static ClassRef @class = new ClassRef(typeof(MaterialColorScheme));

		public MaterialColorScheme(string name) :
			base(name, Resources.MaterialDesign.Name)
		{
			//Debug.EnableTracing(@class);
		}

		public MaterialColorScheme(ResourceDictionary dict, string name = null) :
			base(dict, name, Resources.MaterialDesign.Name)
		{
			//Debug.EnableTracing(@class);
		}

		public override string DefaultVariant {
			get => Resources.MaterialDesign.DefaultColorVariant;
		}

		public Color Primary {
			get => this.GetColor(nameof(Primary));
		}

		public Color Secondary {
			get => this.GetColor(nameof(Secondary));
		}

		public Color PrimaryVariant {
			get => this.GetColor(nameof(PrimaryVariant));
		}

		public Color SecondaryVarianr {
			get => this.GetColor(nameof(SecondaryVarianr));
		}

		public Color Background {
			get => this.GetColor(nameof(Background));
		}

		public Color Surface {
			get => this.GetColor(nameof(Surface));
		}

		public Color Error {
			get => this.GetColor(nameof(Error));
		}

		public Color OnPrimary {
			get => this.GetColor(nameof(OnPrimary));
		}

		public Color OnSecondary {
			get => this.GetColor(nameof(OnSecondary));
		}

		public Color OnSurface {
			get => this.GetColor(nameof(OnSurface));
		}

		public Color OnError {
			get => this.GetColor(nameof(OnError));
		}
	}
}
