using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Text;
using OfficeOpenXml;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.ViewModels;
using Xylia.Preview.UI.Views.Selector;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal sealed class ItemOut : OutSet, IDisposable
{
	#region Properties
	public bool OnlyUpdate { get; set; }
	#endregion

	#region Methods
	public int Start(FileModeDialog.FileMode mode, string? hash = null)
	{
		#region HashList
		var refs = new List<Ref>();

		if (OnlyUpdate)
		{
			HashList = new HashList(hash);
			refs.AddRange(HashList.HashMap);
		}

		refs.AddRange(Data.Select(item => item.PrimaryKey));
		#endregion

		#region Init
		var createdAt = Source!.Provider.CreatedAt;
		var outdir = Path.Combine(
			UserSettings.Default.OutputFolder, "output", "item",
			createdAt.ToString("yyyyMM"),
			createdAt.ToString("dd hhmm"));

		Directory.CreateDirectory(outdir);
		var Path_Hash = Path.Combine(outdir, $@"{createdAt:yyyy-MM-dd hh-mm}.chv");
		var Path_Failure = Path.Combine(outdir, @"no_text.txt");
		var Path_MainFile = Path.Combine(outdir, @"output." + mode.ToString().ToLower());
		#endregion

		#region Output
		switch (mode)
		{
			case FileModeDialog.FileMode.Xlsx:
			{
				var package = new ExcelPackage();
				CreateData(package);
				package.SaveAs(Path_MainFile);
				break;
			}
			case FileModeDialog.FileMode.Txt:
			{
				using var writer = new StreamWriter(new FileStream(Path_MainFile, FileMode.Create));
				CreateText(writer);
				break;
			}

			default: throw new NotSupportedException();
		}

		// extra
		new HashList(refs).Save(Path_Hash);

		var failures = Data.Where(o => o.Name2 is null);
		if (failures.Any())
		{
			using StreamWriter Out_Failure = new(Path_Failure);
			failures.OrderBy(o => o.PrimaryKey).ForEach(item => Out_Failure.WriteLine($"{item.PrimaryKey,-15} {item.Alias}"));
		}
		#endregion

		return Data.Count();
	}

	protected override void CreateData(ExcelPackage package)
	{
		var sheet = CreateSheet(package);
		int row = 1;
		int index = 1;
		sheet.SetColumn(index++, "id", 12);
		sheet.SetColumn(index++, "key", 15);
		sheet.SetColumn(index++, "name", 40);
		sheet.SetColumn(index++, "alias", 40);
		sheet.SetColumn(index++, "job", 20);
		sheet.SetColumn(index++, "desc", 80);
		sheet.SetColumn(index++, "info", 80);

		foreach (var Item in Data.OrderBy(x => x.PrimaryKey))
		{
			row++;
			int column = 1;

			sheet.Cells[row, column++].SetValue(Item.PrimaryKey);
			sheet.Cells[row, column++].SetValue(Item.Key);
			sheet.Cells[row, column++].SetValue(Item.Name2);
			sheet.Cells[row, column++].SetValue(Item.Alias);
			sheet.Cells[row, column++].SetValue(Item.Job.GetText());
			sheet.Cells[row, column++].SetValue(Item.Description);
			sheet.Cells[row, column++].SetValue(Item.Info);
		}
	}

	protected override void CreateText(TextWriter writer)
	{
		foreach (var Item in Data.OrderBy(x => x.PrimaryKey))
		{
			writer.Write("{0,-15}", Item.PrimaryKey);
			writer.Write("{0,-60}", "alias: " + Item.Alias);
			writer.Write("{0,-0}", "name: " + Item.Name2);
			writer.WriteLine();
		}
	}

	public void Dispose()
	{
		_data = null;
		HashList = null;

		GC.Collect();
	}
	#endregion

	#region Data
	private HashList? HashList = null;
	private IEnumerable<ItemSimple>? _data = null;
	private IEnumerable<ItemSimple> Data
	{
		get
		{
			if (_data is null)
			{
				var text = TextDiff.BuildPieceHashes(Source!.Provider.GetTable("text"));
				var items = new BlockingCollection<ItemSimple>();
				Parallel.ForEach(Source.Provider.GetTable("item"), record =>
				{
					if (HashList != null && HashList.CheckFailed(record.PrimaryKey)) return;

					var data = new ItemSimple(record, text);
					items.Add(data);
				});

				// final
				_data = items;
				text.Clear();

				if (items.Count == 0) throw new WarningException(StringHelper.Get("ItemList_Empty"));
			}

			return _data;
		}
	}

	class ItemSimple
	{
		#region Field
		public Ref PrimaryKey;
		public string? Alias;
		public string? Name2;
		public string? Description;
		public string? Info;
		public JobSeq Job;

		public string Key => Convert.ToBase64String([.. BitConverter.GetBytes(PrimaryKey.Id).Reverse()]);

		public ItemSimple(Record record, Dictionary<string, Text> table)
		{
			PrimaryKey = record.PrimaryKey;
			Alias = record.Attributes.Get<string>("alias");

			// attribute offset may move with update, we achieve through querying text
			Name2 = GetName2(table);
			Info = GetInfo(table);
			Description = GetDesc(table);
			Job = GetJob(Alias);
		}
		#endregion

		#region Text
		private string? GetName2(Dictionary<string, Text> table)
		{
			var text = table.GetValueOrDefault($"Item.Name2.{Alias}")?.text;

			// replace rule when error status
			if (text is null)
			{

			}

			return text is null ? null : text + GetEquipGem(Alias);
		}

		private string? GetDesc(Dictionary<string, Text> table)
		{
			string? text = null;
			text += table.GetValueOrDefault($"Item.Desc2.{Alias}")?.text;
			text += table.GetValueOrDefault($"Item.Desc5.{Alias}")?.text;

			return BNS_Cut(text);
		}

		private string? GetInfo(Dictionary<string, Text> table)
		{
			string? text = null;
			text += table.GetValueOrDefault($"Item.MainInfo.{Alias}")?.text;
			text += table.GetValueOrDefault($"Item.IdentifyMain.{Alias}")?.text;
			text += table.GetValueOrDefault($"Item.IdentifySub.{Alias}")?.text;

			return BNS_Cut(text);
		}

		public static string? BNS_Cut(string text)
		{
			if (string.IsNullOrWhiteSpace(text)) return null;

			var builder = new StringBuilder();

			var doc = new HtmlDocument();
			doc.LoadHtml(text);

			foreach (var node in doc.DocumentNode.ChildNodes)
			{
				builder.Append(node switch
				{
					HtmlTextNode textNode => textNode.InnerText,
					Image imageNode => $"[{imageNode.Imagesetpath}]",

					_ => node.InnerHtml,
				});
			}

			return builder.ToString();
		}

		public static JobSeq GetJob(string alias)
		{
			if (string.IsNullOrEmpty(alias)) return JobSeq.JobNone;
			else if (alias.Contains("RynSword", StringComparison.OrdinalIgnoreCase) || alias.Contains("SW")) return JobSeq.귀검사;
			else if (alias.Contains("GreatSword", StringComparison.OrdinalIgnoreCase) || alias.Contains("WA")) return JobSeq.투사;
			else if (alias.Contains("SoulGauntlet", StringComparison.OrdinalIgnoreCase) || alias.Contains("SF")) return JobSeq.기권사;
			else if (alias.Contains("WarDagger", StringComparison.OrdinalIgnoreCase) || alias.Contains("WL")) return JobSeq.주술사;
			else if (alias.Contains("Sword", StringComparison.OrdinalIgnoreCase) || alias.Contains("BM_")) return JobSeq.검사;
			else if (alias.Contains("Gauntlet", StringComparison.OrdinalIgnoreCase) || alias.Contains("KM")) return JobSeq.권사;
			else if (alias.Contains("Staff", StringComparison.OrdinalIgnoreCase) || alias.Contains("SU")) return JobSeq.소환사;
			else if (alias.Contains("Aura-bangle", StringComparison.OrdinalIgnoreCase) || alias.Contains("FM")) return JobSeq.기공사;
			else if (alias.Contains("Axe", StringComparison.OrdinalIgnoreCase) || alias.Contains("DE")) return JobSeq.역사;
			else if (alias.Contains("Dagger", StringComparison.OrdinalIgnoreCase) || alias.Contains("AS")) return JobSeq.암살자;
			else if (alias.Contains("Gun_", StringComparison.OrdinalIgnoreCase) || alias.Contains("PT")) return JobSeq.격사;
			else if (alias.Contains("LongBow", StringComparison.OrdinalIgnoreCase) || alias.Contains("AR")) return JobSeq.궁사;
			else if (alias.Contains("Orb", StringComparison.OrdinalIgnoreCase)) return JobSeq.뇌전술사;
			else if (alias.Contains("DualBlade", StringComparison.OrdinalIgnoreCase)) return JobSeq.쌍검사;
			else if (alias.Contains("Harp", StringComparison.OrdinalIgnoreCase)) return JobSeq.악사;
			else if (alias.Contains("Spear", StringComparison.OrdinalIgnoreCase)) return JobSeq.궁사;

			return JobSeq.JobNone;
		}

		public static string? GetEquipGem(string? alias)
		{
			if (string.IsNullOrEmpty(alias)) return null;

			if (alias.Contains("Gam1", StringComparison.OrdinalIgnoreCase)) return " ☵1";
			else if (alias.Contains("Gan2", StringComparison.OrdinalIgnoreCase)) return " ☳2";
			else if (alias.Contains("Gin3", StringComparison.OrdinalIgnoreCase)) return " ☶3";
			else if (alias.Contains("Son4", StringComparison.OrdinalIgnoreCase)) return " ☱4";
			else if (alias.Contains("Lee5", StringComparison.OrdinalIgnoreCase)) return " ☲5";
			else if (alias.Contains("Gon6", StringComparison.OrdinalIgnoreCase)) return " ☷6";
			else if (alias.Contains("Tae7", StringComparison.OrdinalIgnoreCase)) return " ☴7";
			else if (alias.Contains("Gun8", StringComparison.OrdinalIgnoreCase)) return " ☰8";
			else if (alias.Contains("EquipGem_None", StringComparison.OrdinalIgnoreCase)) return " ☰8";

			return null;
		}
		#endregion
	}
	#endregion
}