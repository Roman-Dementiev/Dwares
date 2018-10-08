using System;
using System.Reflection;
using Xamarin.Forms;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Runtime;


namespace Dwares.Druid.Support
{
	public class BindingScope : NotifyPropertyChanged
	{
		public BindingScope(BindingScope parentScope)
		{
			ParentScope = parentScope;
		}

		public string MethodNameFormat { get; set; } = "On{0}";
		public BindingScope ParentScope { get; }

		public object ExecuteOrder(string order)
		{
			bool invoked;
			return InvokeOrder(order, new object[0], new Type[0], out invoked);
		}

		public object ExecuteOrder(string order, object parameter)
		{
			bool invoked;
			var result = InvokeOrder(order, new object[]{parameter}, new Type[]{typeof(object)}, out invoked);

			if (!invoked && parameter == null) {
				result = InvokeOrder(order, new object[0], new Type[0], out invoked);
			}

			return result;
		}

		public object Execute(string order, params object[] args) => ExecuteOrder(order, args, null);

		public object ExecuteOrder(string order, object[] args, Type[] argTypes = null)
		{
			if (argTypes == null) {
				argTypes = Reflection.GetArgumentTypes(args);
			}
			bool invoked;
			return InvokeOrder(order, args, argTypes, out invoked);
		}

		protected object InvokeOrder(string order, object[] args, Type[] argTypes, out bool invoked)
		{
			var methodName = String.Format(MethodNameFormat, order);
			var method = Reflection.GetMethod(this, methodName, argTypes, null, required: false);
			if (method != null) {
				invoked = true;
				return method.Invoke(this, args);
			} else if (ParentScope != null) {
				return ParentScope.InvokeOrder(order, args, argTypes, out invoked);
			} else {
				invoked = false;
				return null;
			}
		}


		public static BindingScope GetObjectScope(object obj)
		{
			while (obj != null) {
				if (obj is BindingScope scope) {
					return scope;
				}

				if (obj is BindableObject bindable) {
					if (bindable.BindingContext is BindingScope context) {
						return context;
					}
				}

				obj = (obj as Element)?.Parent;
			}

			return null;
		}

		public static BindingScope GetCurrentScope()
		{
			object page = Navigator.CurrentPage;
			if (page == null) {
				page = Application.Current.MainPage;
			}

			return GetObjectScope(page ?? Application.Current);
		}
	}

	public static partial class Extensions
	{
		public static void SetScope(this BindableObject obj, BindingScope scope)
		{
			obj.BindingContext = scope;
		}

		public static BindingScope GetScope(this BindableObject obj)
		{
			return obj.BindingContext as BindingScope;
		}

		public static TScope GetScope<TScope>(this BindableObject obj) where TScope : BindingScope
		{
			return obj.BindingContext as TScope;
		}
	}
}
