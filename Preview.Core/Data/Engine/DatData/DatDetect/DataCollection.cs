namespace Xylia.Preview.Data.Engine.DatData;
public class DataCollection
{
	#region Constructor
	private readonly Dictionary<DatType, List<FileInfo>> DataPathMenu = [];

	public DataCollection(string folder) => Init(folder);
	#endregion

	#region Methods
	private void Init(string folder)
	{
		List<FileInfo> files = [];

		var DirInfo = new DirectoryInfo(folder);
		files.AddRange(DirInfo.GetFiles("*.dat", SearchOption.AllDirectories));
		files.AddRange(DirInfo.GetFiles("*.bin", SearchOption.AllDirectories));

		foreach (var file in files)
		{
			DatType datType;
			switch (file.Name.ToLower())
			{
				case "xml.dat":
				case "xml64.dat":
				case "datafile.bin":
				case "datafile64.bin":
					datType = DatType.Xml;
					break;

				case "config.dat":
				case "config64.dat":
					datType = DatType.Config;
					break;

				case "local.dat":
				case "local64.dat":
				case "localfile.bin":
				case "localfile64.bin":
					datType = DatType.Local;
					break;

				default: continue;
			}

			//add
			if (!DataPathMenu.ContainsKey(datType))
				DataPathMenu.Add(datType, []);

			DataPathMenu[datType].Add(file);
		}
	}

	public List<FileInfo> GetFiles(DatType type, ResultMode mode)
	{
		var result = new List<FileInfo>();
		if (type == DatType.Xml && DataPathMenu.TryGetValue(DatType.Xml, out var fs)) result.AddRange(fs);
		else if (type == DatType.Local && DataPathMenu.TryGetValue(DatType.Local, out fs)) result.AddRange(fs);
		else if (type == DatType.Config && DataPathMenu.TryGetValue(DatType.Config, out fs)) result.AddRange(fs);

		// *
		if (mode == ResultMode.SelectBin) return result.Where(r => r.Extension == ".bin").ToList();
		if (mode == ResultMode.SelectDat) return result.Where(r => r.Extension == ".dat").ToList();

		return result;
	}
	#endregion
}


public enum DatType
{
	Local,
	Xml,
	Config,
}

public enum ResultMode
{
	All,
	SelectBin,
	SelectDat,
}