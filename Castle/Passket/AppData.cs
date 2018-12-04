using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid.Services;
using Passket.Models;
using Passket.Storage;

namespace Passket
{
	public class AppData
	{
		//static ClassRef @class = new ClassRef(typeof(AppData));

		static AppData instance;
		public static AppData Instance {
			get => LazyInitializer.EnsureInitialized(ref instance);
		}

#if DEBUG
		const StorageLocation cLocation = StorageLocation.Pictures;
#else
		const StorageLocation cLocation = StorageLocation.AppData;
#endif
		const string cFilename = "Passket.json";

		AppStorage storage;
		uint lastId = 0;

		Dictionary<uint, IEntity> directory = new Dictionary<uint, IEntity>();
		ObservableCollection<Record> records = new ObservableCollection<Record>();
		ObservableCollection<Group> groups = new ObservableCollection<Group>();
		ObservableCollection<Pattern> patterns = new ObservableCollection<Pattern>();

		public AppData()
		{
			//Debug.EnableTracing(@class);
		}

		public async void Initialize()
		{
			try {
				var folder = await DeviceStorage.GetFolder(cLocation);
				bool exists = await folder.FileExistsAsync(cFilename);
				if (exists) {
					var file = await folder.GetFileAsync(cFilename);
					storage = new JsonStorage(file);
					await storage.Load();
				} else {
					var file = await folder.CreateFileAsync(cFilename, true);
					storage = new JsonStorage(file);
				}
			} catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}

			var accountPattern = new Pattern() { Icon = "account", Name = "Account" };
			accountPattern.Fields.Add(new Pattern.Field { Name = "Account URL", Kind = EntryKind.Uri });
			accountPattern.Fields.Add(new Pattern.Field { Name = "Username", Kind = EntryKind.String });
			accountPattern.Fields.Add(new Pattern.Field { Name = "Password", Kind = EntryKind.Password });
			//accountPattern.Fields.Add(new Pattern.Field { Name = "Hint 1", Kind = EntryKind.Hint });
			//accountPattern.Fields.Add(new Pattern.Field { Name = "Hint 2", Kind = EntryKind.Hint });
			accountPattern.Fields.Add(new Pattern.Field { Name = "Notes", Kind = EntryKind.Text });

			AddPattern(accountPattern);

			var accounts = new Group { Icon = "account", Name = "Web Accounts" };
			var finances = new Group { Icon = "finances", Name = "Finances" };
			var codes = new Group { Icon = "code", Name = "Codes" };
			var notes = new Group { Icon = "note", Name = "Notes" };
			AddGroups(accounts, finances, codes, notes);

			AddRecord(Factory.NewRecord(accountPattern), accounts);
			AddRecord(Factory.NewRecord(accountPattern), accounts);
			AddRecord(Factory.NewRecord(accountPattern), accounts);

			await Save();
		}

		public async Task Save()
		{
			try {
				await storage.Save();
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
		}

		public Record GetRecordByName(string name)
		{
			foreach (var record in records) {
				if (record.Name == name)
					return record;
			}
			return null;
		}

		void AddToDirectory(IEntity entity)
		{
			Debug.AssertNotNull(entity);

			bool assignId = true;
			if (entity.Id > 0) {
				if (directory.ContainsKey(entity.Id)) {
					Debug.Print("Duplicate entity Id: {0}", entity.Id);
				} else {
					if (entity.Id > lastId) {
						lastId = entity.Id;
					}
					assignId = false;
				}
			}

			if (assignId) {
				entity.Id = ++lastId;
			}
			directory.Add(entity.Id, entity);
		}

		IEntity GetEntity(uint id)
		{
			if (directory.ContainsKey(id)) {
				return directory[id];
			} else {
				return null;
			}
		}


		public void AddPattern(Pattern pattern)
		{
			AddToDirectory(pattern);
			patterns.Add(pattern);

			storage.PatternAdded(pattern);
		}

		public void AddPatterns(params Pattern[] list)
		{
			foreach (var pattern in list) {
				AddPattern(pattern);
			}
		}

		public void AddGroup(Group group)
		{
			AddToDirectory(group);
			groups.Add(group);

			storage.GroupAdded(group);
		}

		public void AddGroups(params Group[] list)
		{
			foreach (var group in list) {
				AddGroup(group);
			}
		}

		public void AddRecord(Record record, params Group[] groups)
		{
			AddToDirectory(record);
			records.Add(record);

			foreach (var group in groups) {
				group.Add(record);
			}

			storage.RecordAdded(record);
		}

		public static ObservableCollection<Record> Records => Instance.records;
		public static ObservableCollection<Group> Groups => Instance.groups;
		public static ObservableCollection<Pattern> Patterns => Instance.patterns;

		public static Record GetRecord(uint id) => Instance.GetEntity(id) as Record;
		public static Record GetRecord(string name) => Instance.GetRecordByName(name);
		public static bool HasRecord(string name) => Instance.GetRecordByName(name) != null;

	}
}
