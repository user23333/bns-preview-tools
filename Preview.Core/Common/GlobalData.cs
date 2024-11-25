using CUE4Parse.BNS;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Engine.Definitions;
using Xylia.Preview.Properties;

namespace Xylia.Preview.Common;
public static partial class Globals
{
	private static readonly object Lock = new();

	#region Data
	private static GameFileProvider _provider;
	public static GameFileProvider GameProvider
	{
		get { lock (Lock) { return _provider ??= new(Settings.Default.GameFolder); } }
	}

	private static BnsDatabase _data;
	public static BnsDatabase GameData
	{
		set => _data = value;
		get { lock (Lock) return _data ??= new(DefaultProvider.Load(Settings.Default.GameFolder, Globals.DatSelector), Definition); }
	}

	private static DatafileDefinition _definition;
	public static DatafileDefinition Definition
	{
		get => _definition ??= CompressDatafileDefinition.Load();
		set
		{
			if (_definition != value)
			{
				GameData?.Dispose();
				GameData = null;
			}

			_definition = value;
		}
	}

	/// <summary>
	/// Close current database
	/// </summary>
	public static void ClearData()
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