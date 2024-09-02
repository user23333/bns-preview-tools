using CUE4Parse.BNS;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Engine.Definitions;
using Xylia.Preview.Properties;

namespace Xylia.Preview.Data.Helpers;
public static class FileCache
{
	private static readonly object Lock = new();

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
		get { lock (Lock) return _data ??= new(DefaultProvider.Load(Settings.Default.GameFolder), Definition); }
	}

	private static GameFileProvider _provider;
	public static GameFileProvider Provider
	{
		get { lock (Lock) { return _provider ??= new(Settings.Default.GameFolder); } }
	}

	public static ITextProvider TextProvider { get; set; }

	public static void Clear()
	{
		_definition.Clear();
		_definition = null;
		_data?.Dispose();
		_data = null;

		_provider?.Dispose();
		_provider = null;
	}
}