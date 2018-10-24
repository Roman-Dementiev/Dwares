﻿using System;
using System.Threading;
using Xamarin.Forms;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Runtime;


namespace Dwares.Druid.Support
{
	public class BindingScope : PropertyNotifier, IDescendant
	{
		public static BindingScope AppScope {
			get => Application.Current?.BindingContext as BindingScope;
		}

		public BindingScope(BindingScope parentScope)
		{
			ParentScope = parentScope;
		}

		string title = string.Empty;
		public string Title {
			get { return title; }
			set { SetProperty(ref title, value); }
		}

		public BindingScope ParentScope { get; set; }
		object IDescendant.Parent => ParentScope;

		IWritExecutor writExecutor = null;
		public IWritExecutor WritExecutor {
			get => LazyInitializer.EnsureInitialized(ref writExecutor, () => new WritExecutor(this));
			set => writExecutor = value;
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

		public virtual void UpdateCommands()
		{
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
