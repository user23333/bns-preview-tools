using CUE4Parse.BNS;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Engine.Definitions;
using Xylia.Preview.Properties;

namespace Xylia.Preview.Data.Helpers;
public static class FileCache
{
	private static readonly object Lock = new();

	#region Data
	private static GameFileProvider _provider;
	public static GameFileProvider Provider
	{
		get { lock (Lock) { return _provider ??= new(Settings.Default.GameFolder); } }
	}

	private static DatafileDefinition _definition;
	public static DatafileDefinition Definition
	{
		get => _definition ??= CompressDatafileDefinition.Load();
		set
		{
			_definition = value;
			Settings.Default.DefitionKey = value?.Key;
		}
	}

	private static BnsDatabase _data;
	public static BnsDatabase Data
	{
		set => _data = value;
		get { lock (Lock) return _data ??= new(DefaultProvider.Load(Settings.Default.GameFolder, Globals.DatSelector), Definition); }
	}

	public static void Clear()
	{
		_definition?.Clear();
		_definition = null;
		_data?.Dispose();
		_data = null;

		_provider?.Dispose();
		_provider = null;
	}
	#endregion
}