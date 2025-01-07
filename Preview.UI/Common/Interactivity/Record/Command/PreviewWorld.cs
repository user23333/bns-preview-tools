using System.Diagnostics;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Common.Interactivity;
internal class PreviewWorld : RecordCommand
{
	protected override List<string> Type => ["terrain"];

	protected override void Execute(Record record)
	{
		if (record.OwnerName == "terrain")
		{
			var umap = record.Attributes.Get<string>("umap-name");
			if (!Globals.GameProvider.TryFindGameFile("bnsr/content/bns/package/world/area/" + umap, out var file) &&
				!Globals.GameProvider.TryFindGameFile("bnsr/content/neo_art/area/" + umap, out file))
				throw new Exception(StringHelper.Get("Exception_InvalidTerrain"));

			Execute(file);
		}
	}

	private static void Execute(GameFile umap)
	{	
		var viewer = PreviewModel.SnooperViewer;

		var World = Globals.GameProvider.LoadPackage(umap).GetExports().OfType<UWorld>().First();
		var PersistentLevel = World.PersistentLevel.Load<ULevel>();

		foreach (var index in World.StreamingLevels)
		{
			var StreamingLevel = index.Load()!;
			var WorldAsset = StreamingLevel.Get<FSoftObjectPath>("WorldAsset").Load<UWorld>();
			StreamingLevel.TryGetValue(out FColor LevelColor, "LevelColor");

			Debug.WriteLine(WorldAsset.GetPathName());
			if (viewer.TryLoadExport(default, WorldAsset)) viewer.Run();
		}
	}
}