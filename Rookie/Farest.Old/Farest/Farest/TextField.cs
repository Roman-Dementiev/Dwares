using System;
using Xamarin.Forms;


namespace Farest
{
	public class TextField<T>
	{
		public TextField(Func<T, bool> validate) : this(string.Empty, validate) { }

		public TextField(T value, Func<T, bool> validate) : this(value?.ToString(), validate) { }

		public TextField(string text, Func<T, bool> validate)
		{
			Validate = validate;
			SetText(text);
		}


		public string Text { get; private set; }
		public T Value { get; private set; }
		public bool IsValid { get; private set; }
		public Func<T, bool> Validate { get; }
		//public Color Color { get; private set; } = ViewModelBase.ValidValueColor;

		public bool SetText(string text)
		{
			if (text == Text)
				return false;

			T value = default(T);
			if (string.IsNullOrEmpty(text)) {
				IsValid = true;
			}
			else if (ViewModelBase.TrtGetValue(text, out value)) {
				IsValid = true;
			}
			else {
				IsValid = false;
			}

			if (IsValid && Validate != null) {
				IsValid = Validate(value);
			}

			Text = text;
			Value = value;
			//Color = IsValid ? ViewModelBase.ValidValueColor : ViewModelBase.InvalidValueColor;
			return true;
		}

	}
}
