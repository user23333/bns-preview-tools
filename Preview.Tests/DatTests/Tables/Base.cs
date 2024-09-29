using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.Tests.DatTests;
[TestClass]
public partial class TableTests
{
	private BnsDatabase Database { get; } = new(new FolderProvider(
		new DirectoryInfo(@"D:\Tencent\BnsData\GameData_ZTx").GetDirectories()[^1].FullName));
}