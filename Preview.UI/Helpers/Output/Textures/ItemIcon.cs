using CUE4Parse.BNS.Conversion;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse_Conversion.Textures;
using SkiaSharp;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Models;
using static Xylia.Preview.Data.Models.Item.Grocery;

namespace Xylia.Preview.UI.Helpers.Output.Textures;
public sealed class ItemIcon(string GameFolder, string OutputFolder) : IconOutBase(GameFolder, OutputFolder)
{
	#region Properties
	public bool UseBackground { get; set; } = false;

	public bool IsWhiteList { get; set; } = false;

	public string? HashesPath { get; set; } = null;
	#endregion

	#region Methods
	protected override void Execute(string format, IProgress<float> progress, CancellationToken token)
	{
		var lst = new HashList(HashesPath);
		var Weapon_Lock_04 = provider!.LoadObject<UTexture>("BNSR/Content/Art/UI/GameUI_BNSR/Resource/GameUI_Icon3_R/Weapon_Lock_04")?.Decode();

		var source = database!.Provider.GetTable(nameof(Item));
		var counter = new ProgressHelper(progress, source.Count());

		Parallel.ForEach(source, record =>
		{
			token.ThrowIfCancellationRequested();
			counter.Update();

			try
			{
				#region Data
				var key = record.PrimaryKey;
				if (lst.CheckFailed(key, IsWhiteList)) return;

				var alias = record.Attributes.Get<string>("alias");
				var grade = record.Attributes.Get<sbyte>("item-grade");
				var icon = record.Attributes.Get<Icon>("icon");
				var name2 = record.Attributes.Get<Record>("name2")?.Attributes["text"]?.ToString();
				var GroceryType = record.Attributes.Get<GroceryTypeSeq>("grocery-type");
				#endregion

				#region Compose
				SKBitmap? bitmap = icon?.GetImage(provider) ?? throw new Exception($"Get resouce failed ({icon})");

				if (UseBackground)
				{
					bitmap = IconTexture.GetBackground(grade, provider)?.Image.Compose(bitmap);

					if (GroceryType == GroceryTypeSeq.Sealed) bitmap = bitmap.Compose(Weapon_Lock_04);
				}
				#endregion

				Save(bitmap, format
					.Replace("[alias]", alias)
					.Replace("[id]", key.Id.ToString())
					.Replace("[name]", name2));
			}
			catch (Exception ex)
			{
				logger.Error(string.Format("[{0}] {1}", record, ex.Message));
			}
			finally
			{
				record.Dispose();
			}
		});
	}
	#endregion
}