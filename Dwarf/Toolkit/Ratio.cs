using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Toolkit
{
	public interface IRatio
	{
		bool IsValid { get; }
		double Value { get; }
		double Antecedent { get; }
		double Consequent { get; }
	}

	public interface IRatio<T> : IRatio
	{
		//bool IsValid { get; }
		//double Value { get; }
		new T Antecedent { get; set; }
		new T Consequent { get; set; }
	}

	public struct Ratio<T> : IRatio<T> where T: IConvertible
	{
		public Ratio(T antecedent, T consequent)
		{
			Debug.Assert(Convert.ToDouble(antecedent) >= 0 && Convert.ToDouble(consequent) >= 0);
			Antecedent = antecedent;
			Consequent = consequent;
		}

		public T Antecedent { get; set; }
		public T Consequent { get; set; }
		double IRatio.Antecedent => Convert.ToDouble(Antecedent);
		double IRatio.Consequent => Convert.ToDouble(Consequent);

		public bool IsValid {
			get => Convert.ToDouble(Antecedent) >= 0 && Convert.ToDouble(Consequent) >= 0;
		}

		public double Value {
			get => Convert.ToDouble(Antecedent) / Convert.ToDouble(Consequent);
		}
	}

	public struct Rational : IRatio<int>
	{
		public static readonly Rational _0x0 = new Rational(0, 0);
		public static readonly Rational _1x1 = new Rational(1, 1);
		public static readonly Rational _1x2 = new Rational(1, 2);
		public static readonly Rational _2x1 = new Rational(2, 1);
		public static Rational None => _0x0;
		//public static Ratio Equal => _1x1;
		public static Rational Square => _1x1;

		public Rational(int antecedent, int consequent)
		{
			Debug.Assert(antecedent >= 0 && consequent >= 0);
			Antecedent = antecedent;
			Consequent = consequent;
		}

		public bool IsValid {
			get => Antecedent > 0 && Consequent > 0;
		}

		public double Value {
			get => (double)Antecedent / (double)Consequent;
		}

		public int Antecedent { get; set; }
		public int Consequent { get; set; }
		double IRatio.Antecedent => Antecedent;
		double IRatio.Consequent => Consequent;
	}
}
