//using System;
//using Xamarin.Forms;


//namespace Dwares.Druid.Services
//{
//	/// <summary>
//	/// Device orientations.
//	/// </summary>
//	public enum DeviceOrientations
//	{
//		/// <summary>
//		/// Undefined
//		/// </summary>
//		Undefined,

//		/// <summary>
//		/// The landscape.
//		/// </summary>
//		Landscape,

//		/// <summary>
//		/// The portrait.
//		/// </summary>
//		Portrait
//	}

//	/// <summary>
//	/// Device orientation change message.
//	/// </summary>
//	public class DeviceOrientationChangeMessage
//	{
//		/// <summary>
//		/// Gets or sets the orientation.
//		/// </summary>
//		/// <value>The orientation.</value>
//		public DeviceOrientations Orientation {
//			get;
//			set;
//		}

//		///// <summary>
//		///// Gets the message identifier.
//		///// </summary>
//		///// <value>The message identifier.</value>
//		//public static string MessageId {
//		//	get {
//		//		return "DeviceOrientationChangeMessage";
//		//	}
//		//}
//	}

//	public interface IDeviceOrientationSvc
//	{
//		DeviceOrientations CurrentOrientation { get; }
//		DeviceOrientations PrefferedOrientation { get; set; }
//	}

//	public static class DeviceOrientationSvc
//	{
//		public const string DeviceOrientationChangeMessage = nameof(DeviceOrientationChangeMessage);
//		static DependencyService<IDeviceOrientationSvc> instance;
//		public static IDeviceOrientationSvc Instance => DependencyService<IDeviceOrientationSvc>.GetInstance(ref instance);

//		public static DeviceOrientations CurrentOrientation
//		{
//			get => Instance.CurrentOrientation;
//		}

//		public static DeviceOrientations PrefferedOrientation {
//			get => Instance.PrefferedOrientation;
//			set => Instance.PrefferedOrientation = value;
//		}

//		public static void Subscribe(object subscriber, Action<IDeviceOrientationSvc, DeviceOrientationChangeMessage> callback)
//		{
//			MessagingCenter.Subscribe(subscriber, DeviceOrientationChangeMessage, callback, Instance);
//		}

//		public static void Unsubscribe(object subscriber)
//		{
//			MessagingCenter.Unsubscribe<DeviceOrientationChangeMessage>(subscriber, DeviceOrientationChangeMessage);
//		}

//		public static void OnOrientationChanged(IDeviceOrientationSvc sender)
//		{
//			var message = new DeviceOrientationChangeMessage() { Orientation = sender.CurrentOrientation };
//			MessagingCenter.Send(sender, DeviceOrientationChangeMessage, message);
//		}
//	}
//}
