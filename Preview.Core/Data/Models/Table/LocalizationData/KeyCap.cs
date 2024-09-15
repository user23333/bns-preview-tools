using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class KeyCap : ModelElement
{
	#region Properties
	public KeyCode KeyCode => this.Attributes.Get<KeyCode>("key-code");

	public ImageProperty Icon => Attributes.Get<Icon>("icon")?.GetImage();

	public string Image => this.Attributes["image"].GetText();
	#endregion

	#region Methods
	public static KeyCode GetKeyCode(string o)
	{
		// different from sequence
		if (o == "SPACEBAR") return KeyCode.Space;

		return o.Replace("_", null).ToEnum<KeyCode>();
	}

	public static KeyCap Cast(string KeyCode) => Cast(GetKeyCode(KeyCode));

	public static KeyCap Cast(KeyCode KeyCode) => FileCache.Data.Provider.GetTable<KeyCap>()[(byte)KeyCode];
	#endregion
}