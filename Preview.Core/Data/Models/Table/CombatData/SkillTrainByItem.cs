using System.Text;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Properties;

namespace Xylia.Preview.Data.Models;
public sealed class SkillTrainByItem : ModelElement
{
	#region Attributes
	public Ref<Skill3>[] OriginSkill { get; set; }

	public Ref<Skill3>[] ChangeSkill { get; set; }

	public string Icon { get; set; }

	public Ref<Text> Description { get; set; }
	#endregion


	#region Methods
	public string Description2
	{
		get
		{
			// dislay raw data
			if (Settings.Default.UseDebugMode)
			{
				StringBuilder builder = new();

				for (int x = 0; x < 6; x++)
				{
					var OriginSkill = this.OriginSkill[x].Instance;
					var ChangeSkill = this.ChangeSkill[x].Instance;
					if (OriginSkill is null) continue;

					// newline 
					if (builder.Length != 0) builder.Append("<br/>");

					builder.Append($"<font name=\"00008130.UI.Vital_LightBlue\">{OriginSkill?.Name2}</font>武功变更为<font name=\"00008130.UI.Vital_LightBlue\">{ChangeSkill.Name2}</font><br/>");
					ChangeSkill.Systematization.ForEach(s => builder.Append($"<arg id=\"skill-systematization:{s}\" p=\"id:skill-systematization.name2\"/> "));
				}

				return builder.ToString();
			}

			return Description.GetText();
		}
	}
	#endregion
}