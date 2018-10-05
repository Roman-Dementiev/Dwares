using System;
using Xamarin.Forms;


namespace Dwares.Druid
{
	public interface IChoice
	{
		Image Image { get; }
		Image SmallImage { get; }
		Image LargeImage { get; }
		string Label { get; }
		string ShortLabel { get; }
		string LongLabel { get; }
		string Description { get; }
	}

	public class Cloice : IChoice
	{
		public Image Image {
			get => SmallImage ?? LargeImage;
			set => SmallImage = value;
		}
		public string Label {
			get => ShortLabel ?? LongLabel;
			set => ShortLabel = value;
		}

		public Image SmallImage { get; set; }
		public Image LargeImage { get; set; }
		public string ShortLabel { get; set; }
		public string LongLabel { get; set; }
		public string Description { get; set; }
	}

	//[Flags]
	public enum ChoiceAppearance
	{
		None = 0,
		SmallImage = 1,
		LargeImage = 2,
		ShortLabel = 4,
		LongLabel = 8,
		Image = SmallImage | LargeImage,
		Label = ShortLabel | LongLabel,
		SmallImageAndShortLabel = SmallImage | ShortLabel,
		SmallImageAndLongLabel = SmallImage | LongLabel,
		SmallImageAndLabel = SmallImage | Label,
		LargeImageAndShortLabel = LargeImage | ShortLabel,
		LargeImageAndLongLabel = LargeImage | LongLabel,
		LargeImageAndLabel = LargeImage | Label,
		ImageAndShortLabel = Image | ShortLabel,
		ImageAndLongLabel = Image | LongLabel,
		ImageAndLabel = Image | Label,
		Default = ImageAndLabel
	}
}
