//using System;
//using Dwares.Druid.Services;


//namespace Dwares.Druid
//{
//	public class PermissionException : UnauthorizedAccessException
//	{
//		public const string DefaultMessageFormat = "Permission {0} was not granted: status={1}";

//		public PermissionException() { }
//		public PermissionException(string message) : base(message) { }
//		public PermissionException(string message, Exception inner) : base(message, inner) { }

//		public PermissionException(PermissionType permission, PermissionStatus status, string message = null) :
//			base(String.IsNullOrEmpty(message) ? String.Format(DefaultMessageFormat, permission, status) : message)
//		{
//			PermissionType = permission;
//			Status = status;
//		}

//		PermissionType PermissionType { get; }
//		PermissionStatus Status { get; }
//	}

//	public class FeatureNotSupportedException : InvalidOperationException
//	{
//		public const string DefaultMessageFormat = "{0} is not supported";

//		public FeatureNotSupportedException() { }
//		public FeatureNotSupportedException(string message) : base(message) { }
//		public FeatureNotSupportedException(string message, Exception innerException) : base(message, innerException) { }

//		public FeatureNotSupportedException(string feature, string message) :
//			base(String.IsNullOrEmpty(message) ? String.Format(DefaultMessageFormat, feature) : message)
//		{
//			Feature = feature;
//		}

//		public string Feature { get; }
//	}

//	public class FeatureNotEnabledException : InvalidOperationException
//	{
//		public const string DefaultMessageFormat = "{0} is not enabled";

//		public FeatureNotEnabledException() {}
//		public FeatureNotEnabledException(string message) : base(message) {}
//		public FeatureNotEnabledException(string message, Exception innerException) : base(message, innerException) {}

//		public FeatureNotEnabledException(string feature, string message) :
//			base(String.IsNullOrEmpty(message) ? String.Format(DefaultMessageFormat, feature) : message)
//		{
//			Feature = feature;
//		}

//		public string Feature { get; }
//	}
//}
