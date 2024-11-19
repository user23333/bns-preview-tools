using System.Diagnostics;
using System.IO;
using System.Text;
using CUE4Parse.BNS;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.Utils;
using CUE4Parse_Conversion.Textures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Tests.Extensions;

namespace Xylia.Preview.Tests.PakTests;
[TestClass]
public partial class AssetTest
{
	[TestMethod]
	public void ClassTest()
	{
		var data = File.ReadAllText("").ToBytes();

		foreach (var s1 in Encoding.ASCII.GetString(data).Split((char)0x00))
		{
			var t = s1.SubstringAfter("/Script/Engine.BNSEnvObject:");
			Console.WriteLine($"[UPROPERTY] public object {t};");
		}
	}

	[TestMethod]
	public void ObjectTest()
	{
		using var provider = new GameFileProvider(IniHelper.Instance.GameFolder);

		var package = provider.LoadPackage(@"BNSR\Content\Art\FX\05_BM\EquipShow/SS_EquipShow_Wolf") as Package;

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
	public void PluginTest()
	{
		using var provider = new GameFileProvider(IniHelper.Instance.GameFolder);
		Console.WriteLine(provider.FixPath("00009499.Minimap_Status_Event"));
	}
}