using System;
using System.Reflection;
using Xamarin.Forms;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Runtime;


namespace Dwares.Druid.Support
{
	public class BindingScope : PropertyNotifier
	{
		public static BindingScope AppScope {
			get => Application.Current?.BindingContext as BindingScope;
		}

		public BindingScope(BindingScope parentScope)
		{
			ParentScope = parentScope;
		}

		public string MethodNameFormat { get; set; } = "On{0}";
		public BindingScope ParentScope { get; }

		string title = string.Empty;
		public string Title {
			get { return title; }
			set { SetProperty(ref title, value); }
		}

		public object ExecuteWrit(string writ)
		{
			return InvokeWrit(writ, new object[0], new Type[0], out var invoked);
		}

		public object ExecuteWrit(string writ, object parameter)
		{
			var result = InvokeWrit(writ, new object[] { parameter }, new Type[] { typeof(object) }, out var invoked);

			if (!invoked && parameter == null) {
				result = InvokeWrit(writ, new object[0], new Type[0], out invoked);
			}

			return result;
		}

		public object Execute(string writ, params object[] args) => ExecuteWrit(writ, args, null);

		public object ExecuteWrit(string writ, object[] args, Type[] argTypes = null)
		{
			if (argTypes == null) {
				argTypes = Reflection.GetArgumentTypes(args);
			}
			return InvokeWrit(writ, args, argTypes, out var invoked);
		}

		protected object InvokeWrit(string writ, object[] args, Type[] argTypes, out bool invoked)
		{
			var methodName = String.Format(MethodNameFormat, writ);
			var method = Reflection.GetMethod(this, methodName, argTypes, null, required: false);
			if (method != null) {
				invoked = true;
				return method.Invoke(this, args);
			} else if (ParentScope != null) {
				return ParentScope.InvokeWrit(writ, args, argTypes, out invoked);
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
			object page = Navigator.ContentPage;
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
