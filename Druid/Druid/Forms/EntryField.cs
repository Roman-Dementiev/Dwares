//using System;
//using Dwares.Dwarf;
//using Dwares.Dwarf.Toolkit;


//namespace Dwares.Druid.Forms
//{
//	public class EntryField : Field<string>, ITextHolder
//	{
//		//static ClassRef @class = new ClassRef(typeof(EntryField));

//		public EntryField()
//		{
//			//Debug.EnableTracing(@class);
//		}


//		public string Text {
//			get => Value;
//			set => Value = Text;
//		}
//	}

//	public class EntryField<T> : Field<T>, ITextHolder
//	{
//		//static ClassRef @class = new ClassRef(typeof(EntryField));

//		public EntryField()
//		{
//			//Debug.EnableTracing(@class);
//		}

//		string text;
//		public string Text {
//			get => text ?? Value?.ToString();
//			set {
//				if (value == text)
//					return;

//				text = value;
//				isValid = null;
//			}
//		}

//		protected override Exception Validate()
//		{
//			if (text != null) {
//				try {
//					ConvertFromText();
//				}
//				catch (Exception exc) {
//					return new ValidationError(ValidationError.InvalidType, exc);
//				}
//			}

//			return base.Validate();
//		}

//		protected virtual void ConvertFromText()
//		{
//			var value = Convert.ChangeType(text, typeof(T));
//			Value = (T)value;

//		}
//	}
//}
