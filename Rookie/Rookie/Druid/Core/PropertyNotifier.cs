﻿//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;


//namespace Dwares.Rookie.Druid
//{
//	public class PropertyNotifier : INotifyPropertyChanged
//	{
//		public event PropertyChangedEventHandler PropertyChanged;

//		protected void FirePropertyChanged([CallerMemberName] string propertyName = "")
//		{
//			if (PropertyChanged != null) {
//				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
//			}
//		}

//		protected void PropertiesChanged(params string[] names)
//		{
//			var changed = PropertyChanged;
//			if (changed != null) {
//				foreach (var propertyName in names) {
//					changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
//				}
//			}
//		}

//		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
//		{
//			//if (EqualityComparer<T>.Default.Equals(oldValue,, value))
//			//	return false;
//			if (Object.Equals(storage, value))
//				return false;

//			storage = value;
//			FirePropertyChanged(propertyName);
//			return true;
//		}

//		protected bool SetProperty<T>(IValueHolder<T> storage, T value, [CallerMemberName]string propertyName = "")
//		{
//			//if (Object.Equals(storage.Value, value))
//			//	return false;
//			if (EqualityComparer<T>.Default.Equals(storage.Value, value))
//				return false;

//			storage.Value = value;
//			FirePropertyChanged(propertyName);
//			return true;
//		}

//		protected bool SetPropertyEx<T>(ref T storage, T value, params string[] changedProperties)
//		{
//			//if (EqualityComparer<T>.Default.Equals(oldValue,, value))
//			//	return false;
//			if (Object.Equals(storage, value))
//				return false;

//			storage = value;
//			PropertiesChanged(changedProperties);
//			return true;
//		}

//		protected bool SetPropertyEx<T>(IValueHolder<T> storage, T value, params string[] changedProperties)
//		{
//			//if (Object.Equals(storage.Value, value))
//			//	return false;
//			if (EqualityComparer<T>.Default.Equals(storage.Value, value))
//				return false;

//			storage.Value = value;
//			PropertiesChanged(changedProperties);
//			return true;
//		}

//	}
//}
