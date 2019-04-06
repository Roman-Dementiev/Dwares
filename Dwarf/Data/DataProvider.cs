//using System;
//using Dwares.Dwarf.Toolkit;


//namespace Dwares.Dwarf.Data
//{
//	public class DataProvider<TCredentials> : Implicit<IDataProvider>
//		where TCredentials : ICredentials
//	{
//		public static IDataProvider Instance { get; set; }

//		public DataProvider(IDataProvider provider) : this(provider, Instance == null) { }

//		public DataProvider(IDataProvider provider, bool setAsInstance) :
//			base(provider)
//		{
//			Guard.ArgumentNotNull(provider, nameof(provider));

//			if (setAsInstance) {
//				Instance = provider;
//			}
//		}

//		public IDataBase GetDataBase(TCredentials credentials) 
//			=> Value.GetDataBase(credentials);
//	}

//	public class DataProvider : DataProvider<ICredentials>
//	{
//		public DataProvider(IDataProvider provider) : base(provider) { }
//		public DataProvider(IDataProvider provider, bool setAsInstance) : base(provider, setAsInstance) { }

//	}
//}
