using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Toolkit
{
	public static class UnitConverter
	{
		const double KilometersPerMile = 1.60934;
		const double MilesPerKilometer = 1 / KilometersPerMile;

		public static double MilesToKilometers(double miles) => miles * KilometersPerMile;
		public static double KilometersToMiles(double kilometers) => kilometers * MilesPerKilometer;
	}
}
