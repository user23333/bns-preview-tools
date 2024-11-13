using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class Text : ModelElement, IHaveName
{
	#region Constructors
	public Text()
	{

	}

	public Text(string alias, string text)
	{
		this.alias = alias;
		this.text = text;
	}
	#endregion

	#region Attributes
	public string alias { get; set; }

	public string text { get; set; }
	#endregion

	#region Methods
	string IHaveName.Name => this.text;

	/// <summary>
	/// Convert XML text to record
	/// </summary>
	/// <param name="xml"></param>
	/// <returns></returns>
	public static Text Parse(string xml)
	{
		try
		{
			var element = XElement.Parse(xml, LoadOptions.PreserveWhitespace);
			var reader = element.CreateReader();
			reader.MoveToContent();

			var alias = element.Attribute("alias")?.Value;
			var text = reader.ReadInnerXml();

			return new Text(alias, text);
		}
		catch
		{
			return null;
		}
	}
	#endregion
}

/// <summary>
/// Represents a weakly typed list of objects that can be accessed by index.
/// </summary>
public sealed class TextArguments : IEnumerable, ICollection, IList
{
	#region Event
	public event EventHandler Changed;
	#endregion

	#region Properties
	// Gets and sets the capacity of this list.  The capacity is the size of
	// the internal array used to hold items.  When set, the internal
	// array of the list is reallocated to the given capacity.
	public int Capacity
	{
		get => _items.Length;
		set
		{
			ArgumentOutOfRangeException.ThrowIfLessThan(value, _size);

			if (value != _items.Length)
			{
				if (value > 0)
				{
					var newItems = new object[value];
					if (_size > 0)
					{
						Array.Copy(_items, newItems, _size);
					}
					_items = newItems;
				}
				else
				{
					_items = [];
				}
			}
		}
	}

	// Read-only property describing how many elements are in the List.
	public int Count => _size;

	bool IList.IsFixedSize => false;

	bool IList.IsReadOnly => false;

	// Is this List synchronized (thread-safe)?
	bool ICollection.IsSynchronized => false;

	// Synchronization root for this object.
	object ICollection.SyncRoot => this;

	/// <summary>
	/// Gets or sets the element at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get or set.</param>
	/// <returns></returns>
	public object this[int index]
	{
		get => index >= _size ? default : _items[index];
		set
		{
			if (index >= _size)
			{
				if (index >= _items.Length) Grow(index + 1);

				_size = index + 1;
			}

			_items[index] = value;
			Changed?.Invoke(this, EventArgs.Empty);
		}
	}
	#endregion


	#region Methods

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Add(object item)
	{
		var array = _items;
		int size = _size;
		if ((uint)size < (uint)array.Length)
		{
			_size = size + 1;
			array[size] = item;
		}
		else
		{
			AddWithResize(item);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddWithResize(object item)
	{
		Debug.Assert(_size == _items.Length);
		int size = _size;
		Grow(size + 1);
		_size = size + 1;
		_items[size] = item;
	}

	// Adds the elements of the given collection to the end of this list. If
	// required, the capacity of the list is increased to twice the previous
	// capacity or the new size, whichever is larger.
	//
	public void AddRange(IEnumerable<object> collection)
	{
		if (collection is ICollection<object> c)
		{
			int count = c.Count;
			if (count > 0)
			{
				if (_items.Length - _size < count)
				{
					Grow(checked(_size + count));
				}

				c.CopyTo(_items, _size);
				_size += count;
			}
		}
		else
		{
			using IEnumerator<object> en = collection.GetEnumerator();
			while (en.MoveNext())
			{
				Add(en.Current);
			}
		}
	}

	/// <summary>
	/// Increase the capacity of this list to at least the specified <paramref name="capacity"/>.
	/// </summary>
	/// <param name="capacity">The minimum capacity to ensure.</param>
	internal void Grow(int capacity)
	{
		Capacity = GetNewCapacity(capacity);
	}

	/// <summary>
	/// Enlarge this list so it may contain at least <paramref name="insertionCount"/> more elements
	/// And copy data to their after-insertion positions.
	/// This method is specifically for insertion, as it avoids 1 extra array copy.
	/// You should only call this method when Count + insertionCount > Capacity.
	/// </summary>
	/// <param name="indexToInsert">Index of the first insertion.</param>
	/// <param name="insertionCount">How many elements will be inserted.</param>
	private void GrowForInsertion(int indexToInsert, int insertionCount = 1)
	{
		Debug.Assert(insertionCount > 0);

		int requiredCapacity = checked(_size + insertionCount);
		int newCapacity = GetNewCapacity(requiredCapacity);

		// Inline and adapt logic from set_Capacity

		var newItems = new object[newCapacity];
		if (indexToInsert != 0)
		{
			Array.Copy(_items, newItems, length: indexToInsert);
		}

		if (_size != indexToInsert)
		{
			Array.Copy(_items, indexToInsert, newItems, indexToInsert + insertionCount, _size - indexToInsert);
		}

		_items = newItems;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private int GetNewCapacity(int capacity)
	{
		Debug.Assert(_items.Length < capacity);

		int newCapacity = _items.Length == 0 ? DefaultCapacity : 2 * _items.Length;

		// Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
		// Note that this check works even when _items.Length overflowed thanks to the (uint) cast
		if ((uint)newCapacity > Array.MaxLength) newCapacity = Array.MaxLength;

		// If the computed capacity is still less than specified, set to the original argument.
		// Capacities exceeding Array.MaxLength will be surfaced as OutOfMemoryException by Array.Resize.
		if (newCapacity < capacity) newCapacity = capacity;

		return newCapacity;
	}
	#endregion

	#region Interface
	public IEnumerator GetEnumerator() => _items.Take(_size).GetEnumerator();

	public void CopyTo(Array array, int index)
	{
		// Delegate rest of error checking to Array.Copy.
		Array.Copy(_items, 0, array, index, _size);
	}

	int IList.Add(object item)
	{
		Add(item!);
		return Count - 1;
	}

	public void Clear()
	{
		int size = _size;
		_size = 0;
		if (size > 0)
		{
			Array.Clear(_items, 0, size); // Clear the elements so that the gc can reclaim the references.
		}
	}

	public bool Contains(object item)
	{
		return _size != 0 && IndexOf(item) >= 0;
	}

	public int IndexOf(object item)
	{
		return Array.IndexOf(_items, item, 0, _size);
	}

	public void Insert(int index, object item)
	{
		// Note that insertions at the end are legal.
		if ((uint)index > (uint)_size)
		{
			throw new ArgumentOutOfRangeException(nameof(index));
		}
		if (_size == _items.Length)
		{
			GrowForInsertion(index, 1);
		}
		else if (index < _size)
		{
			Array.Copy(_items, index, _items, index + 1, _size - index);
		}
		_items[index] = item;
		_size++;
	}

	public void Remove(object item)
	{
		int index = IndexOf(item);
		if (index >= 0) RemoveAt(index);
	}

	public void RemoveAt(int index)
	{
		if ((uint)index >= (uint)_size)
		{
			throw new ArgumentOutOfRangeException(nameof(index));
		}
		_size--;
		if (index < _size)
		{
			Array.Copy(_items, index + 1, _items, index, _size - index);
		}
	}
	#endregion

	#region Private Fields
	private const int DefaultCapacity = 4;

	internal object[] _items = new object[DefaultCapacity];
	internal int _size;
	#endregion
}


public static class TextExtension
{
	public static string GetText(this object obj, IDataProvider provider = null)
	{
		if (obj is null) return null;
		else if (obj is IHaveName instance) return instance.Name;
		else if (obj is Enum sequence) return SequenceExtensions.GetText(sequence);
		else if (obj is Record record && record.OwnerName == "text") return record.Attributes.Get<string>("text");
		else if (obj is string alias) return (provider ?? Globals.GameData.Provider)?[alias];
		else throw new NotSupportedException();
	}

	public static string GetText(this object obj, TextArguments arguments) => GetText(obj).Replace(arguments);

	public static string Replace(this string source, TextArguments arguments)
	{
		if (source is null) return null;

		foreach (Match m in new Regex("<arg.*?/>").Matches(source).Cast<Match>())
		{
			var doc = new HtmlDocument();
			string html = m.Value;
			doc.LoadHtml(html);

			// element
			var obj = ((Arg)doc.DocumentNode.FirstChild).GetObject(arguments);
			source = source.Replace(html, obj switch
			{
				Enum seq => SequenceExtensions.GetText(seq),
				ImageProperty image => image.Tag,
				_ => obj?.ToString(),
			});
		}

		return source;
	}
}