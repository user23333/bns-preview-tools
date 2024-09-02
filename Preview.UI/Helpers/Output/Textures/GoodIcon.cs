using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Helpers.Output.Textures;
public sealed class GoodIcon(string GameFolder, string OutputFolder) : IconOutBase(GameFolder, OutputFolder)
{
	protected override void Execute(string? format, IProgress<float> progress, CancellationToken token)
	{
		var source = database!.Provider.GetTable<GoodsIcon>();
		var counter = new ProgressHelper(progress, source.Count());

		Parallel.ForEach(source, record =>
		{
			token.ThrowIfCancellationRequested();
			counter.Update();

			var bitmap = record.Icon?.GetImage(provider);
			Save(bitmap, record.PrimaryKey.ToString());
		});
	}
}