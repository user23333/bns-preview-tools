using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class KeyCommand : ModelElement
{
	#region Attributes
	public KeyCommandSeq Command { get; set; }

	public string DefaultKeycap { get; set; }
	#endregion


	#region Methods
	private KeyCap[] GetKeyCaps()
	{
		var result = new List<KeyCap>();

		if (this.DefaultKeycap != null)
		{
			foreach (var o in DefaultKeycap.Split(','))
			{
				if (string.IsNullOrWhiteSpace(o) || o == "none") continue;

				if (o.StartsWith('^'))
				{
					result.Add(KeyCap.Cast(KeyCode.Control));
					result.Add(KeyCap.Cast(o[1..]));
				}
				else if (o.StartsWith('~'))
				{
					result.Add(KeyCap.Cast(KeyCode.Alt));
					result.Add(KeyCap.Cast(o[1..]));
				}
				else result.Add(KeyCap.Cast(o));
			}
		}

		return [.. result];
	}

	private KeyCap GetKey(int Index) => this.GetKeyCaps().Length >= Index + 1 ? this.GetKeyCaps()[Index] : null;

	public KeyCap Key1 => GetKey(0);
	public KeyCap Key2 => GetKey(1);

	public override string ToString() => this.Key1?.Image;

	public static KeyCommand Cast(KeyCommandSeq KeyCommand) => FileCache.Data.Provider.GetTable<KeyCommand>().FirstOrDefault(o => o.Command == KeyCommand);
	#endregion
}