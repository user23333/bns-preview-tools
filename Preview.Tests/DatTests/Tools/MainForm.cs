using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Serilog;
using Serilog.Events;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Engine.Definitions;
using Xylia.Preview.Tests.DatTests.Tools.Utils;
using Xylia.Preview.Tests.Extensions;

namespace Xylia.Preview.Tests.DatTests.Tools;
public partial class MainForm : Form
{
	#region Constructor
	public MainForm()
	{
		InitializeComponent();

		_ = new LogHelper(richOut);
		CheckForIllegalCrossThreadCalls = false;

		ReadConfig(this);

		// register log
		string template = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message:lj}{NewLine}{Exception}";
		Log.Logger = new LoggerConfiguration()
			.WriteTo.Debug(LogEventLevel.Debug, outputTemplate: template)
			.CreateLogger();
	}
	#endregion

	#region Helpers

	private static DateTime m_LastTime = DateTime.MinValue;

	public void ReadConfig(Control container)
	{
		string SECTION = "Test";

		foreach (Control c in container.Controls)
		{
			ReadConfig(c);

			var value = IniHelper.Instance.ReadValue(SECTION, $"{c.FindForm().Name}_{c.Name}");
			if (c is CheckBox checkBox)
			{
				if (!string.IsNullOrWhiteSpace(value)) checkBox.Checked = value.To<bool>();
				checkBox.CheckedChanged += (s, e) => IniHelper.Instance.WriteValue(SECTION, this.Name + "_" + c.Name, checkBox.Checked);
			}
			else if (c is TextBox textBox)
			{
				if (!string.IsNullOrWhiteSpace(value)) c.Text = value;
				c.TextChanged += (s, e) => IniHelper.Instance.WriteValue(SECTION, $"{c.FindForm().Name}_{c.Name}", c.Text);
			}
		}
	}

	private void DoubleClickPath(object sender, EventArgs e)
	{
		if (DateTime.Now.Subtract(m_LastTime).TotalSeconds <= 2) return;
		m_LastTime = DateTime.Now;

		var selected = (sender as Control).Text?.Trim();
		if (selected.Contains('|'))
		{
			selected = selected.Split('|')[0];
		}

		Process.Start(new ProcessStartInfo("Explorer.exe")
		{
			Arguments = Directory.Exists(selected) ? selected : ("/e,/select," + selected)
		});
	}

	public static IEnumerable<string> OpenPath(Control link, string Filter = null, bool Multiselect = false)
	{
		var openFile = new OpenFileDialog();

		string FilePath = link.Text;
		if (!string.IsNullOrWhiteSpace(FilePath) && !FilePath.Contains('|'))
		{
			openFile.InitialDirectory = Directory.Exists(FilePath) ? FilePath : new FileInfo(FilePath).DirectoryName;
		}


		openFile.Filter = (Filter == null ? null : Filter + "|") + "所有文件|*";
		openFile.Multiselect = Multiselect;

		if (openFile.ShowDialog() == DialogResult.OK)
		{
			if (openFile.FileNames.Length == 1) link.Text = openFile.FileName;
			else
			{
				StringBuilder sb = new();

				foreach (var f in openFile.FileNames)
					sb.Append(f + "|");

				link.Text = sb.ToString();
			}

			return openFile.FileNames;
		}

		return null;
	}

	private static void OpenFolder(Control link)
	{
		var dialog = new FolderBrowserDialog();
		if (dialog.ShowDialog() == DialogResult.OK) link.Text = dialog.SelectedPath;
	}

	private void ClearLog(object sender, EventArgs e)
	{
		this.richOut.Clear();
	}
	#endregion


	#region Dat
	private void bntSearchDat_Click(object sender, EventArgs e) => OpenPath(txbDatFile);

	private void txbDatFile_TextChanged(object sender, EventArgs e)
	{
		var s = (Control)sender;
		string Text = s.Text.Trim();

		if (Directory.Exists(Text))
		{
			var dir = new DirectoryInfo(Text);
			var files = dir.GetFiles("*.dat", SearchOption.AllDirectories);

			s.Text = files.FirstOrDefault()?.FullName;
		}
		else if (File.Exists(Text))
		{
			txbRpFolder.Text = Path.GetDirectoryName(Text) + @"\Export\" + Path.GetFileNameWithoutExtension(Text);
		}
	}

	private void button3_Click(object sender, EventArgs e)
	{
		var openFile = new OpenFileDialog() { Filter = "XML 文件|*.xml" };
		if (openFile.ShowDialog() != DialogResult.OK) return;

		Task.Run(() =>
		{
			XmlDocument patchDoc = new();
			patchDoc.Load(openFile.FileName);

			foreach (XmlNode patchNode in patchDoc.SelectNodes("//patch"))
			{
				string path = txbRpFolder.Text + "\\" + patchNode?.Attributes["file"]?.Value;
				if (File.Exists(path))
				{
					XmlDocument fileDoc = new() { PreserveWhitespace = false };
					fileDoc.Load(path);

					ModifyNodes(patchNode, fileDoc.DocumentElement);
					fileDoc.Save(path);
				}
			}
		});
	}

	public static void ModifyNodes(XmlNode config, XmlNode file)
	{
		foreach (XmlElement node in config.SelectNodes("./select-node"))
		{
			var query = node.GetAttribute("query");
			if (query == "/config") ModifyNodes(node, file);
			else foreach (XmlNode x in file.SelectNodes(query)) ModifyNodes(node, x);
		}

		foreach (XmlNode node in config.SelectNodes("./set-value"))
		{
			if (file is XmlAttribute attribute)
			{
				var target = node.Attributes["value"];
				if (target != null) attribute.Value = target.Value;
			}
			else
			{
				throw new NotSupportedException();
			}
		}
	}

	private void BntStart_Click(object sender, EventArgs e)
	{
		if (!File.Exists(txbDatFile.Text)) throw new Exception("选择要解压缩的.dat文件.");
		if (Directory.Exists(txbRpFolder.Text))
		{
			if (MessageBox.Show("即将开始解包文件，是否确认?", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
				return;
		}


		Task.Run(() => ThirdSupport.Extract(new PackageParam(txbDatFile.Text)
		{
			FolderPath = txbRpFolder.Text,
			//BinaryXmlVersion = BinaryXmlVersion.None,
		}));
	}

	private void btnRepack_Click(object sender, EventArgs e)
	{
		string outdir = txbRpFolder.Text;
		if (!Directory.Exists(outdir))
		{
			MessageBox.Show("请选择封包文件夹", "Error");
			return;
		}

		new Thread(o =>
		{
			try
			{
				var param = new PackageParam(txbDatFile.Text)
				{
					FolderPath = outdir,
					CompressionLevel = CompressionLevel.Fast,
				};

				if (checkBox1.Checked) ThirdSupport.Pack(param);
				else BNSDat.CreateFromDirectory(param);

				Console.WriteLine("Pack completed");
			}
			catch (Exception ee)
			{
				MessageBox.Show(ee.ToString());
			}

		}).Start();
	}
	#endregion

	#region Bin
	private void button1_Click(object sender, EventArgs e) => OpenFolder(textBox2);

	private void button8_Click(object sender, EventArgs e) => OpenPath(textBox1);

	private void button4_Click(object sender, EventArgs e) => OpenFolder(textBox3);

	private void button7_Click(object sender, EventArgs e)
	{
		var definition = TableDefinition.LoadFrom(new(), File.OpenRead(textBox1.Text));

		foreach (var element in definition.DocumentElement.Children)
		{
			foreach (var attribute in element.ExpandedAttributes.OrderBy(x => x.Offset))
			{
				Console.WriteLine($"{attribute.Offset:X}  -  {attribute.Name}");
			}

			foreach (var sub in element.Subtables)
			{
				foreach (var attribute in sub.ExpandedAttributesSubOnly.OrderBy(x => x.Offset))
				{
					Console.WriteLine($"[{sub.Name}] {attribute.Offset:X}  -  {attribute.Name}");
				}
			}
		}
	}


	private DatabaseTests set;
	private void button6_Click(object sender, EventArgs e)
	{
		Task.Run(() =>
		{
			set ??= new DatabaseTests(DefaultProvider.Load(textBox2.Text), textBox3.Text);
			try
			{
				set.Output(textBox1.Text.Split('|'));
			}
			catch (Exception ex)
			{
				Console.WriteLine("[error] " + ex);
			}
		});
	}
	#endregion

	#region Page
	private void Btn_HexToDecimal_Click(object sender, EventArgs e)
	{
		Convert_Warning.Text = null;
		string text = Convert_Hex.Text.Replace('-', ' ');

		try
		{
			if (radioButton2.Checked)
			{
				Convert_Decimal.Text = long.Parse(text, System.Globalization.NumberStyles.HexNumber).ToString();
			}
			else
			{
				var c = new CommonConvert(text.ToBytes());
				Convert_Decimal.Text = c.MainValue?.ToString();
				Convert_Warning.Text = $"Short {c.Short1},{c.Short2}\nFloat {c.Float}";
			}
		}
		catch (Exception ee)
		{
			Convert_Warning.Text = ee.Message;
		}
	}

	private void Btn_DecimalToHex_Click(object sender, EventArgs e)
	{
		Convert_Warning.Text = null;
		label2.Text = null;

		try
		{
			if (long.TryParse(Convert_Decimal.Text, out long result))
			{
				var CC = new CommonConvert(result);
				Convert_Hex.Text = CC.ToString();
				Convert_Warning.Text = $"Short {CC.Short1},{CC.Short2}\nFloat {CC.Float}";
			}
		}
		catch (Exception ee)
		{
			Convert_Warning.Text = ee.Message;
			Console.WriteLine(ee.Message);
		}
	}

	private void button12_Click(object sender, EventArgs e) => richTextBox1.Text = CreateClass.Instance(richTextBox1.Text);

	private void button5_Click(object sender, EventArgs e) => richTextBox1.Text = CreateEnum.Instance(richTextBox1.Text?.Trim());

	private void button16_Click(object sender, EventArgs e)
	{
		var sb = new StringBuilder();
		foreach (var line in this.richTextBox1.Text.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)))
		{
			string[] ls = Regex.Split(line, "\\s+");
			sb.Append($"{ls[0]}=\"{ls[1].Replace("%", null)}\" ");
		}

		this.richTextBox1.Text = $"  <record {sb}/>\n";
	}

	private void button15_Click(object sender, EventArgs e)
	{
		XmlDocument tmp = new();
		tmp.LoadXml($"<?xml version=\"1.0\"?>\n<table>{richTextBox1.Text?.Trim()}</table>");

		var record = tmp.SelectSingleNode("table/*");
		if (record is null) return;


		StringBuilder rtf = new();
		rtf.Append(@"{\rtf1 ");

		foreach (XmlAttribute attr in record.Attributes)
		{
			rtf.Append(@"\trowd");
			for (int j = 1; j <= 2; j++) rtf.Append($@"\cellx{j * 4000}");

			//create row
			rtf.Append($@"\intbl {GetAsc2Code(attr.Name)}\cell {GetAsc2Code(attr.Value)}\row");
		}

		rtf.Append(@"\pard ");
		rtf.Append('}');

		this.richTextBox1.Clear();
		this.richTextBox1.SelectedRtf = rtf.ToString();
	}

	public static string GetAsc2Code(string txt)
	{
		string result = null;
		foreach (var t in txt) result += "\\u" + (int)t + "  ";

		return result;
	}
	#endregion
}