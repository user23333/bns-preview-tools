using CUE4Parse.BNS.AssetRegistry;
using CUE4Parse.Encryption.Aes;
using CUE4Parse.FileProvider;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.FileProvider.Vfs;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Versions;
using CUE4Parse.Utils;
using Xylia.Preview.Data.Engine.DatData;

namespace CUE4Parse.BNS;
public sealed class GameFileProvider : DefaultFileProvider, IDisposable
{
    #region Constructors
    internal const string _aesKey = "0xd2e5f7f94e625efe2726b5360c1039ce7cb9abb760a94f37bb15a6dc08741656";
    public FAssetRegistryState AssetRegistryModule { get; private set; }

    static GameFileProvider()
    {
        // register game custom class
        ObjectTypeRegistry.RegisterEngine(typeof(GameFileProvider).Assembly);
    }

    public GameFileProvider(string GameDirectory, bool LoadOnInit = false) : base(
        GameDirectory, SearchOption.AllDirectories, true,
        new() { Game = EGame.GAME_BladeAndSoul })
    {
        this.Initialize();
        this.SubmitKey(new FGuid(), new FAesKey(_aesKey));
        // this.LoadLocalization(ELanguage.Korean);

        // load asset registry
        if (LoadOnInit) LoadAssetRegistry();
    }
    #endregion

    #region Override Methods
    public override bool TryFindGameFile(string path, out GameFile file)
    {
        var uassetPath = base.FixPath(path);

        // localization asset path
        var local = Locale.Current;
        if (Files.TryGetValue(uassetPath.Replace("bnsr/content/", $"bnsr/content/local/{local.Publisher}/{local.Language}/package/", StringComparison.OrdinalIgnoreCase), out file)) return true;
        if (Files.TryGetValue(uassetPath, out file)) return true;

        return base.TryFindGameFile(path, out file);
    }

    public new string FixPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        else if (path.Contains('.') && !path.Contains('/'))
        {
            // common replace rule
            // actually associated through the AssetRegistry, but loading is time-consuming
            string Ue4Path;
            if (path.StartsWith("00008758")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_Icon/" + path[9..];
            else if (path.StartsWith("00021326")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_Icon2nd/" + path[9..];
            else if (path.StartsWith("00052219")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_Icon3rd/" + path[9..];
            else if (path.StartsWith("00078990")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_Icon4th/" + path[9..];
            else if (path.StartsWith("00008130")) Ue4Path = "/Game/Art/UI/GameUI_BNSR/Resource/GameUI_FontSet_R/" + path[9..];
            else if (path.StartsWith("00009076")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_Window/" + path[9..];
            else if (path.StartsWith("00009499")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_Map_Indicator/" + path[9..];
            else if (path.StartsWith("00010047")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_ImageSet_R/" + path[9..];
            else if (path.StartsWith("00015590")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_Tag/" + path[9..];
            else if (path.StartsWith("00027869")) Ue4Path = "/Game/Art/FX/01_Source/05_SF/FXUI_03/" + path[9..];
            else if (path.StartsWith("00027918")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_Portrait/" + path[9..];
            else if (path.StartsWith("00033689")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_KeyKap/" + path[9..];
            else if (path.StartsWith("00043230")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_SkillBookImage/" + path[9..];
            else if (path.StartsWith("00064443")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_FishIcon/" + path[9..];
            else if (path.StartsWith("00079972")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_CollectionCard2D/" + path[9..];
            else if (path.StartsWith("00079973")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_CollectionCard3D/" + path[9..];
            else if (path.StartsWith("00080271")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_CollectionCard3D2nd/" + path[9..];
            else if (path.StartsWith("00080646")) Ue4Path = "/Game/Art/UI/GameUI/Resource/GameUI_CollectionCard3D3rd/" + path[9..];
            else if (path.StartsWith("MiniMap_", StringComparison.OrdinalIgnoreCase)) Ue4Path = "/Game/bns/Package/World/GameDesign/commonpackage/" + path;
            else
            {
                lock (this) if (AssetRegistryModule is null) LoadAssetRegistry();

                return AssetRegistryModule.ObjectRef.TryGetValue(path, out path) ? FixPath(path) : null;
            }

            Ue4Path = FixPath(Ue4Path.Replace('.', '/'));
            return string.Concat(Ue4Path, ".", Ue4Path.Split('/')[^1]);
        }
        // path = path;

        return FixPath(path, StringComparison.OrdinalIgnoreCase).SubstringBeforeLast(".uasset");
    }
    #endregion

    #region LoadObject Methods
    public override UObject LoadObject(string objectPath) => Task.Run(() => LoadObjectAsync(objectPath)).Result;

    public override T LoadObject<T>(string objectPath) => Task.Run(() => LoadObjectAsync<T>(objectPath)).Result;

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

    public override async Task<T> LoadObjectAsync<T>(string objectPath)
    {
        return (await LoadObjectAsync(objectPath)) as T;
    }

    private void LoadAssetRegistry()
    {
        if (!TryCreateReader("BNSR/AssetRegistry.bin", out var archive))
            throw new FileNotFoundException();

        var dt = DateTime.Now;
        AssetRegistryModule = new FAssetRegistryState(archive);
        archive.Dispose();

        Console.WriteLine($"Initialize asset registry, taked {(DateTime.Now - dt).Seconds}s");
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