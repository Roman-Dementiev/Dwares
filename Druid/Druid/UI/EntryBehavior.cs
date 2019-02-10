using System;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.UI
{
	public abstract class EntryBehavior : Behavior<Entry>
	{
		//static ClassRef @class = new ClassRef(typeof(EntryBehavior));

		public EntryBehavior()
		{
			//Debug.EnableTracing(@class);
		}

		protected override void OnAttachedTo(Entry entry)
		{
			base.OnAttachedTo(entry);
			entry.TextChanged += Entry_TextChanged;
		}

		protected override void OnDetachingFrom(Entry entry)
		{
			entry.TextChanged -= Entry_TextChanged;
		}

		private void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
			OnTextChanged(sender as Entry, e.NewTextValue, e.OldTextValue);
		}

		protected abstract void OnTextChanged(Entry entry, string newTextValue, string oldTextValue);
	}
}
