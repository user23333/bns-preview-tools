using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Common.Extension;
public static class LinqExtensions
{
	#region IEnumerable
	public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action, bool skipNull = true) => ForEach(collection, (x, i) => action(x), skipNull);

	public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> action, bool skipNull = true)
	{
		int idx = -1;

		foreach (var item in collection)
		{
			idx++;
			if (skipNull && item is null) continue;

			action(item, idx);
		}
	}

	public static List<T> Randomize<T>(this IEnumerable<T> source)
	{
		var originalList = new List<T>(source); // Create a new list, so no operation performed here affects the original list object.
		var randomList = new List<T>();

		var r = new Random();

		int randomIndex;

		while (originalList.Count > 0)
		{
			randomIndex = r.Next(0, originalList.Count);  // Choose a random object in the list
			randomList.Add(originalList[randomIndex]); // Add it to the new, random list
			originalList.RemoveAt(randomIndex); // Remove to avoid duplicates
		}

		return randomList;
	}

	/// <summary>
	/// Projects each element of a NotNull sequence into a new form.
	/// </summary>
	/// <typeparam name="TSource">The type of the elements of source.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by selector.</typeparam>
	/// <param name="source">A sequence of values to invoke a transform function on.</param>
	/// <param name="selector"> A transform function to apply to each element.</param>
	/// <returns></returns>
	public static IEnumerable<TResult> SelectNotNull<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
	{
		return source.Select(selector).Where(x => x != null);
	}

	/// <summary>
	/// Determines whether a sequence is empty.
	/// </summary>
	/// <typeparam name="T">The type of the elements of source.</typeparam>
	/// <param name="source">The System.Collections.Generic.IEnumerable`1 to check for emptiness.</param>
	/// <returns></returns>
	public static bool IsEmpty<T>(this IEnumerable<T> source) => source == null || !source.Any();

	public static string Join(string separator, params string[] source) => Join(separator, (IEnumerable<string>)source);

	public static string Join(string separator, IEnumerable<string> source)
	{
		return string.Join(separator, source.Where(t => !string.IsNullOrWhiteSpace(t)));
	}
	#endregion

	#region Array
	public static void For<T>(ref T[] array, int size, Func<int, T> func)
	{
		array = For(size, func);
	}

	public static T[] For<T>(int size, Func<int, T> func)
	{
		var array = new T[size];
		for (int index = 0; index < size; index++)
			array[index] = func(index + 1);

		return array;
	}

	public static Tuple<T1, T2>[] Create<T1, T2>(T1[] array1, T2[] array2)
	{
		ArgumentNullException.ThrowIfNull(array1);
		ArgumentNullException.ThrowIfNull(array2);

		var source = new Tuple<T1, T2>[Math.Max(array1.Length, array2.Length)];
		for (int i = 0; i < source.Length; i++)
		{
			source[i] = new Tuple<T1, T2>(
				array1.ElementAtOrDefault(i),
				array2.ElementAtOrDefault(i));
		}

		return source;
	}
	#endregion

	#region Expand
	public static IEnumerable<T> Values<T>(this IEnumerable<Ref<T>> source) where T : ModelElement
	{
		return source.Select(x => x.Instance).Where(x => x != null);
	}
	#endregion
}