using CUE4Parse.FileProvider;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Helpers.Output.Textures;
public sealed class GoodIcon(string GameFolder, string OutputFolder) : IconOutBase(GameFolder, OutputFolder)
{
	protected override void Output(DefaultFileProvider provider, string? format, IProgress<int> progress, CancellationToken cancellationToken)
	{
		Parallel.ForEach(db!.Provider.GetTable<GoodsIcon>(), record =>
		{
			cancellationToken.ThrowIfCancellationRequested();

			var bitmap = record.Icon?.GetImage(provider);
			Save(bitmap, record.PrimaryKey.ToString()!);
		});
	}
}