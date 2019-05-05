﻿using System;
using System.Threading;
using Xamarin.Forms;


namespace Farest
{
	public interface ISettings
	{
		decimal GetDecimal(string key, decimal defaultValue);
		void SetDecimal(string key, decimal value);
	}

	public abstract class SettingsImplementation : ISettings
	{
		public abstract bool TryGet(string key, out object value);
		public abstract bool TrySet(string key, object value);

		public decimal GetDecimal(string key, decimal defaultValue)
		{
			object value;
			if (TryGet(key, out value)) {
				if (value is decimal result)
					return result;
				if (value is IConvertible convertible)
					return Convert.ToDecimal(convertible);
			}

			return defaultValue;
		}

		public void SetDecimal(string key, decimal value)
		{
			TrySet(key, value);
		}

	}

	public class Settings
	{
		static ISettings instance;
		static ISettings Instance {
			get => LazyInitializer.EnsureInitialized(ref instance, () => DependencyService.Get<ISettings>());
		}

		public static decimal GetDecimal(string key, decimal defaultValue = default(decimal))
			=> Instance.GetDecimal(key, defaultValue);

		public static void SetDecimal(string key, decimal value)
			=> Instance.SetDecimal(key, value);

		public static decimal Flagfall {
			get => GetDecimal("Flagfall", 7M);
			set => SetDecimal("Flagfall", value);
		}

		public static decimal MilesRate {
			get => GetDecimal("MilesRate", 0.595M);
			set => SetDecimal("MilesRate", value);
		}

		public static decimal MinutesRate {
			get => GetDecimal("MinutesRate", 0.255M);
			set => SetDecimal("MinutesRate", value);
		}
	}
}