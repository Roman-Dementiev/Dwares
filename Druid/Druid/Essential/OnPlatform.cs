using System;
using Xamarin.Forms;


namespace Dwares.Druid.Essential
{
	public class OnPlatform<T>
	{
		public OnPlatform(T defaultValue = default(T))
		{
			Default = defaultValue;
		}

		public virtual T Default { get; set; }

		T value;
		public virtual T Value {
			get => this.value == null ? Default : value;
			set => this.value = value;
		}


		public T Android {
			get => Get(Device.Android);
			set => Set(value, Device.Android);
		}

		public T iOS {
			get => Get(Device.iOS);
			set => Set(value, Device.iOS);
		}

		public T UWP {
			get => Get(Device.UWP);
			set => Set(value, Device.UWP);
		}

		public T Get(string platform)
		{
			if (String.IsNullOrEmpty(platform) || Device.RuntimePlatform == platform) {
				return Value;
			} else {
				return default(T);
			}
		}

		void Set(T value, string platform)
		{
			if (String.IsNullOrEmpty(platform) || Device.RuntimePlatform == platform) {
				Value = value;
			}
		}

		public static implicit operator T(OnPlatform<T> onp)
		{
			return onp.Value;
		}
	}
}
