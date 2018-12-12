using System;
using System.Collections.Generic;
using System.Threading;
using Dwares.Dwarf.Toolkit;


namespace Passket.Models
{
	public class EntryKind : KeyType
	{
		public static readonly EntryKind String = Create(nameof(String));
		public static readonly EntryKind Number = Create(nameof(Number));
		public static readonly EntryKind Integer = Create(nameof(Integer));
		public static readonly EntryKind Boolean = Create(nameof(Boolean));
		public static readonly EntryKind Password = Create(nameof(Password));
		public static readonly EntryKind Email = Create(nameof(Email));
		public static readonly EntryKind Uri = Create(nameof(Uri));
		public static readonly EntryKind Text = Create(nameof(Text));
		public static readonly EntryKind PINCode = Create(nameof(PINCode));
		public static readonly EntryKind Hint = Create(nameof(Hint));

		public static EntryKind ForKey(string key) => ForKey<EntryKind>(key, Dict);
		public static EntryKind Create(string key) => Create<EntryKind>(key, Dict);

		static Dictionary<string, EntryKind> dict;
		static Dictionary<string, EntryKind> Dict => LazyInitializer.EnsureInitialized(ref dict);
	}
}
