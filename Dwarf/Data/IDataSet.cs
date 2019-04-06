//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;


//namespace Dwares.Dwarf.Data
//{

//	public struct DataQueryResult
//	{
//		public IEnumerable<IDataRecord> Records { get; set; }
//		public int Count { get; set; }
//		public object NextToken { get; set; }
//	}

//	public interface IDataSet
//	{
//		Task<IDataRecord> GetRecord(object recordId);
//		Task<ICollection<IDataRecord>> GetRecords();
//		Task<DataQueryResult> QueryRecords(object queryToken);
//		Task<DataQueryResult> QueryFormula(IQueryFormula formula);
//	}
//}
