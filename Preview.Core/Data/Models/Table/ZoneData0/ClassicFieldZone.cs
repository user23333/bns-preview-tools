using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public sealed class ClassicFieldZone : ModelElement, IAttration
{
	#region IAttraction
	public string Name => this.Attributes["classic-field-zone-name2"].GetText();

	public string Description => this.Attributes["classic-field-zone-desc"].GetText();
	#endregion
}