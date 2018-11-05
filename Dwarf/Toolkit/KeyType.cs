using System;


namespace Dwares.Dwarf.Toolkit
{
	public class KeyType<TKey>
	{
		protected KeyType() { }

		protected KeyType(TKey key)
		{
			Key = key;
		}

		public TKey Key { get; protected set; }

		public static T Create<T>(TKey key) where T : KeyType<TKey>, new()
		{
			return new T { Key = key };
		}
	}

	public class KeyType : KeyType<string>
	{
		protected KeyType() { }
	}


	//public class SampleType : KeyType
	//{
	//	public static readonly SampleType Sampel1 = Create<SampleType>("sample1");
	//	public static readonly SampleType Sampel2 = Create<SampleType>("sample2");
	//	public static readonly SampleType Sampel3 = Create<SampleType>("sample3");
	//}

	//public class SampleIntType : KeyType<int>
	//{
	//	public static readonly SampleIntType Sampel1 = Create<SampleIntType>(1);
	//	public static readonly SampleIntType Sampel2 = Create<SampleIntType>(2);
	//	public static readonly SampleIntType Sampel3 = Create<SampleIntType>(3);
	//}
}
