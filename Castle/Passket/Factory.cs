using System;
using System.Collections.Generic;
using System.Threading;
using Xamarin.Forms;
using Dwares.Dwarf;
using Passket.Models;


namespace Passket
{
	public class Factory
	{
		//static ClassRef @class = new ClassRef(typeof(Factory));

		static Factory instance;
		public static Factory Instance {
			get => LazyInitializer.EnsureInitialized(ref instance);
		}

		Dictionary<EntryKind, EntryFactory> factories;

		public Factory()
		{
			//Debug.EnableTracing(@class);
			InitFactories();
		}

		IEntry CreateEntry(string name, EntryKind kind)
		{
			if (factories.ContainsKey(kind)) {
				var func = factories[kind].CreateEntry;
				if (func != null) {
					var entry = func(kind);
					entry.Name = name;
					return entry;
				}
			}

			Debug.Fail(String.Format("Can not create entry of type {0}", kind));
			return null;
		}

		public static string AutoRecordName(string sampleName)
		{
			if (string.IsNullOrEmpty(sampleName))
				sampleName = "Record";

			for (int i = 1; ; i++) {
				string name = string.Format("{0} {1}", sampleName, i);
				if (!AppData.HasRecord(name))
					return name;
			}
		}

		public Record CreateRecord(Pattern pattern, string name)
		{
			var record = new Record() {
				Name = name ?? AutoRecordName(pattern.Name),
				Icon = pattern.Icon,
				Info = pattern.Info
			};
			
			foreach (var field in pattern.Fields) {
				var entry = CreateEntry(field.Name, field.Kind);
				record.Entries.Add(entry);
			}
			return record;
		}

		public Record CreateRecord(Record template, string name)
		{
			var record = new Record() {
				Name = name ?? AutoRecordName(template.Name),
				Icon = template.Icon,
				Info = template.Info
			};

			foreach (var templateEntry in template.Entries) {
				var entry = CreateEntry(templateEntry.Name, templateEntry.Kind);
				record.Entries.Add(entry);
			}

			return record;
		}
			
		public View CreateVew(IEntry entry, ref bool needLabel)
		{
			if (factories.ContainsKey(entry.Kind)) {
				var f = factories[entry.Kind];
				var func = f.CreateView ?? f.CreateEdit;
				if (func != null) {
					var options = f.ViewOptions ?? f.EditOptions;
					if (options != null) {
						options.ReadOnly = true;
					}
					return func(entry, options, ref needLabel);
				}
			}

			Debug.Fail(String.Format("Can not create UI for entry of type {0}", entry.Kind));
			return null;
		}

		public View CreateEdit(IEntry entry, ref bool needLabel)
		{
			if (factories.ContainsKey(entry.Kind)) {
				var f = factories[entry.Kind];
				var func = f.CreateEdit;
				if (func != null) {
					var options = f.EditOptions;
					if (options != null) {
						options.ReadOnly = false;
					}
					return func(entry, options, ref needLabel);
				}
			}

			Debug.Fail(String.Format("Can not create UI for entry of type {0}", entry.Kind));
			return null;
		}


		public static IEntry NewEntry(string name, EntryKind kind) => Instance.CreateEntry(name, kind);
		public static Record NewRecord(Pattern pattern, string name = null) => Instance.CreateRecord(pattern, name);
		public static Record NewRecord(Record template, string name = null) => Instance.CreateRecord(template, name);


		internal delegate IEntry CreateEntryDelegate(EntryKind kind);
		internal delegate View CreateUIDelegate(IEntry entry, UIOptions options, ref bool needLabel);

		struct EntryFactory
		{
			public EntryFactory(CreateEntryDelegate createEntry, CreateUIDelegate createView, CreateUIDelegate createEdit, UIOptions viewOptions = null, UIOptions editOptions = null)
			{
				CreateEntry = createEntry;
				CreateView = createView;
				CreateEdit = createEdit;
				ViewOptions = viewOptions;
				EditOptions = editOptions;
			}

			public CreateEntryDelegate CreateEntry { get; set; }
			public CreateUIDelegate CreateView { get; set; }
			public CreateUIDelegate CreateEdit { get; set; }
			public UIOptions ViewOptions { get;set; }
			public UIOptions EditOptions { get; set; }
		}

		static IEntry NewStringEntry(EntryKind kind) => new Entry<string>(kind);

		static View NewStringView(IEntry entry, object options, ref bool needLabel)
		{
			var view = new Label();
			if (entry.Value != null) {
				view.Text = entry.Value.ToString() ?? string.Empty;
			}
			return view;
		}

		static View NewStringEdit(IEntry entry, object options, ref bool needLabel)
		{
			var view = new Xamarin.Forms.Entry();
			if (entry.Value != null) {
				view.Text = entry.Value.ToString() ?? string.Empty;
			}
			return view;
		}
	
		void InitFactories()
		{
			factories = new Dictionary<EntryKind, EntryFactory> {
				{ EntryKind.String, new EntryFactory(NewStringEntry, NewStringView, NewStringEdit) },
				{ EntryKind.Password, new EntryFactory(NewStringEntry, NewStringView, NewStringEdit) },
				{ EntryKind.Email, new EntryFactory(NewStringEntry, NewStringView, NewStringEdit) },
				{ EntryKind.Uri, new EntryFactory(NewStringEntry, NewStringView, NewStringEdit) },
				{ EntryKind.Text, new EntryFactory(NewStringEntry, NewStringView, NewStringEdit) },
				{ EntryKind.PINCode, new EntryFactory(NewStringEntry, NewStringView, NewStringEdit) },
				{ EntryKind.Number, new EntryFactory(NewStringEntry, NewStringView, NewStringEdit) },
				{ EntryKind.Integer, new EntryFactory(NewStringEntry, NewStringView, NewStringEdit) },
			};
		}
	}


	public class UIOptions
	{
		public bool ReadOnly { get; set; }
		// TODO
	}
}
