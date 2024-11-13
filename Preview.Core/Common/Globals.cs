using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.Common;
public static partial class Globals
{
	public static ITextProvider TextProvider { internal get; set; }

	public static IMessageBox MessageBox { internal get; set; }

	public static IDatSelect DatSelector { internal get; set; }
}

public interface IMessageBox
{
	public Func<string, bool> Show { get; }
}