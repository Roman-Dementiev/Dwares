﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Dwares.Dwarf.Toolkit
{
	public class PropertyNotifier : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public bool IsModified { get; set; }

		protected virtual void FirePropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged != null) {
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected virtual void PropertiesChanged(IEnumerable<string> names)
		{
			var changed = PropertyChanged;
			if (changed != null)
			{
				foreach (var propertyName in names) {
					changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
				}
			}
		}

		protected void PropertiesChanged(params string[] names)
		{
			//var changed = PropertyChanged;
			//if (changed != null) {
			//	foreach (var propertyName in names) {
			//		changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
			//	}
			//}
			PropertiesChanged((IEnumerable<string>)names);
		}

		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			//if (EqualityComparer<T>.Default.Equals(oldValue,, value))
			//	return false;
			if (Object.Equals(storage, value))
				return false;

			storage = value;
			IsModified = true;
			FirePropertyChanged(propertyName);
			return true;
		}

		protected bool SetProperty<T>(IValueHolder<T> storage, T value, [CallerMemberName]string propertyName = "")
		{
			//if (Object.Equals(storage.Value, value))
			//	return false;
			if (EqualityComparer<T>.Default.Equals(storage.Value, value))
				return false;

			storage.Value = value;
			IsModified = true;
			FirePropertyChanged(propertyName);
			return true;
		}

		protected bool SetTextProperty(ITextHolder storage, string text, [CallerMemberName]string propertyName = "")
		{
			if (storage.Text == text)
				return false;

			storage.Text = text;
			IsModified = true;
			FirePropertyChanged(propertyName);
			return true;
		}

		protected bool SetPropertyEx<T>(ref T storage, T value, IEnumerable<string> changedProperties)
		{
			//if (EqualityComparer<T>.Default.Equals(oldValue,, value))
			//	return false;
			if (Object.Equals(storage, value))
				return false;

			storage = value;
			IsModified = true;
			PropertiesChanged(changedProperties);
			return true;
		}

		protected bool SetPropertyEx<T>(ref T storage, T value, params string[] changedProperties)
		{
			return SetPropertyEx(ref storage, value, (IEnumerable<string>)changedProperties);
		}

		protected bool SetPropertyEx<T>(IValueHolder<T> storage, T value, params string[] changedProperties)
		{
			//if (Object.Equals(storage.Value, value))
			//	return false;
			if (EqualityComparer<T>.Default.Equals(storage.Value, value))
				return false;

			storage.Value = value;
			IsModified = true;
			PropertiesChanged(changedProperties);
			return true;
		}

		protected bool SeTexttPropertyEx(ITextHolder storage, string text, params string[] changedProperties)
		{
			if (storage.Text == text)
				return false;

			storage.Text = text;
			IsModified = true;
			PropertiesChanged(changedProperties);
			return true;
		}

	}
}
