using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Internals;


namespace Dwares.Druid.Support
{
	public interface IDevice
	{
		string Platform { get; }
		TargetIdiom Idiom { get; }
		//DeviceInfo Info { get; }

		Size PixelScreenSize { get; }
		Size ScaledScreenSize { get; }
		double ScalingFactor { get; }
		DeviceOrientation DefaultOrientation { get; }
		DeviceOrientation CurrentOrientation { get; set; }
	}

	public static class DeviceEx
	{
		static RealDevice realDevice = null;
		public static IDevice RealDevice => LazyInitializer.EnsureInitialized(ref realDevice);

		static IDevice instance = null;
		public static IDevice Instance { 
			get {
				if (instance == null) {
					instance = RealDevice;
				}
				return instance;
			}
			set {
				instance = value;
			}
		}

		public static string Platform => Instance.Platform;
		public static bool Is_iOS => Platform == Device.iOS;
		public static bool Is_UWP => Platform == Device.UWP;
		public static bool Is_Android => Platform == Device.Android;

		public static TargetIdiom Idiom => Instance.Idiom;
		public static bool IsPhone = Idiom == TargetIdiom.Phone;
		public static bool IsTablet = Idiom == TargetIdiom.Tablet;
		public static bool IsDesktop = Idiom == TargetIdiom.Desktop;
		public static bool IsTV = Idiom == TargetIdiom.TV;

		//public static DeviceInfo Info => Instance.Info;
		public static Size PixelScreenSize => Instance.PixelScreenSize;
		public static Size ScaledScreenSize => Instance.ScaledScreenSize;
		public static double ScalingFactor => Instance.ScalingFactor;
		public static DeviceOrientation DefaultOrientation => Instance.DefaultOrientation;
		public static DeviceOrientation CurrentOrientation {
			get => Instance.CurrentOrientation;
			set => Instance.CurrentOrientation = value;
		}


		static List<MockDevice> mocks = null;

		public static void AddMock(MockDevice mock)
		{
			if (mocks == null)
				mocks = new List<MockDevice>();

			mocks.Add(mock);
		}

		public static IDevice GetMock(string name)
		{
			if (mocks != null) {
				foreach (var mock in mocks) {
					if (mock.Name == name)
						return mock;
				}
			}
			return null;
		}

		public static bool SelectMock(string name)
		{
			var mock = GetMock(name);
			if (mock == null)
				return false;

			Instance = mock;
			return true;
		}
	}


	internal class RealDevice : IDevice
	{
		public string Platform => Xamarin.Forms.Device.RuntimePlatform;
		public TargetIdiom Idiom => Xamarin.Forms.Device.Idiom;
		//public DeviceInfo Info => Xamarin.Forms.Device.Info;
		public Size PixelScreenSize => Xamarin.Forms.Device.Info.PixelScreenSize;
		public Size ScaledScreenSize => Xamarin.Forms.Device.Info.ScaledScreenSize;
		public double ScalingFactor => Xamarin.Forms.Device.Info.ScalingFactor;
		public DeviceOrientation DefaultOrientation => Xamarin.Forms.Device.Info.CurrentOrientation;
		public DeviceOrientation CurrentOrientation {
			get => Xamarin.Forms.Device.Info.CurrentOrientation;
			set => Xamarin.Forms.Device.Info.CurrentOrientation = value;
		}
	}

	public class MockDevice : IDevice
	{
		public MockDevice(string name, string platform, TargetIdiom idiom, Size screenSize, DeviceOrientation orientation = DeviceOrientation.Portrait)
		{
			Name = name;
			Platform = platform;
			Idiom = idiom;
			PixelScreenSize = ScaledScreenSize = screenSize;
			ScalingFactor = 1;
			DefaultOrientation = CurrentOrientation = orientation;
		}

		public string Name { get; set; }

		public string Platform { get; set; }
		public TargetIdiom Idiom { get; set; }
		//public DeviceInfo Info { get; set; }
		public Size PixelScreenSize { get; set; }
		public Size ScaledScreenSize { get; set; }
		public double ScalingFactor { get; set; }
		public DeviceOrientation DefaultOrientation { get; set; }
		public DeviceOrientation CurrentOrientation { get; set; }
	}
}
