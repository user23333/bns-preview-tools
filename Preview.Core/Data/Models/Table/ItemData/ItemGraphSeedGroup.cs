using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public sealed class ItemGraphSeedGroup : ModelElement ,IHaveName
{
	#region Methods
	public string Name => this.Attributes["name2"].GetText() ?? ToString();
	#endregion
}