using System.IO;
using CUE4Parse.BNS;
using CUE4Parse.BNS.Conversion;
using Serilog;
using SkiaSharp;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Views.Selector;

namespace Xylia.Preview.UI.Helpers.Output.Textures;
public abstract class IconOutBase : IDisposable
{
	#region Constructor
	private readonly string _gameDirectory;
	private readonly string _outputDirectory;
	private readonly char[] _invalidChars = Path.GetInvalidFileNameChars();
	protected readonly ILogger logger;

	protected BnsDatabase? database;
	protected GameFileProvider? provider;

	public IconOutBase(string GameFolder, string OutputFolder)
	{
		// path
		_gameDirectory = GameFolder;
		_outputDirectory = OutputFolder;

		// log
		string template = "{Timestamp:yyyy-MM-dd HH:mm:ss} {Message:lj}{NewLine}";
		string folder = Path.GetDirectoryName(_outputDirectory) + "\\Log";

		logger = new LoggerConfiguration()
			.WriteTo.Logger(lc => lc
				.WriteTo.File(Path.Combine(folder, $"{DateTime.Now:yyyyMMdd}.log"), outputTemplate: template))
			.CreateLogger();
	}
	#endregion


	#region Methods
	public void Initialize(CancellationToken cancellationToken)
	{
		logger.Information("init game data");
		provider = new GameFileProvider(_gameDirectory, true);
		cancellationToken.ThrowIfCancellationRequested();

		database = new BnsDatabase(DefaultProvider.Load(_gameDirectory, DatSelectDialog.Instance));
		_ = database.Provider.GetTable<IconTexture>();
		cancellationToken.ThrowIfCancellationRequested();
	}

	public void Execute(string format, Action<float> progress, CancellationToken cancellationToken)
	{
		logger.Information($"start {GetType().Name}...");
		Directory.CreateDirectory(_outputDirectory);

		cancellationToken.ThrowIfCancellationRequested();
		Execute(format, new Progress<float>(progress), cancellationToken);
	}

	protected abstract void Execute(string format, IProgress<float> progress, CancellationToken cancellationToken);

	protected void Save(SKBitmap? source, string name)
	{
		if (source is null) return;

		// Invalid chars
		if (name.IndexOfAny(_invalidChars) >= 0)
		{
			foreach (char c in _invalidChars)
				name = name.Replace(c.ToString(), "_");
		}

		source.Save(_outputDirectory + $@"\{name}.png");
		source.Dispose();
	}
	#endregion

	#region Dispose
	public void Dispose()
	{
		database?.Dispose();
		database = null;
		provider?.Dispose();
		provider = null;

		GC.SuppressFinalize(this);
		GC.Collect();
	}
	#endregion
}

internal class ProgressHelper(IProgress<float> progress, int total)
{
	public readonly IProgress<float> Progress = progress;
	public readonly float Total = total;
	public int Current;

	public void Update()
	{
		Progress?.Report(++Current / Total);
	}
}