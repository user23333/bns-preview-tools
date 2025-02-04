using System.Globalization;

namespace Xylia.Preview.Data.Models.Document;
public class Link : HtmlElementNode
{
	#region Fields
	public string Id;
	public bool IgnoreInput;
	public bool Editable;

	#endregion

	#region Methods 
	public static long[] Parse(string id) => id.Split('.').Select(x => long.Parse(x, NumberStyles.HexNumber)).ToArray();
	#endregion
}