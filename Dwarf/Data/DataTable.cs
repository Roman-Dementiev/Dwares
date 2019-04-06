//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Dwares.Dwarf.Toolkit;


//namespace Dwares.Dwarf.Data
//{
//	public class DataTable : Implicit<IDataTable>
//	{
//		//static ClassRef @class = new ClassRef(typeof(DataTable));

//		public DataTable(IDataBase database, object tableId)
//		{
//			//Debug.EnableTracing(@class);
//			Guard.ArgumentNotNull(database, nameof(database));

//			Value = database.GetTable(tableId);
//		}

//		public DataTable(IDataTable table)
//		{
//			//Debug.EnableTracing(@clIDataTable<TRecord>ass);
//			Guard.ArgumentNotNull(table, nameof(table));

//			Value = table;
//		}

//		//public IDataTable Table { get; }
//		public object TableId => Value.TableId;
//		public object Name => Value.Name;

//		public virtual Task Initialize()
//		{
//			return Task.CompletedTask;
//		}

//		public virtual async Task<Exception> Probe(bool throwErrors = true)
//		{
//			try {
//				await Value.Probe();
//				return null;
//			}
//			catch (Exception exc) {
//				if (throwErrors)
//					throw exc;
//				return exc;
//			}
//		}

//		public Task<IDataRecord> GetRecord(object recordId)
//			=> Value.GetRecord(recordId);

//		public Task<ICollection<IDataRecord>> GetRecords()
//			=> Value.GetRecords();

//		public Task<DataQueryResult> QueryRecords(object queryToken)
//			=> Value.QueryRecords(queryToken);

//		public Task<DataQueryResult> QueryFormula(IQueryFormula formula)
//			=> Value.QueryFormula(formula);

//		public Task<IDataRecord> CreateRecord(IDataRecord record, IEnumerable<string> fieldNames = null)
//			=> Value.CreateRecord(record, fieldNames);

//		public Task<IDataRecord> UpdateRecord(IDataRecord record, IEnumerable<string> fieldNames = null)
//			=> Value.UpdateRecord(record, fieldNames);

//		public Task<bool> DeleteRecord(object recordId)
//			=> Value.DeleteRecord(recordId);
//	}


//	public class DataTable<TRecord> : DataTable where TRecord : DataRecord, new()
//	{
//		public DataTable(IDataBase database, object tableId) : base(database, tableId) { }

//		public new async Task<TRecord> GetRecord(object recordId)
//		{
//			var result = await base.GetRecord(recordId);
//			return new TRecord {
//				Value = result
//			};
//		}

//		public async Task<TRecord> CreateRecord(TRecord record, params string[] fieldNames)
//		{
//			var result = await base.CreateRecord(record.Value, fieldNames);
//			return new TRecord {
//				Value = result
//			};
//		}
//		public async Task<TRecord> UpdateRecord(TRecord record, params string[] fieldNames)
//		{
//			var result = await base.UpdateRecord(record.Value, fieldNames);
//			return new TRecord {
//				Value = result
//			};
//		}

//		public static TRecord[] QueryResultRecords(IEnumerable<IDataRecord> records, int count)
//		{
//			if (records == null)
//				return null;

//			if (records is TRecord[] _records)
//				return _records;

//			var array = new TRecord[count];
//			int index = 0;
//			foreach (var record in records) {
//				array[index++] = new TRecord { Value = record };
//			}
//			return array;
//		}

//		public static TRecord[] QueryResultRecords(ICollection<IDataRecord> records) => QueryResultRecords(records, records.Count);
//		public static TRecord[] QueryResultRecords(DataQueryResult result) => QueryResultRecords(result.Records, result.Count);

//		public async Task<TRecord[]> ListRecords(object queryToken = null)
//		{
//			//var list = await List(queryBuilder);
//			//return list.Records;

//			if (queryToken == null) {
//				var records = await GetRecords();
//				return QueryResultRecords(records);
//			}
//			else {
//				var result = await QueryRecords(queryToken);
//				return QueryResultRecords(result);
//			}
//		}

//		public async Task<TRecord[]> FilterRecords(IQueryFormula formula)
//		{
//			//var list = await List(queryBuilder);
//			//return list.Records;

//			if (formula == null) {
//				var records = await GetRecords();
//				return QueryResultRecords(records);
//			}
//			else {
//				var result = await QueryFormula(formula);
//				return QueryResultRecords(result);
//			}
//		}

//		public async Task CopyRecords(DataTable<TRecord> destTable)
//		{
//			var list = await ListRecords();

//			foreach (var record in list) {
//				await destTable.CreateRecord(record);
//			}
//		}
//	}
//}
