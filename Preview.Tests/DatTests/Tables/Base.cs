using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.Tests.DatTests;
[TestClass]
public partial class TableTests
{
	private BnsDatabase Database { get; } = new(new FolderProvider(
		new DirectoryInfo(@"D:\Tencent\BnsData\GameData_ZTx").GetDirectories()
			.Where(o => Regex.IsMatch(o.Name, "^[0-9]{1,8}$")).Last().FullName,
		new Locale(EPublisher.ZTx)), Globals.Definition);
}