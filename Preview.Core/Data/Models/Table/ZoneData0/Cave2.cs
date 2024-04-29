namespace Xylia.Preview.Data.Models;
public sealed class Cave2 : ModelElement, IAttration
{
	#region Attributes
	public sbyte UiTextGrade;
	#endregion

	#region IAttraction
	public string Name => this.Attributes["cave2-name2"].GetText();
	public string Description => this.Attributes["cave2-desc"].GetText();
	#endregion
}