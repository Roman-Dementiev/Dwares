//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Dwares.Dwarf.Toolkit;


//namespace Dwares.Dwarf.Data
//{
//	public class DataBase<TCredentials> : Implicit<IDataBase> where TCredentials : ICredentials
//	{
//		public DataBase() : this(DataProvider.Instance) { }

//		public DataBase(TCredentials credentials) : this(DataProvider.Instance, credentials) { }

//		public DataBase(IDataProvider provider, TCredentials credentials = default(TCredentials))
//		{
//			Guard.ArgumentNotNull(provider, nameof(provider));
			
//			Db = provider.GetDataBase(credentials);
//			Credentials = credentials;
//		}

//		public DataBase(IDataBase database, TCredentials credentials)
//		{
//			Guard.ArgumentNotNull(database, nameof(database));

//			Db = database;
//			Credentials = credentials;
//		}

//		public IDataBase Db {
//			get => Value;
//			set => Value = value;
//		}
//		public object BaseId => Db.BaseId;
//		public TCredentials Credentials { get; }

//		public virtual Task Initialize()
//		{
//			return Task.CompletedTask;
//		}

//		public IDataTable GetTable<TRecord>(object tableId) where TRecord : IDataRecord
//			=> Db.GetTable(tableId);
//	}

//	public class DataBase : DataBase<ICredentials>
//	{
//		public DataBase() { }
//		public DataBase(ICredentials credentials) : base(credentials) { }
//		public DataBase(IDataBase database, ICredentials credentials) : base(database, credentials) { }
//	}
//}
