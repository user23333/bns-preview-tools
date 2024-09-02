using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Helpers;

namespace Xylia.Preview.Data.Models;
public class SkillModifyInfo : ModelElement
{
	#region Attributes
	public short RecycleDurationModifyPercent { get; set; }

	public Msec RecycleDurationModifyDiff { get; set; }

	public short[] SpConsumeModifyDiff { get; set; }

	public short DamagePowerPercentModifyPercent { get; set; }

	public int DamagePowerPercentModifyDiff { get; set; }

	public short HpDrainPercentModifyPercent { get; set; }

	public int HpDrainPercentModifyDiff { get; set; }

	public short HealPercentModifyPercent { get; set; }

	public int HealPercentModifyDiff { get; set; }

	public Ref<Text> Description { get; set; }

	public sealed class Normal : SkillModifyInfo
	{

	}

	public sealed class Skill : SkillModifyInfo
	{
		public int[] ParentSkill3Id { get; set; }
	}

	public sealed class Skillsystematization : SkillModifyInfo
	{
		public Ref<Skillsystematization> Systematization { get; set; }
	}
	#endregion


	#region Methods
	private enum TextType
	{
		Percent,
		Value,
		Second1,
		Second2,
	}

	public string Describe
	{
		get
		{
			#region text
			List<string> Text = [];
			void AddText(string name, double value, TextType type)
			{
				if (value == 0) return;
				if (type == TextType.Percent) value /= 10;

				Text.Add((type switch
				{
					TextType.Percent => value > 0 ? "Name.SkillModifyByEquipment.Plus.Percent" : "Name.SkillModifyByEquipment.Minus.Percent",
					TextType.Value => value > 0 ? "Name.SkillModifyByEquipment.Plus.Value" : "Name.SkillModifyByEquipment.Minus.Value",
					TextType.Second1 => value > 0 ? "Name.SkillModifyByEquipment.Plus.Second" : "Name.SkillModifyByEquipment.Minus.Second",
					TextType.Second2 => value > 0 ? "Name.SkillModifyByEquipment.Plus.Second.Integer" : "Name.SkillModifyByEquipment.Minus.Second.Integer",
					_ => null,
				}).GetText([this , name.GetText(), Math.Abs(value)]));
			}

			AddText("Name.SkillModifyByEquipment.recycle-duration", this.RecycleDurationModifyPercent, TextType.Second1);
			AddText("Name.SkillModifyByEquipment.recycle-duration", this.RecycleDurationModifyDiff.TotalSeconds, TextType.Second2);
			AddText("Name.SkillModifyByEquipment.damage-power-percent", this.DamagePowerPercentModifyPercent, TextType.Percent);
			AddText("Name.SkillModifyByEquipment.damage-power-percent", this.DamagePowerPercentModifyDiff, TextType.Value);
			AddText("Name.SkillModifyByEquipment.sp-consume", this.SpConsumeModifyDiff[0], TextType.Value);
			AddText("Name.SkillModifyByEquipment.sp-consume", this.SpConsumeModifyDiff[1], TextType.Value);
			AddText("Name.SkillModifyByEquipment.hp-drain-percent", this.HpDrainPercentModifyPercent, TextType.Percent);
			AddText("Name.SkillModifyByEquipment.hp-drain-percent", this.HpDrainPercentModifyDiff, TextType.Value);
			AddText("Name.SkillModifyByEquipment.heal-percent", this.HealPercentModifyPercent, TextType.Percent);
			AddText("Name.SkillModifyByEquipment.heal-percent", this.HealPercentModifyDiff, TextType.Value);

			if (Text.Count == 0) return null;
			#endregion

			#region skill
			string skill = null;
			if (this is Skill Skill)
			{
				skill = string.Format("<font name=\"00008130.UI.Vital_LightBlue\">{0}</font>",
					string.Join(", ", Skill.ParentSkill3Id.SelectNotNull(id => Provider.GetTable<Skill3>()[new Ref(id, 1)]?.Name2.GetText())));
			}
			#endregion

			return string.Join("<br/>", Text.Select(text => skill + text));
		}
	}
	#endregion
}