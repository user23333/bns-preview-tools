using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Helpers;

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
	public static Dictionary<ModelElement, WorldAccountExpedition> GetTargets()
	{
		Dictionary<ModelElement, WorldAccountExpedition> dict = [];

		foreach (var record in FileCache.Data.Provider.GetTable<WorldAccountExpedition>())
		{
			record.Target.ForEach(x => dict[x] = record);
		}

		return dict;
	}
	#endregion
}