//#define DIM_3D
using System;
using SkiaSharp;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.Painting 
{
	public interface IDim
	{
		double Width { get; }
		double Height { get; }
#if DIM_3D
		double Depth { get; }
		bool Is3D { get; }
#endif
	}

	public interface IDim<T> where T : struct, IConvertible
	{
		T Width { get; }
		T Height { get; }
#if DIM_3D
		T Depth { get; }
		bool Is3D { get; }
#endif
	}

	public struct Dim<T>: IDim, IDim<T> where T: struct, IConvertible
	{
		public Dim(T x, T y)
		{
			X = x;
			Y = y;
#if DIM_3D
			Z = null;
#endif
		}

		public Dim(T x, T y, T z)
		{
			X = x;
			Y = y;
#if DIM_3D
			Z = z;
#endif
		}

		public T X { get; set; }
		public T Y { get; set; }
		public T Width => X;
		public T Height => Y;
		double IDim.Width => Convert.ToDouble(X);
		double IDim.Height => Convert.ToDouble(Y);

#if DIM_3D
		public T? Z { get; set; }
		public T Depth => Z ?? default(T);
		double IDim.Depth => Convert.ToDouble(Z);
		public bool Is3D => Z != null;
#endif

		public override string ToString()
		{
			return Strings.Properties(this, nameof(Width), nameof(Height));
		}

		public static T Conv<TValue>(TValue value) => (T)Convert.ChangeType(value, typeof(T));

		public static explicit operator Size(Dim<T> dim)
			=> new Size(Convert.ToDouble(dim.Width), Convert.ToDouble(dim.Height));
	
		public static explicit operator Dim<T>(Size size)
			=> new Dim<T>(Conv(size.Width), Conv(size.Height));

		public static explicit operator SKSize(Dim<T> dim)
			=> new SKSize(Convert.ToSingle(dim.Width), Convert.ToSingle(dim.Height));

		public static explicit operator Dim<T>(SKSize size)
			=> new Dim<T>(Conv(size.Width), Conv(size.Height));
	}
}
