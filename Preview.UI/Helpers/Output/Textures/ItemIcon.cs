using CUE4Parse.BNS.Conversion;
using CUE4Parse.FileProvider;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse_Conversion.Textures;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Models;
using static Xylia.Preview.Data.Models.Item.Grocery;

namespace Xylia.Preview.UI.Helpers.Output.Textures;
public sealed class ItemIcon(string GameFolder, string OutputFolder) : IconOutBase(GameFolder, OutputFolder)
{
    #region Properties
    public bool UseBackground { get; set; } = false;

    public bool IsWhiteList { get; set; }= false;

    public string? ChvPath { get; set; } = null;
    #endregion


    #region Methods
    protected override void Output(DefaultFileProvider provider, string format, CancellationToken token)
    {
        var lst = new HashList(ChvPath);
        var Weapon_Lock_04 = provider.LoadObject<UTexture>("BNSR/Content/Art/UI/GameUI_BNSR/Resource/GameUI_Icon3_R/Weapon_Lock_04")?.Decode();

        Parallel.ForEach(db!.Provider.GetTable("Item"), (record) =>
        {
            token.ThrowIfCancellationRequested();

            #region Data
            var key = record.PrimaryKey;
            if (lst.CheckFailed(key, IsWhiteList)) return;

            var alias = record.Attributes.Get<string>("alias");
            var grade = record.Attributes.Get<sbyte>("item-grade");
            var icon = record.Attributes["icon"]?.ToString();

            var name2 = record.Attributes.Get<Record>("name2")?.Attributes["text"]?.ToString();
            var GroceryType = record.SubclassType == 2 ? record.Attributes["grocery-type"]?.ToEnum<GroceryTypeSeq>() : null;

            record.Dispose();
            #endregion


            try
            {
                #region process
                var bitmap = IconTexture.Parse(icon, db, provider)?.Image ??
                    throw new Exception($"get resouce failed ({icon})");

                if (UseBackground)
                {
                    bitmap = IconTexture.GetBackground(grade, provider)?.Image.Compose(bitmap);

                    if (GroceryType == GroceryTypeSeq.Sealed) bitmap = bitmap.Compose(Weapon_Lock_04);
                }
                #endregion

                #region tags
                //var ProfileCopyright = bitmap.GetPropertyItem(0xc6fe);
                //ProfileCopyright.Value = Encoding.UTF8.GetBytes("blade & soul");
                //bitmap.SetPropertyItem(ProfileCopyright);
                #endregion

                var path = format.Replace("[alias]", alias).Replace("[id]", key.Id.ToString()).Replace("[name]", name2);
                Save(bitmap, path);
            }
            catch (Exception ee)
            {
                logger.Error($"{key} [{name2}]  " + ee.Message);
            }
        });
    }
    #endregion
}