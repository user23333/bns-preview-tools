﻿using System.Diagnostics;
using System.IO;
using CUE4Parse.BNS;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Exports.BuildData;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Writers;
using CUE4Parse_Conversion.Textures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xylia.Preview.Tests.Extensions;

namespace Xylia.Preview.Tests.PakTests;
[TestClass]
public partial class AssetExport
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



		using var writer = new FArchiveWriter();
		package.Serialize(writer);

		writer.Flush();

		var path = @"F:\Resources\文档\Programming\C#\FModel2\Output\Exports\BNSR\Content\Art\FX\05_BM\EquipShow\a.uasset";
		File.WriteAllBytes(path , writer.GetBuffer());
	}

	[TestMethod]
	public void MapTest()
	{
		using var provider = new GameFileProvider(IniHelper.Instance.GameFolder);

		var umap = provider.LoadPackage("bnsr/content/neo_art/area/zncs_guildbase_p.umap");


		//var MapRegistry = provider.LoadObject<UMapBuildDataRegistry>($"/Game/bns/Package/World/Area/{name}_BuiltData");
	}
}