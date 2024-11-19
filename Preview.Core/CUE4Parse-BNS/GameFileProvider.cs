using CUE4Parse.BNS.AssetRegistry;
using CUE4Parse.BNS.Plugins;
using CUE4Parse.Compression;
using CUE4Parse.Encryption.Aes;
using CUE4Parse.FileProvider;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Versions;
using Xylia.Preview.Data.Engine.DatData;

namespace CUE4Parse.BNS;
public sealed class GameFileProvider : DefaultFileProvider, IDisposable
{
	#region Constructors
	internal const string _aesKey = "0xd2e5f7f94e625efe2726b5360c1039ce7cb9abb760a94f37bb15a6dc08741656";
	public FAssetRegistryState AssetRegistryModule { get; private set; }

	static GameFileProvider()
	{
		CompressionHelper.InitZlib();
		CompressionHelper.InitOodle();

		// register game custom class
		ObjectTypeRegistry.RegisterEngine(typeof(GameFileProvider).Assembly);
	}

	public GameFileProvider(string GameDirectory, bool LoadOnInit = false) : base(
		GameDirectory, SearchOption.AllDirectories, true,
		new() { Game = EGame.GAME_BladeAndSoul })
	{
		this.Initialize();
		this.SubmitKey(new FGuid(), new FAesKey(_aesKey));

		// load asset registry
		if (LoadOnInit) LoadAssetRegistry();
	}
	#endregion

	#region Override Methods
	private UE3PackagePlugin UE3PackagePlugin;

	public override bool TryFindGameFile(string path, out GameFile file)
	{
		var uassetPath = base.FixPath(path);

		// localization asset path
		var local = Locale.Current;
		if (Files.TryGetValue(uassetPath.Replace("bnsr/content/", $"bnsr/content/local/{local.Publisher}/{local.Language}/package/", StringComparison.OrdinalIgnoreCase), out file)) return true;
		if (Files.TryGetValue(uassetPath.Replace("bnsr/content/", $"bnsr/content/local/{local.AdditionalPublisher}/{local.Language}/package/", StringComparison.OrdinalIgnoreCase), out file)) return true;

		return base.TryFindGameFile(path, out file);
	}

	public new string FixPath(string path)
	{
		if (string.IsNullOrEmpty(path)) return null;
		else if (!path.Contains('.') && !path.Contains('/')) return path;
		else
		{
			UE3PackagePlugin ??= new UE3PackagePlugin(this);
			return FixPath(UE3PackagePlugin.Redirect(path), StringComparison.OrdinalIgnoreCase); 
		}
	}
	#endregion

	#region LoadObject Methods
	public override T LoadObject<T>(string objectPath) => Task.Run(() => LoadObjectAsync<T>(objectPath)).Result;

	public override UObject LoadObject(string objectPath) => Task.Run(() => LoadObjectAsync(objectPath)).Result;

	public override async Task<T> LoadObjectAsync<T>(string objectPath) => (await LoadObjectAsync(objectPath)) as T;

	public override async Task<UObject> LoadObjectAsync(string objectPath)
	{
		try
		{
			return await base.LoadObjectAsync(FixPath(objectPath));
		}
		catch
		{
			return null;
		}
	}

	internal FAssetRegistryState LoadAssetRegistry()
	{
		if (AssetRegistryModule is null)
		{
			if (!TryCreateReader("BNSR/AssetRegistry.bin", out var archive))
				throw new FileNotFoundException();

			var dt = DateTime.Now;
			AssetRegistryModule = new FAssetRegistryState(archive);
			archive.Dispose();

			Console.WriteLine($"Initialize asset registry, taked {(DateTime.Now - dt).Seconds}s");
		}

		return AssetRegistryModule;
	}
	#endregion

	#region Interface
	void IDisposable.Dispose()
	{
		AssetRegistryModule?.ObjectRef.Clear();

		// GC issue on CUE4
		(Files as FileProviderDictionary)?.Clear();
		base.Dispose();
	}
	#endregion
}