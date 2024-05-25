using CUE4Parse.FileProvider;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Helpers.Output.Textures;
public sealed class SkillIcon(string GameFolder, string OutputFolder) : IconOutBase(GameFolder, OutputFolder)
{
    protected override void Output(DefaultFileProvider provider, string format, CancellationToken cancellationToken)
    {
        Parallel.ForEach(db!.Provider.GetTable<Skill3>(), record =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                // name
                var key = record.PrimaryKey;
                var alias = record.Attributes.Get<string>("alias");
                var name2 = record.Name2.GetText();
                var path = format.Replace("[alias]", alias).Replace("[id]", key.ToString()).Replace("[name]", name2);

                // image
                var bitmap = IconTexture.Parse(record.IconTexture, record.IconIndex, db, provider)?.Image;
                Save(bitmap, path);
            }
            catch (Exception ee)
            {
                logger.Error($"{record} " + ee.Message);
            }
        });
    }
}