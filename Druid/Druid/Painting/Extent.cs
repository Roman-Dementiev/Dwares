using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Dwares.Druid.Painting
{
	public struct Extent<T>: IDim where T: IConvertible
	{
		public Extent(T uniformSize)
		{
			Left = Top = Right = Bottom = uniformSize;
		}

		public Extent(T horizontalSize, T verticalSize)
		{
			Left = Right = horizontalSize;
			Top = Bottom = verticalSize;
		}

		public Extent(T left, T top, T right, T bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public T Left { get; set; }
		public T Top { get; set; }
		public T Right { get; set; }
		public T Bottom { get; set; }

		public double Width => Convert.ToDouble(Left) + Convert.ToDouble(Right);
		public double Height => Convert.ToDouble(Top) + Convert.ToDouble(Bottom);
		
		//public override bool Equals(object obj);
		//public override int GetHashCode();
		//public static bool operator ==(Thickness left, Thickness right);
		//public static bool operator !=(Thickness left, Thickness right);

		public static implicit operator Thickness(Extent<T> extent) => new Thickness(
			left: Convert.ToDouble(extent.Left),
			top: Convert.ToDouble(extent.Top),
			right: Convert.ToDouble(extent.Right),
			bottom: Convert.ToDouble(extent.Bottom)
		);

		public static implicit operator Extent<T>(Thickness thickness) => new Extent<T>(
			left: Conv(thickness.Left),
			top: Conv(thickness.Top),
			right: Conv(thickness.Right),
			bottom: Conv(thickness.Bottom)
		);

		public static implicit operator Extent<T>(T uniformSize) => new Extent<T>(uniformSize); 

		public static T Conv<TValue>(TValue value) => (T)Convert.ChangeType(value, typeof(T));
	}
}
