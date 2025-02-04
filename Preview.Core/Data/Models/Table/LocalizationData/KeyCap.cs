using Xylia.Preview.Common;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class KeyCap : ModelElement
{
	#region Attributes
	public KeyCode KeyCode { get; set; }

	public Ref<Text> Name { get; set; }

	public Ref<Text> ShortName { get; set; }

	public Ref<Text> Image { get; set; }

	public Icon Icon { get; set; }

	public string ScrollImageset { get; set; }

	public float ScrollImagesetScale { get; set; }
	#endregion

	#region Methods
	public static KeyCode GetKeyCode(string s)
	{
		// different from sequence
		if (s == "SPACEBAR") return KeyCode.Space;

		return s.Replace("_", null).ToEnum<KeyCode>();
	}

	public static implicit operator KeyCap(KeyCode code) => Globals.GameData.Provider.GetTable<KeyCap>()[(byte)code];
	#endregion
}