using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.Core;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Common;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Controls.Primitives;
using Xylia.Preview.UI.Converters;
using Xylia.Preview.UI.Extensions;

namespace Xylia.Preview.UI.Controls;
public class BnsCustomMinimapWidget : BnsCustomBaseWidget
{
	#region Constructors
	public BnsCustomMinimapWidget()
	{
		Focusable = true;
		LayoutTransform = Transform = new ScaleTransform() { ScaleX = 1, ScaleY = 1 };
		UnitFilters.OnFilterChanged += FilterUnit;
	}
	#endregion

	#region Events
	public event EventHandler<MapInfo>? MapChanged;
	public event EventHandler<Vector32>? OnClick;
	#endregion

	#region Public Properties
	private static readonly Type Owner = typeof(BnsCustomMinimapWidget);

	public static readonly DependencyProperty MapInfoProperty = Owner.Register<MapInfo>(nameof(MapInfo), null,
		FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnMapChanged);

	public MapInfo MapInfo
	{
		get => (MapInfo)this.GetValue(MapInfoProperty);
		set => this.SetValue(MapInfoProperty, value);
	}

	public static readonly DependencyProperty ZoomProperty = Owner.Register(nameof(Zoom), 1d, callback: SetZoom);

	public double Zoom
	{
		get => (double)this.GetValue(ZoomProperty);
		set => this.SetValue(ZoomProperty, value);
	}
	#endregion

	#region Protected Methods
	protected override Size MeasureOverride(Size constraint)
	{
		base.MeasureOverride(constraint);

		var size = MapInfo?.ImageSize ?? 0;
		return new Size(size, size);
	}

	protected override void OnMouseEnter(MouseEventArgs e)
	{
		this.Focus();
		base.OnMouseEnter(e);
	}

	protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed)
		{
			_original = new Point(ScrollOffset.X, ScrollOffset.Y);
			_mouseOffset = e.GetPosition(this);
		}

		if (e.RightButton == MouseButtonState.Pressed)
		{
			var offset = Mouse.GetPosition(this);
			OnClick?.Invoke(this, (Vector32)Parse(offset));
		}
	}

	//protected override void OnPreviewMouseMove(MouseEventArgs e)
	//{
	//	if (e.LeftButton == MouseButtonState.Pressed)
	//	{
	//		_isDragging = true;
	//		Cursor = Cursors.SizeAll;

	//		var offset = _mouseOffset - e.GetPosition(this);
	//		this.ScrollOffset = new Vector(
	//			_original.X + offset.X,
	//			_original.Y + offset.Y);
	//		this.InvalidateVisual();
	//	}
	//}

	//protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
	//{
	//	if (_isDragging)
	//	{
	//		_isDragging = false;
	//		e.Handled = true;

	//		Cursor = Cursors.Arrow;
	//	}
	//}

	protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
	{
		if (!Keyboard.IsKeyDown(Key.LeftCtrl)) return;
		if (e.Delta < 0) Zoom -= 0.1;
		else Zoom += 0.1;

		e.Handled = true;
	}

	protected override void OnPreviewKeyDown(KeyEventArgs e)
	{
		base.OnPreviewKeyDown(e);

		if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
		{
			switch (e.Key)
			{
				case Key.Add: Zoom += 0.1; break;
				case Key.Subtract: Zoom -= 0.1; break;
				case Key.NumPad0: Zoom = 1.0; break;
			}
		}
	}
	#endregion

	#region Private Methods
	private static void SetZoom(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		var widget = (BnsCustomMinimapWidget)d;
		widget.Transform.ScaleX = widget.Transform.ScaleY = (double)e.NewValue;
	}

	private static async void OnMapChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		var widget = (BnsCustomMinimapWidget)d;
		var value = (MapInfo)e.NewValue;
		await widget.OnMapChanged(value);
	}

	private async Task OnMapChanged(MapInfo value)
	{
		this.Children.Clear();

		this.Zoom = value.Zoom;
		this.MapDepth = MapInfo.GetMapDepth(value);
		this.BaseImageProperty = new ImageProperty()
		{
			EnableImageSet = true,
			ImageSet = new MyFPackageIndex(value.Imageset)
		};

		// unit
		await this.LoadMapUnit(value, []);
		this.MapChanged?.Invoke(this, value);
	}

	private async Task LoadMapUnit(MapInfo MapInfo, List<MapInfo> MapTree)
	{
		MapTree.Add(MapInfo);
		await GetMapUnit(MapInfo, MapTree);

		// children
		if (MapInfo.Alias != "World")
		{
			Globals.GameData.Provider.GetTable<MapInfo>()
				.Where(o => o.ParentMapinfo == MapInfo)
				.ForEach(async o => await LoadMapUnit(o, new(MapTree)));
		}
	}

	private async Task GetMapUnit(MapInfo MapInfo, List<MapInfo> MapTree)
	{
		// skip force position
		var provider = Globals.GameData.Provider;
		if (MapTree.Any(x => x.UsePosInParent)) return;

		// find zone
		foreach (var zone in provider.GetTable<Zone>().Where(x => x.Map == MapInfo && x.ZoneType2 == Zone.ZoneType2Seq.Persistent))
		{
			foreach (var record in provider.GetTable<ZoneTeleportPosition>().Where(x => x.Zone == zone.Id))
			{
				var teleport = provider.GetTable<Teleport>().FirstOrDefault(x => x.TeleportPosition == record);
				if (teleport is null) continue;

				var Image = new ImageProperty() { EnableImageSet = true, ImageSet = new MyFPackageIndex("00009499.Teleport_point_possible_normal") };
				var OverImage = new ImageProperty() { EnableImageSet = true, ImageSet = new MyFPackageIndex("00009499.Teleport_point_possible_over") };

				AddChild(record.Position, null, () =>
				{
					var widget = new BnsCustomImageWidget()
					{
						Tag = MapUnit.CategorySeq.Teleport,
						ToolTip = teleport.Name,
						BaseImageProperty = Image,
					};
					widget.MouseEnter += new((_, _) => widget.BaseImageProperty = OverImage);
					widget.MouseLeave += new((_, _) => widget.BaseImageProperty = Image);

					return widget;
				});
			}

			foreach (var record in provider.GetTable<ZoneEnv2Spawn>().Where(x => x.Zone == zone.Id))
			{
				var env2 = record.Env2.Value;
				var env2place = record.Env2place.Value;
				if (env2 is null || env2place is null || string.IsNullOrEmpty(env2.MapunitImageDisableImageset)) continue;

				var point = env2place.Attributes.Get<Vector32>("action-point");
				var size = new FVector2D(env2.MapunitImageDisableSizeX, env2.MapunitImageDisableSizeY);
				var Image = new ImageProperty() { EnableImageSet = true, ImageSet = new MyFPackageIndex(env2.MapunitImageDisableImageset) };
				var OverImage = new ImageProperty() { EnableImageSet = true, ImageSet = new MyFPackageIndex(env2.MapunitImageDisableOverImageset) };

				AddChild(point, size, () =>
				{
					var widget = new BnsCustomImageWidget()
					{
						BaseImageProperty = Image,
						Tag = env2.MapUnitCategory,
						ToolTip = record.Env2.Value.Name,
					};
					widget.MouseEnter += new((_, _) => widget.BaseImageProperty = OverImage);
					widget.MouseLeave += new((_, _) => widget.BaseImageProperty = Image);

					return widget;
				});
			}
		}

		// unit		 
		foreach (var mapunit in provider.GetTable<MapUnit>().Where(o => o.Mapid == MapInfo.Id && o.MapDepth <= this.MapDepth))
		{
			// ignore quest area guide
			if (mapunit is MapUnit.Quest or MapUnit.GuildBattleFieldPortal) continue;

			#region Initialize	
			var category = mapunit.Category;
			object? tooltip = mapunit.Name;
			var Image = new ImageProperty() { EnableImageSet = true, ImageSet = new MyFPackageIndex(mapunit.Imageset) };
			var OverImage = mapunit.OverImageset.IsValid ? new ImageProperty() { EnableImageSet = true, ImageSet = new MyFPackageIndex(mapunit.OverImageset) } : Image;

			if (mapunit is MapUnit.Attraction)
			{
				tooltip = mapunit.Attributes.Get<ModelElement>("attraction");  //tref
			}
			else if (mapunit is MapUnit.Npc)
			{
				var npc = mapunit.Attributes.Get<Npc>("npc");
				if (npc != null)
				{
					tooltip = npc.Name2.GetText();

					for (int i = 0; i < npc.ForwardingTypes.Length; i++)
					{
						var forwardingType = npc.ForwardingTypes[i];
						if (forwardingType == ForwardingType.AcquireQuest)
						{
							var quest = npc.Quests[i];
							category = MapUnit.CategorySeq.Quest;
							tooltip += $"<br/><arg id=\"quest:{quest}\" p=\"id:quest.front-icon.scale.150\"/> <arg id=\"quest:{quest}\" p=\"id:quest.name2\"/>";

							Image = quest.Value?.FrontIcon;
							OverImage = quest.Value?.FrontIconOver;
						}
					}
				}
			}
			else if (mapunit is MapUnit.Boss)
			{
				var npc = mapunit.Attributes.Get<Npc>("npc");
				if (npc != null) tooltip = npc.Name2.GetText();
			}
			#endregion

			#region Widget
			AddChild(mapunit.Position, mapunit.Size, () =>
			{
				var widget = new BnsCustomImageWidget()
				{
					BaseImageProperty = Image,
					Tag = category,
					ToolTip = new BnsTooltipHolder(),
					DataContext = tooltip,
				};
				widget.MouseEnter += new((_, _) => widget.BaseImageProperty = OverImage);
				widget.MouseLeave += new((_, _) => widget.BaseImageProperty = Image);
				widget.MouseLeftButtonDown += new((_, _) =>
				{
					if (mapunit is MapUnit.Link) this.MapInfo = provider.GetTable<MapInfo>()[mapunit.Attributes.Get<short>("link-mapid")];
					else Debug.WriteLine(mapunit.Attributes);
				});

				return widget;
			});
			#endregion
		}
	}

	private void FilterUnit(object? sender, EventArgs args)
	{
		foreach (FrameworkElement element in this.Children)
		{
			if (element.Tag is MapUnit.CategorySeq category)
			{
				element.SetVisiable(UnitFilters.Contains(category));
			}
		}
	}
	#endregion


	#region Helpers
	/// <summary>
	/// The axis direction is diffrent with the layout direction
	/// </summary>
	private Point Parse(FVector position)
	{
		ArgumentNullException.ThrowIfNull(MapInfo);

		float posX = (position.X - MapInfo.LocalAxisX) / MapInfo.Scale;
		float posY = (position.Y - MapInfo.LocalAxisY) / MapInfo.Scale;

		return new Point(posY, MapInfo.ImageSize - posX);
	}

	public FVector Parse(Point point)
	{
		ArgumentNullException.ThrowIfNull(MapInfo);

		return new FVector(
			MapInfo.LocalAxisX + MapInfo.Scale * (MapInfo.ImageSize - point.Y),
			MapInfo.LocalAxisY + MapInfo.Scale * point.X,
			0);
	}

	/// <summary>
	/// Adds the specified element to the widget
	/// </summary>
	/// <param name="position"></param>
	/// <param name="widget"></param>
	public void AddChild(FVector position, FVector2D? size, UserWidget widget)
	{
		var offset = Parse(position);
		size ??= new FVector2D(32, 32);

		if (widget.Tag is MapUnit.CategorySeq category)
		{
			widget.SetVisiable(UnitFilters.Contains(category));
		}

		LayoutData.SetAlignments(widget, new FVector2D(0.5f, 0.5f));
		LayoutData.SetOffsets(widget, new FLayout.Offset((float)offset.X, (float)offset.Y, size.Value.X, size.Value.Y));
		Children.Add(widget);
	}

	public void AddChild(FVector position, FVector2D? size, Func<UserWidget> widget) => AddChild(position, size, Dispatcher.Invoke(widget.Invoke));

	public class MapUnitFilterManager : IEnumerable
	{
		public event EventHandler? OnFilterChanged;
		private readonly Dictionary<MapUnit.CategorySeq, MapUnitFilter> dict = [];

		public MapUnitFilterManager()
		{
			Add(new(this, MapUnit.CategorySeq.Quest));
			Add(new(this, MapUnit.CategorySeq.Teleport));
			Add(new(this, MapUnit.CategorySeq.Airdash));
			Add(new(this, MapUnit.CategorySeq.Auction));
			Add(new(this, MapUnit.CategorySeq.Store));
			Add(new(this, MapUnit.CategorySeq.Camp));
			Add(new(this, MapUnit.CategorySeq.PartyCamp));
			Add(new(this, MapUnit.CategorySeq.Roulette));
			Add(new(this, MapUnit.CategorySeq.FieldBoss));
			Add(new(this, MapUnit.CategorySeq.Craft));
			Add(new(this, MapUnit.CategorySeq.ExpeditionEnv));
			Add(new(this, MapUnit.CategorySeq.ExpeditionEnv_Collection) { IsChecked = false });
			Add(new(this, MapUnit.CategorySeq.WanderingNpc));
			Add(new(this, MapUnit.CategorySeq.Npc) { IsChecked = false });
			Add(new(this, MapUnit.CategorySeq.Env) { NotUsed = true });
			Add(new(this, MapUnit.CategorySeq.GatherEnv) { NotUsed = true });
		}

		private class MapUnitFilter(MapUnitFilterManager manager, MapUnit.CategorySeq category)
		{
			public MapUnit.CategorySeq Category => category;
			public string Name => category.GetText();
			public bool NotUsed { get; set; }

			private bool _isChecked = true;
			public bool IsChecked
			{
				get => _isChecked;
				set
				{
					_isChecked = value;
					manager.OnFilterChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		private void Add(MapUnitFilter filter) => dict.Add(filter.Category, filter);

		public bool Contains(MapUnit.CategorySeq category) => !dict.TryGetValue(category, out var filter) || !filter.NotUsed & filter.IsChecked;

		public IEnumerator GetEnumerator() => dict.Values.Where(x => !x.NotUsed).GetEnumerator();
	}
	#endregion

	#region Fields
	public MapUnitFilterManager UnitFilters = new();
	MapUnit.MapDepthSeq MapDepth;

	readonly ScaleTransform Transform;
	bool _isDragging;
	Point _mouseOffset;
	Point _original;
	#endregion
}