using System.Runtime.InteropServices;
using Xylia.Preview.Common;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class KeyCommand : ModelElement
{
	[StructLayout(LayoutKind.Explicit, Size = 64)]
	public struct KeyCommandKey(KeyCommandSeq command, JobSeq pcjob) : IGameDataKey
	{
		[FieldOffset(0)] public KeyCommandSeq Command = command;
		[FieldOffset(2)] public JobSeq PcJob = pcjob;
	}

	#region Attributes
	public KeyCommandSeq Command { get; set; }

	public JobSeq PcJob { get; set; }

	public CategorySeq Category { get; set; }

	public enum CategorySeq
	{
		None,
		Movement,
		Panel,
		Function,
		Skill,
		Social,
		Mark,
		BnsMode,
		Joypad,
		Spectate,
		COUNT
	}

	public JoypadCategorySeq JoypadCategory { get; set; }

	public enum JoypadCategorySeq
	{
		None,
		JoypadMovement,
		JoypadSkill,
		JoypadPanelAndFunction,
		JoypadSocial,
		JoypadMark,
		JoypadSpecialFunction,
		JoypadSpectate,
		COUNT
	}

	public Ref<Text> Name { get; set; }

	public string DefaultKeycap { get; set; }

	public bool ModifierEnabled { get; set; }

	public sbyte SortNo { get; set; }

	public sbyte Layer { get; set; }

	public short OptionSortNo { get; set; }

	public UsableJoypadModeSeq UsableJoypadMode { get; set; }

	public enum UsableJoypadModeSeq
	{
		None,
		Any,
		Ui,
		Action,
		COUNT
	}

	public bool JoypadCustomizeEnabled { get; set; }

	public bool JoypadOverlappedBindingEnabled { get; set; }
	#endregion

	#region Methods
	/// <summary>
	/// 0 is HotKey<br/>1 is SubHotKey<br/>2 is Joypad
	/// </summary>
	/// <returns></returns>
	private List<KeyCap[]> GetKeyCodes()
	{
		var result = new List<KeyCap[]>();

		if (DefaultKeycap != null)
		{
			foreach (var s in DefaultKeycap.Split(','))
			{
				var caps = new KeyCap[2];

				if (s.StartsWith('^'))
				{
					caps[0] = KeyCode.Control;
					caps[1] = KeyCap.GetKeyCode(s[1..]);
				}
				else if (s.StartsWith('~'))
				{
					caps[0] = KeyCode.Alt;
					caps[1] = KeyCap.GetKeyCode(s[1..]);
				}
				else
				{
					caps[1] = KeyCap.GetKeyCode(s);
				}

				result.Add(caps);
			}
		}

		return result;
	}

	public KeyCap Key1 => GetKeyCodes()[0][1];

	public override string ToString() => string.Join('+', GetKeyCodes()[0].SelectNotNull(code => code?.Name.GetText()));

	public static KeyCommand Cast(KeyCommandSeq command) => Globals.GameData.Provider.GetTable<KeyCommand>()[new KeyCommandKey(command, JobSeq.JobNone)];
	#endregion
}