﻿using System.ComponentModel;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public sealed class MapInfo : ModelElement, IHaveName, INotifyPropertyChanged
{
	#region Attributes
	public short Id { get; set; }

	public string Alias { get; set; }

	public short GroupId { get; set; }

	public short Floor { get; set; }

	public Ref<Text> Name2 { get; set; }

	public Ref<MapInfo> ParentMapinfo { get; set; }

	public float Scale { get; set; }

	[Name("map-group-1")]
	public Ref<MapGroup1> MapGroup1 { get; set; }

	[Name("map-group-2")]
	public Ref<MapGroup2> MapGroup2 { get; set; }

	public float LocalAxisX { get; set; }

	public float LocalAxisY { get; set; }

	public short ImageSize { get; set; }

	public string Imageset { get; set; }

	public string ImagesetAlphamap { get; set; }

	public bool UsePosInParent { get; set; }

	public float PosInParentX { get; set; }

	public float PosInParentY { get; set; }

	public Ref<Terrain> Terrain { get; set; }

	public float Zoom { get; set; }

	public short SortNo { get; set; }

	public string ArenaDungeonParentMapinfo { get; set; }

	public bool ArenaDungeonUsePosInParent { get; set; }

	public float ArenaDungeonPosInParentX { get; set; }

	public float ArenaDungeonPosInParentY { get; set; }
	#endregion

	#region Methods
	public string Name => Name2.GetText();

	public static MapUnit.MapDepthSeq GetMapDepth(MapInfo MapInfo)
	{
		var ParentMapinfo = MapInfo.ParentMapinfo;
		if (ParentMapinfo.HasValue) return GetMapDepth(ParentMapinfo) + 1;

		return MapUnit.MapDepthSeq.N1;
	}
	#endregion


	#region INotifyPropertyChanged
	public event PropertyChangedEventHandler PropertyChanged;

	bool _isSelected;
	public bool IsSelected
	{
		get => _isSelected;
		set
		{
			_isSelected = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
		}
	}
	#endregion
}