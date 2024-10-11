using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Material;
using Xylia.Preview.Data.Helpers;

namespace Xylia.Preview.UI.FModel.Views;
public class ModelData
{
	public string? DisplayName;
	public UObject? Export;
	public UAnimSet? AnimSet;
	public List<UMaterialInstance>? Materials;

	public IEnumerable<string?> Cols
	{
		set
		{
			Materials = [];

			// Splits string array into substrings based on a specified delimiting character
			foreach (var material in value
				.Where(o => !string.IsNullOrEmpty(o))
				.SelectMany(o => o!.Split(',')))
			{
				var export = FileCache.Provider.LoadObject(material);
				if (export is UMaterialInstance unrealMaterial)
					Materials.Add(unrealMaterial);
			}
		}
	}
}