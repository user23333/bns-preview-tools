using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class WorldAccountExpedition : ModelElement
{
	#region Attributes
	public short Group { get; set; }

	public sbyte Level { get; set; }

	public string Alias { get; set; }

	public bool Deprecated { get; set; }

	public ExpeditionTypeSeq ExpeditionType { get; set; }

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

	public Ref<ModelElement>[] Target { get; set; }

	public short[] Count { get; set; }

	public ConditionSeq Condition { get; set; }

	public enum ConditionSeq
	{
		And,
		Or,
		COUNT
	}

	public AttachAbilitySeq[] Ability { get; set; }

	public int[] AbilityValue { get; set; }

	public int[] AbilityBaseValue { get; set; }

	public bool Boss { get; set; }

	[Name("map-group-1")]
	public Ref<MapGroup1> MapGroup1 { get; set; }

	public Ref<Text> Name { get; set; }

	public Ref<Text> Description { get; set; }

	public Ref<Text> Story { get; set; }

	public ObjectPath BossImage { get; set; }

	public Icon[] TargetIcon { get; set; }

	public Ref<Text>[] TargetDesc { get; set; }
	#endregion

	#region Methods
	public static Dictionary<ModelElement, WorldAccountExpedition> GetTargets(IDataProvider provider)
	{
		Dictionary<ModelElement, WorldAccountExpedition> dict = [];

		foreach (var record in provider.GetTable<WorldAccountExpedition>())
		{
			record.Target.Values().ForEach(x => dict[x] = record);
		}

		return dict;
	}
	#endregion
}