﻿using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Common.Interface;

namespace Xylia.Preview.Data.Models;
public sealed class FactionBattleFieldZone : Record, IAttraction
{
	public Ref<Zone> Zone;
	public Ref<AttractionGroup> Group;

	public bool UiFilterAttractionQuestOnly;

	public Ref<Text> RespawnConfirmText;

	public sbyte RequiredLevel;
	public sbyte RequiredFactionLevel;

	public Ref<Text> FactionBattleFieldZoneName2;
	public Ref<Text> FactionBattleFieldZoneDesc;

	public string ThumbnailImage;

	public Ref<AttractionRewardSummary> RewardSummary;


	#region Interface
	public override string GetText => this.FactionBattleFieldZoneName2.GetText();

	public string GetDescribe() => this.FactionBattleFieldZoneDesc.GetText();
	#endregion
}