using System;
using Windows.Devices.Sensors;
using Windows.Graphics.Display;
using Xamarin.Forms;
using Dwares.Druid.Services;


[assembly: Dependency(typeof(Dwares.Druid.UWP.DeviceOrientationSvc))]
namespace Dwares.Druid.UWP
{
	public class DeviceOrientationSvc : IDeviceOrientationSvc
	{
		//public static readonly ClassUnit @class = new ClassUnit(typeof(DeviceOrientationSvc), Druid_UWP.@namespace);

		public DeviceOrientationSvc()
		{
			var displayInformation = DisplayInformation.GetForCurrentView();
			displayInformation.OrientationChanged += DisplayInformation_OrientationChanged;
		}

		private void DisplayInformation_OrientationChanged(DisplayInformation sender, object args)
		{
			//Debug.Trace(@class, nameof(DisplayInformation_OrientationChanged));
		}

		public DeviceOrientations CurrentOrientation {
			get {
				var displayOrientation = DisplayInformation.GetForCurrentView().CurrentOrientation;
				return ToDeviceOrientations(displayOrientation);
			}
		}

		public DeviceOrientations PrefferedOrientation
		{ 
			get => ToDeviceOrientations(DisplayInformation.AutoRotationPreferences);
			set {
				switch (value)
				{
				case DeviceOrientations.Landscape:
					DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape;
					break;
				case DeviceOrientations.Portrait:
					DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
					break;
				default:
					DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
					break;
				}
			}
		}

		static DeviceOrientations ToDeviceOrientations(DisplayOrientations displayOrientation)
		{
			switch (displayOrientation)
			{
			case DisplayOrientations.Landscape:
			case DisplayOrientations.LandscapeFlipped:
				return DeviceOrientations.Landscape;

			case DisplayOrientations.Portrait:
			case DisplayOrientations.PortraitFlipped:
				return DeviceOrientations.Portrait;

			default:
				return DeviceOrientations.Undefined;
			}

		}

		//public static void NotifyOrientationChange(Windows.Graphics.Display.OrientationChangedEvent e)
		//{
		//	bool isLandscape = (e.Orientation & PageOrientation.Landscape) == PageOrientation.Landscape;
		//	var msg = new DeviceOrientationChangeMessage() {
		//		Orientation = isLandscape ? DeviceOrientations.Landscape : DeviceOrientations.Portrait
		//	};
		//	MessagingCenter.Send<DeviceOrientationChangeMessage>(msg, DeviceOrientationChangeMessage.MessageId);
		//}
	}
}
