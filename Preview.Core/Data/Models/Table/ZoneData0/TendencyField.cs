using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public sealed class TendencyField : ModelElement, IAttraction
{
	#region Attraction
	public string Name => this.Attributes["tendency-field-name2"].GetText();

	public string Description => this.Attributes["tendency-field-desc"].GetText();
	#endregion
}