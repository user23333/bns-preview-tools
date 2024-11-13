using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Material;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.UI.FModel.Views;
public class ModelData
{
	public string? DisplayName;
	public UObject? Export;
	public UAnimSet? AnimSet;
	public List<UMaterialInstance>? Materials;

	public IEnumerable<ObjectPath> Cols
	{
		set
		{
			if (value is null) return;
			Materials = [];

			// Splits string array into substrings based on a specified delimiting character
			foreach (var material in value.Where(o => o.IsValid)
				.SelectMany(o => o.Path.Split(',')))
			{
				var export = Globals.GameProvider.LoadObject(material);
				if (export is UMaterialInstance unrealMaterial)
					Materials.Add(unrealMaterial);
			}
		}
	}
}