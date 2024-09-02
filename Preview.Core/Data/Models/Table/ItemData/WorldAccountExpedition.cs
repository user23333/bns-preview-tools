using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.Data.Models;
public sealed class WorldAccountExpedition : ModelElement
{
	#region Attributes
	public enum ExpeditionTypeSeq
	{
		None,
		Collection,
		Card,
		Monster,
		Story,
		ViewPoint,
		COUNT
	}

	public ExpeditionTypeSeq ExpeditionType { get; set; }


	public Ref<ModelElement>[] Target { get; set; }

	public short[] Count { get; set; }
	#endregion

	#region Methods
	public static Dictionary<ModelElement, WorldAccountExpedition> GetTargets(IDataProvider provider)
	{
		Dictionary<ModelElement, WorldAccountExpedition> dict = [];

		foreach (var record in provider.GetTable<WorldAccountExpedition>())
		{
			record.Target.Select(x => x.Instance).ForEach(x => dict[x] = record);
		}

		return dict;
	}
	#endregion
}