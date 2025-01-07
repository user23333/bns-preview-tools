using System.Diagnostics;
using CUE4Parse.BNS;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse_Conversion.Textures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xylia.Preview.Tests.Extensions;

namespace Xylia.Preview.Tests;
[TestClass]
public partial class PakTest
{
	[TestMethod]
	public void ObjectTest()
	{
		using var provider = new GameFileProvider(IniHelper.Instance.GameFolder);
		// var obj = provider.LoadObject(@"BNSR/Content/Art/FX/05_BM/EquipShow/SS_EquipShow_Wolf.SS_EquipShow_Wolf");
		var obj = provider.LoadObject(@"bnsr/content/neo_art/cinema/baekchung/labyrinth/zncs_labyrinth_cinemacontrol.zncs_labyrinth_cinemacontrol");

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
				
				break;
			}	  

			case UBnsParticleSystem ParticleSystem:
				//Trace.WriteLine(JsonConvert.SerializeObject(emitter, Formatting.Indented));
			break;

			case UWorld World:
			{
				foreach (var index in World.PersistentLevel.Load<ULevel>().Actors)
				{
					Debug.WriteLine(index.GetPathName());
				}			 
				break;
			}
				
			default:
				Console.WriteLine("debug not support: {0}" , obj?.GetFullName());	 
				break;
		}
	}
}