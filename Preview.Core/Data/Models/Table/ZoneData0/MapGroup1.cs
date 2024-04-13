using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public sealed class MapGroup1 : ModelElement , IHaveName
{
	#region Attributes
	public Ref<Text> Name2 { get; set; }

	public Ref<MapGroup1Expedition> Expedition { get; set; }
	#endregion

	#region Methods
	public string Text => Name2.GetText();
	#endregion
}