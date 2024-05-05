using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Xylia.Preview.Data.Common;
using Xylia.Preview.Data.Engine.BinData.Helpers;

namespace Xylia.Preview.Data.Engine.BinData.Models;
[JsonConverter(typeof(StringLookupConverter))]
public class StringLookup : IDisposable
{
	#region Fields
	byte[] _data = [];
	int _length;
	#endregion

	#region Properties
	internal bool IsPerTable { get; set; }

	internal byte[] Data
	{
		get => _data.AsSpan(0, _length).ToArray();
		set
		{
			_data = value;
			_length = value.Length;
		}
	}

	public string[] Strings
	{
		get => Encoding.Unicode.GetString(_data, 0, _length).Split('\0');
		//set
		//{
		//	StringBuilder _stringBuilder = new();
		//	foreach (var s in value)
		//	{
		//		_stringBuilder.Append(s);
		//		_stringBuilder.Append('\0');
		//	}

		//	Data = Encoding.Unicode.GetBytes(_stringBuilder.ToString());
		//}
	}
	#endregion

	#region Methods
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public string GetString(int offset)
	{
		if (offset >= 0 && offset < _data.Length)
			return _data.GetNStringUTF16(offset);

		return null;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public int AppendString(string str, out int size)
	{
		ArgumentNullException.ThrowIfNull(_data);
		str ??= "";

		var position = this._length;
		var strBytes = Encoding.Unicode.GetBytes(str + "\0");
		size = strBytes.Length;

		Write(strBytes);
		return position;
	}

	private void Write(byte[] buffer)
	{
		// expand array
		var length = _length + buffer.Length;
		if (length > _data.Length)
		{
			int newCapacity = Math.Max(length, 256);
			if (newCapacity < length * 2) newCapacity = length * 2;

			Array.Resize(ref _data, Math.Min(newCapacity, Array.MaxLength));
		}

		// append buffer
		Array.Copy(buffer, 0, _data, _length, buffer.Length);
		_length = length;
	}

	public void Dispose()
	{
		_data = null;

		GC.SuppressFinalize(this);
	}
	#endregion
}