using System.Diagnostics;
using CUE4Parse.BNS;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse_Conversion.Textures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xylia.Preview.Tests.Extensions;

namespace Xylia.Preview.Tests.PakTests;
[TestClass]
public partial class AssetTest
{
	[TestMethod]
	public void ObjectTest()
	{
		using GameFileProvider Provider = new(IniHelper.Instance.GameFolder);

		var package = Provider.LoadPackage(@"BNSR\Content\Art\FX\05_BM\EquipShow/SS_EquipShow_Wolf") as Package;

		var obj = package.GetExport("SS_EquipShow_Wolf");
		switch (obj)
		{
			case UTexture2D texture:
				var bitmap = texture.Decode(ETexturePlatform.DesktopMobile);
				break;

			case UShowObject ShowObject:
			{
				foreach (var a in ShowObject.EventKeys)
				{
					Trace.WriteLine(JsonConvert.SerializeObject(a.Load(), Formatting.Indented));
				}
			}
			break;

			case UBnsParticleSystem ParticleSystem:
			{
				//Trace.WriteLine(JsonConvert.SerializeObject(emitter, Formatting.Indented));
			}
			break;
		}
	}

	[TestMethod]
	public void MapTest()
	{
		using var provider = new GameFileProvider(IniHelper.Instance.GameFolder);

		var umap = provider.LoadPackage("bnsr/content/neo_art/area/zncs_interserver_001_p.umap");

		var world = umap.GetExports().OfType<UWorld>().First();
		var level = world.PersistentLevel.Load<ULevel>();
		// world.StreamingLevels
	}
}