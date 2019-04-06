//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;


//namespace Dwares.Dwarf.Data
//{
//	public interface IDataTable : IDataSet
//	{
//		object TableId { get; }
//		string Name { get; }

//		Task Probe();

//		Task<IDataRecord> CreateRecord(IDataRecord record, IEnumerable<string> fieldNames);
//		Task<IDataRecord> UpdateRecord(IDataRecord record, IEnumerable<string> fieldNames);
//		//Task<IDataRecord> ReplaceRecord(IDataRecord record, IEnumerable<string> fieldNames);
//		Task<bool> DeleteRecord(object recordId);
//	}
//}
