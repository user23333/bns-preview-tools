using SkiaSharp;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Helpers.Output.Textures;
public sealed class SkillIcon(string GameFolder, string OutputFolder) : IconOutBase(GameFolder, OutputFolder)
{
	protected override void Execute(string format, IProgress<float> progress, CancellationToken token)
	{
		var source = database!.Provider.GetTable(nameof(Skill3));
		var counter = new ProgressHelper(progress, source.Count());

		Parallel.ForEach(source, record =>
		{
			token.ThrowIfCancellationRequested();
			counter.Update();

			try
			{
				// data
				var key = record.PrimaryKey;
				var alias = record.Attributes.Get<string>("alias");
				var name2 = record.Attributes.Get<Record>("name2").GetText();
				var iconTexture = record.Attributes.Get<Record>("icon-texture");
				var iconIndex = record.Attributes.Get<short>("icon-index");
				var icon = record.Attributes.Get<Icon>("icon") ?? new Icon(iconTexture, iconIndex);

				// image
				SKBitmap? bitmap = icon.GetImage(provider) ?? throw new Exception($"Get resouce failed ({icon})");
				Save(bitmap, format
					.Replace("[alias]", alias)
					.Replace("[id]", key.ToString())
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
}