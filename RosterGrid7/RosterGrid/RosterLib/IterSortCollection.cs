
using System.Collections;

namespace EricUtility.Iterators
{
	/// <summary>
	/// Iterate a collection in sorted order, either using the built-in ordering
	/// for the object or using a class implementing IComparer.
	/// </summary>
	public class IterSortCollection: IterIsolate, IEnumerable
	{
		internal class IterSortEnumerator: IterIsolateEnumerator, IEnumerator
		{
			internal IterSortEnumerator(IEnumerator enumerator, IComparer comparer): base(enumerator)
			{
				if (comparer != null)
				{
					items.Sort(comparer);
				}
				else
				{
					items.Sort();
				}
			}
		}

		/// <summary>
		/// Create an instance of the IterIsolate Class
		/// </summary>
		/// <param name="enumerable">A class that implements IEnumerable</param>
		public IterSortCollection(IEnumerable enumerable): base(enumerable)
		{
		}

		/// <summary>
		/// Create an instance of the IterIsolate Class, using a different sort order
		/// </summary>
		/// <param name="enumerable">A class that implements IEnumerable</param>
		/// <param name="comparer">A class that implements IComparer</param>
		public IterSortCollection(IEnumerable enumerable, IComparer comparer): base(enumerable)
		{
			this.comparer = comparer;
		}

		public new IEnumerator GetEnumerator()
		{
			return new IterSortEnumerator(enumerable.GetEnumerator(), comparer);
		}
		IComparer comparer;
	}

//	public class IterSortTest
//	{
//		public static void Main()
//		{
//			Console.WriteLine();
//			Console.WriteLine("Testing IterSortCollection");
//
//			Hashtable hash = new Hashtable();
//			hash.Add("Dog", 45);
//			hash.Add("Cat", 45);
//			hash.Add("Aardvark", 3);
//			hash.Add("Whale", 0);
//
//				// old way
//            string[] keys = new string[hash.Count];
//			hash.Keys.CopyTo(keys, 0);
//			Array.Sort(keys);
//			foreach (string s in keys)
//			{
//				Console.WriteLine("{0} = {1}", s, hash[s]);
//			}
//
//			foreach (string s in new IterSortCollection(hash.Keys))
//			{
//				Console.WriteLine("{0} = {1}", s, hash[s]);
//			}
//		}
//	}
}
