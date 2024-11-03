using System.Diagnostics;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Common.Interactivity;
internal class PreviewWorld : RecordCommand
{
	protected override List<string> Type => ["terrain"];

	protected override void Execute(Record record)
	{
		if (record.OwnerName == "terrain")
		{
			Execute("bnsr/content/neo_art/area/" + record.Attributes.Get<string>("umap-name"));
		}
	}

	public static void Execute(string umap)
	{
		var viewer = PreviewModel.SnooperViewer;

		var World = FileCache.Provider.LoadPackage(umap).GetExports().OfType<UWorld>().First();
		var PersistentLevel = World.PersistentLevel.Load<ULevel>();

		foreach (var index in World.StreamingLevels)
		{
			var StreamingLevel = index.Load();
			var WorldAsset = StreamingLevel.Get<FSoftObjectPath>("WorldAsset").Load<UWorld>();
			StreamingLevel.TryGetValue(out FColor LevelColor, "LevelColor");

			Debug.WriteLine(WorldAsset.GetPathName());
			if (viewer.TryLoadExport(default, WorldAsset)) viewer.Run();
		}
	}
}