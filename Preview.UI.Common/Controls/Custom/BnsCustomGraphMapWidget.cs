using System.Collections.Concurrent;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Common;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Controls.Helpers;
using Xylia.Preview.UI.Controls.Primitives;
using Xylia.Preview.UI.Extensions;
using static Xylia.Preview.Data.Models.Item.Gem;
using static Xylia.Preview.Data.Models.ItemGraph;

namespace Xylia.Preview.UI.Controls;
public class BnsCustomGraphMapWidget : BnsCustomBaseWidget
{
	#region Constructors
	static BnsCustomGraphMapWidget()
	{
		InitializeCommands();
	}

	public BnsCustomGraphMapWidget()
	{
		Background = Brushes.Transparent;
		ClipToBounds = true;
		LayoutTransform = Transform = new TranslateTransform();
	}
	#endregion

	#region Commands
	public static RoutedCommand ViewDetail { get; private set; }
	public static RoutedCommand ChangeSubGroup { get; private set; }
	public static RoutedCommand SetStartingPoint { get; private set; }
	public static RoutedCommand SetDestination { get; private set; }

	static void InitializeCommands()
	{
		ViewDetail = new RoutedCommand("ViewDetail", Owner);
		ChangeSubGroup = new RoutedCommand("SetStartingPoint", Owner);
		SetStartingPoint = new RoutedCommand("SetStartingPoint", Owner);
		SetDestination = new RoutedCommand("SetDestination", Owner);

		Owner.RegisterCommandHandler(ViewDetail, OnViewDetail);
		Owner.RegisterCommandHandler(ChangeSubGroup, OnChangeSubGroup, CanChangeSubGroup);
		Owner.RegisterCommandHandler(SetStartingPoint, OnSetStartingPoint);
		Owner.RegisterCommandHandler(SetDestination, OnSetDestination);
	}

	private void SetCommandTarget(DependencyObject target, ContextMenu parent)
	{
		System.Windows.Data.BindingOperations.SetBinding(target, MenuItem.CommandTargetProperty, new Binding(parent, "PlacementTarget"));
	}

	private static void CanChangeSubGroup(object sender, CanExecuteRoutedEventArgs e)
	{
		if (e.Source is not BnsCustomImageWidget node) return;

		// check if exist sub-group
		//e.CanExecute = !inloading && source != null && source.CanSave;
	}

	private static void OnChangeSubGroup(object sender, ExecutedRoutedEventArgs e)
	{
		if (sender is not BnsCustomGraphMapWidget widget) return;
	}

	private static void OnSetStartingPoint(object sender, ExecutedRoutedEventArgs e)
	{
		if (sender is not BnsCustomGraphMapWidget widget) return;

		widget.Starting?.ExpansionComponentList["Node_StartImage"]?.SetExpansionShow(false);
		widget.Starting?.InvalidateVisual();
		widget.Starting = e.Source as BnsCustomImageWidget;
		widget.Starting!.ExpansionComponentList["Node_StartImage"]?.SetExpansionShow(true);
		widget.Starting!.InvalidateVisual();

		widget.FindPath();
	}

	private static void OnSetDestination(object sender, ExecutedRoutedEventArgs e)
	{
		if (sender is not BnsCustomGraphMapWidget widget) return;

		widget.Destination?.ExpansionComponentList["Node_PurposeImage"]?.SetExpansionShow(false);
		widget.Destination?.InvalidateVisual();
		widget.Destination = e.Source as BnsCustomImageWidget;
		widget.Destination!.ExpansionComponentList["Node_PurposeImage"]?.SetExpansionShow(true);
		widget.Destination!.InvalidateVisual();

		widget.FindPath();
	}

	private static void OnViewDetail(object sender, ExecutedRoutedEventArgs e)
	{
		if (sender is not BnsCustomGraphMapWidget widget) return;
		if (e.Source is not BnsCustomImageWidget node) return;

		widget.ViewDetailed?.Invoke(node, node.DataContext);
	}
	#endregion

	#region Event
	public event EventHandler<IEnumerable<ItemGraphRouteHelper>>? RoutesChanged;

	public event EventHandler<Edge>? SetRouteThrough;

	public event EventHandler<object> ViewDetailed;
	#endregion

	#region Public Properties
	private static readonly Type Owner = typeof(BnsCustomGraphMapWidget);

	public static readonly DependencyProperty CellSizeProperty = Owner.Register(nameof(CellSize), new Size(150, 150),
		FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender);

	public static readonly DependencyProperty ShowLinesProperty = Owner.Register(nameof(ShowLines), BooleanBoxes.FalseBox);
	internal static readonly DependencyProperty RatioProperty = Owner.Register(nameof(Ratio), 1d, callback: SetRatio);

	public Size CellSize { get => (Size)this.GetValue(CellSizeProperty); set => this.SetValue(CellSizeProperty, value); }

	public bool ShowLines { get => (bool)this.GetValue(ShowLinesProperty); set => this.SetValue(ShowLinesProperty, value); }


	//public int ColumnGap { get; set; } = 45;
	//public int RowGap { get; set; } = 35;

	public float MaxZoomRatio { get; set; } = 1.3F;
	public float MinZoomRatio { get; set; } = 0.7F;

	public double Ratio
	{
		get => (double)this.GetValue(RatioProperty);
		set => this.SetValue(RatioProperty, value);
	}
	#endregion


	#region Protected Methods
	protected override void OnInitialized(EventArgs e)
	{
		base.OnInitialized(e);

		// Template
		NodeTemplate = GetChild<BnsCustomImageWidget>("NodeTemplate");
		HorizontalRulerItemTemplate = GetChild<BnsCustomImageWidget>("HorizontalRulerItemTemplate");
		VerticalRulerItemTemplate = GetChild<BnsCustomImageWidget>("VerticalRulerItemTemplate");

		// ContextMenu
		var NodeMenu = new ContextMenu();
		NodeMenu.Items.Add(new MenuItem() { Header = "详细信息", Command = ViewDetail });
		NodeMenu.Items.Add(new MenuItem() { Header = "切换", Command = ChangeSubGroup });
		NodeMenu.Items.Add(new MenuItem() { Header = "设为起始物品", Command = SetStartingPoint });
		NodeMenu.Items.Add(new MenuItem() { Header = "设为结束物品", Command = SetDestination });
		NodeMenu.Items.Cast<DependencyObject>().ForEach(item => SetCommandTarget(item, NodeMenu));
		NodeTemplate.ContextMenu = NodeMenu;
	}

	protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
	{
		base.OnPreviewMouseWheel(e);

		if (!Keyboard.IsKeyDown(Key.LeftCtrl)) return;

		if (e.Delta < 0) Ratio -= 0.05;
		else Ratio += 0.05;

		e.Handled = true;
	}

	protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed)
		{
			_original = new Point(ScrollOffset.X, ScrollOffset.Y);
			_mouseOffset = e.GetPosition(this);
		}
	}

	protected override void OnPreviewMouseMove(MouseEventArgs e)
	{
		if (e.LeftButton == MouseButtonState.Pressed)
		{
			_isDragging = true;
			Cursor = Cursors.SizeAll;

			var offset = _mouseOffset - e.GetPosition(this);
			this.ScrollOffset = new Vector(
				_original.X + offset.X,
				_original.Y + offset.Y);
			this.InvalidateVisual();
		}
	}

	protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
	{
		if (_isDragging)
		{
			_isDragging = false;
			e.Handled = true;

			Cursor = Cursors.Arrow;
		}
	}

	protected override Rect ArrangeChild(UIElement child, Size constraint)
	{
		var size = CellSize;

		var column = BnsCustomColumnListWidget.GetColumn(child);
		var row = BnsCustomColumnListWidget.GetRow(child);

		if (column >= 0)
		{
			double x = column * size.Width + ((size.Width - child.DesiredSize.Width) / 2) - ScrollOffset.X;
			double y = row * size.Height + ((size.Height - child.DesiredSize.Height) / 2) - ScrollOffset.Y;
			return new Rect(new Point(x, y), child.DesiredSize);
		}

		return base.ArrangeChild(child, constraint);
	}

	protected override void OnRender(DrawingContext dc)
	{
		base.OnRender(dc);

		if (this.ShowLines)
		{
			var size = CellSize;
			var DefaultBorder = Color.FromArgb(52, 0, 255, 110);

			int MaxColumn = 0, MaxRow = 0;
			foreach (UIElement child in Children)
			{
				if (child == null) continue;

				MaxRow = Math.Max(MaxRow, BnsCustomColumnListWidget.GetRow(child) + 1);
				MaxColumn = Math.Max(MaxColumn, BnsCustomColumnListWidget.GetColumn(child) + 1);
			}

			for (int column = 0; column < MaxColumn; column++)
			{
				for (int row = 0; row < MaxRow; row++)
				{
					double x = column * size.Width - ScrollOffset.X;
					double y = row * size.Height - ScrollOffset.Y;

					var rect = new Rect(x, y, size.Width, size.Height);
					dc.DrawRectangle(new SolidColorBrush(), pen: new Pen(new SolidColorBrush(DefaultBorder), 1), rect);
					dc.DrawText(new FormattedText($"{row}-{column}",
						CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 10, Brushes.Black, 96),
						new Point(x, y));
				}
			}
		}
	}
	#endregion

	#region Public Methods
	public void Update(string? type, JobSeq job = JobSeq.JobNone)
	{
		ArgumentNullException.ThrowIfNull(NodeTemplate);

		Starting = Destination = null;
		this.Children.Clear();

		#region Data
		var table = Globals.GameData.Provider.GetTable<ItemGraph>();
		var seeds = table.Where(record => record is ItemGraph.Seed seed).Cast<ItemGraph.Seed>();

		var seq = type.ToEnum<EquipType>();
		if (seq != EquipType.None) seeds = seeds.Where(x => x.ItemEquipType == seq);
		else if (type == "EnchantGem1") seeds = seeds.Where(x => x.SeedItem.FirstOrDefault().Value is Item.Gem gem && gem.WeaponEnchantGemSlotType == WeaponEnchantGemSlotTypeSeq.First);
		else if (type == "EnchantGem2") seeds = seeds.Where(x => x.SeedItem.FirstOrDefault().Value is Item.Gem gem && gem.WeaponEnchantGemSlotType == WeaponEnchantGemSlotTypeSeq.Second);
		else if (type == "AccessoryGem") seeds = seeds.Where(x => x.SeedItem.FirstOrDefault().Value is Item.Gem gem && gem.AccessoryEnchantGemEquipAccessoryType != AccessoryEnchantGemEquipAccessoryTypeSeq.None);
		else return;

		if (!seeds.Any()) return;
		#endregion

		#region Node	
		var groups = seeds.Select(record => record.SeedItemGroup.Value).Where(x => x != null).Reverse().Distinct();
		var items = new Dictionary<Item, BnsCustomImageWidget>();

		foreach (var seed in seeds)
		{
			// valid data
			var SeedItems = seed.SeedItem.Select(x => x.Value).Where(x => x != null).ToArray();
			var item = SeedItems.FirstOrDefault(x => x.EquipJobCheck.CheckSeq(job)) ?? SeedItems.FirstOrDefault();
			if (item is null) continue;

			#region Widget
			var widget = NodeTemplate.Clone();
			widget.DataContext = item;
			widget.ContextMenu = NodeTemplate.ContextMenu;
			widget.ToolTip = new BnsTooltipHolder();

			BnsCustomColumnListWidget.SetRow(widget, seed.Row);
			BnsCustomColumnListWidget.SetColumn(widget, seed.Column);
			#endregion

			#region Expansion
			widget.ExpansionComponentList["Node_SubGroupImage"]!.SetExpansionShow(SeedItems.Length > 1);
			widget.ExpansionComponentList["Node_Icon"]!.SetValue(item.FrontIcon?.GetImage() with
			{
				HorizontalAlignment = EHorizontalAlignment.HAlign_Center,
				VerticalAlignment = EVerticalAlignment.VAlign_Top,
				EnableResourceSize = true,
				ImageScale = 0.625f,
				Offset = new(0, 5),
			});
			widget.ExpansionComponentList["Node_ItemName"]!.SetValue(item.ItemName);
			widget.ExpansionComponentList["Node_ViaImage"]!.SetExpansionShow(false);
			widget.ExpansionComponentList["Node_PurposeImage"]!.SetExpansionShow(false);
			widget.ExpansionComponentList["Node_StartImage"]!.SetExpansionShow(false);
			widget.ExpansionComponentList["Node_EquipedImage"]!.SetExpansionShow(false);
			widget.ExpansionComponentList["Node_OverImage"]!.SetExpansionShow(false);
			widget.ExpansionComponentList["Node_OverImage2"]!.SetExpansionShow(false);
			widget.ExpansionComponentList["Node_PressedImage"]!.SetExpansionShow(false);
			widget.ExpansionComponentList["Node_SearchedImage"]!.SetExpansionShow(false);
			widget.ExpansionComponentList["Node_PossessionImage"]!.SetExpansionShow(false);
			#endregion

			this.Children.Add(items[item] = widget);
		}
		#endregion

		#region Edge
		foreach (var edge in table.OfType<ItemGraph.Edge>())
		{
			if (!edge.StartItem.HasValue || !items.TryGetValue(edge.StartItem, out var StartItem)) continue;
			if (!edge.EndItem.HasValue || !items.TryGetValue(edge.EndItem, out var EndItem)) continue;

			// valid main ingredient
			var MainIngredient = edge.FeedRecipe.Value?.MainIngredient.Value;
			if (MainIngredient is Item item && !item.EquipJobCheck.CheckSeq(job)) continue;

			this.Children.Add(new Edge(edge, StartItem, EndItem));
		}
		#endregion
	}

	/// <summary>
	/// Search for the specified item and move to it if exist
	/// </summary>
	public void Search(Item item)
	{

	}
	#endregion

	#region Private Helpers
	// Widget
	private static void SetRatio(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		// get final scale 
		var widget = (BnsCustomGraphMapWidget)d;
		widget.Ratio = Math.Max(widget.MinZoomRatio, Math.Min(widget.MaxZoomRatio, widget.Ratio));
		widget.LayoutTransform = new ScaleTransform() { ScaleX = widget.Ratio, ScaleY = widget.Ratio };
	}

	private void FindPath()
	{
		if (Starting?.DataContext is not Item start ||
		 Destination?.DataContext is not Item dest) return;

		// Create lookup
		var edges = Children.OfType<Edge>().ToLookup(seed => seed.Data.EndItem.Value);

		ConcurrentBag<Edge[]> routes = [];
		Test(start, dest, 0, []);

		void Test(Item start, Item dest, PriorityMode mode, Edge[] route)
		{
			// For same item, means that is different recipe 
			// select one of according the user configuration
			var paths = edges[dest].GroupBy(x => x.Data.StartItem).Select(paths =>
			{
				Edge? edge = null;

				if (mode == PriorityMode.Definite)
				{
					edge = paths.FirstOrDefault(o => o.Data.SuccessProbability == SuccessProbabilitySeq.Definite);
				}

				return edge ?? paths.First();
			});

			foreach (var edge in paths)
			{
				// check current step
				if (!edges.Contains(dest)) continue;

				// create route copy
				var _route = new Edge[route.Length + 1];
				Array.Copy(route, 0, _route, 0, route.Length);
				_route[^1] = edge;

				// find next step
				var _dest = edge.Data.StartItem.Value;
				if (start.Equals(_dest)) routes.Add(_route);
				else Test(start, _dest, mode, _route);
			}
		}

		RoutesChanged?.Invoke(this, routes.Select(x => new ItemGraphRouteHelper(x)));
	}

	// Edge
	public sealed class Edge : Shape
	{
		#region Fields
		private static readonly Brush DefiniteBrush = Brushes.Green;
		private static readonly Brush StochasticBrush = Brushes.Blue;
		public const double PinOffset = 8;
		#endregion

		#region Properties
		public ItemGraph.Edge Data { get; private set; }

		protected override Geometry DefiningGeometry => Path;

		public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path",
			typeof(Geometry), typeof(Edge), new FrameworkPropertyMetadata(Geometry.Empty,
				FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

		public Geometry Path
		{
			get { return (Geometry)GetValue(PathProperty); }
			set { SetValue(PathProperty, value); }
		}

		public static readonly DependencyProperty HighlightProperty = DependencyProperty.Register("Highlight",
			typeof(bool), typeof(Edge), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox,
				FrameworkPropertyMetadataOptions.AffectsRender));

		public bool Highlight
		{
			get { return (bool)GetValue(HighlightProperty); }
			set { SetValue(HighlightProperty, value); }
		}
		#endregion

		#region Methods
		public Edge(ItemGraph.Edge edge, BnsCustomImageWidget start, BnsCustomImageWidget end)
		{
			ToolTip = new BnsTooltipHolder();
			DataContext = Data = edge;
			StrokeThickness = 2.5;
			Stroke = edge.SuccessProbability == SuccessProbabilitySeq.Definite ? DefiniteBrush : StochasticBrush;

			// Set direction
			var dock1 = edge.StartOrientation == OrientationSeq.Horizontal ? Dock.Right : Dock.Top;
			if (edge.EdgeType == EdgeTypeSeq.JumpTransform) dock1 = (Dock)4;
			var dock2 = edge.EndOrientation == OrientationSeq.Horizontal ? Dock.Left : Dock.Bottom;

			start.Loaded += (_, _) => OnLoaded(start, end, dock1, dock2);
		}

		private void OnLoaded(BnsCustomImageWidget node1, BnsCustomImageWidget node2, Dock dock1, Dock dock2)
		{
			// Compute pos
			var sour = node1.PinPoint(dock1);
			var dest = node2.PinPoint(dock2);

			// Create path point
			var points = new List<Point>();
			var current = sour;

			if (dock1 == (Dock)4)
			{
				points.Add(new Point(sour.X - 50, sour.Y));
				points.Add(new Point(sour.X - 50, dest.Y));
			}
			else if (dock1 == Dock.Left || dock1 == Dock.Right) points.Add(current = current with { X = (sour.X + dest.X) / 2 });
			else points.Add(current = current with { Y = (sour.Y + dest.Y) / 2 });

			if (dock2 == Dock.Left || dock2 == Dock.Right) points.Add(current = current with { Y = dest.Y });
			else points.Add(current = current with { X = dest.X });

			points.Add(dest);

			// Update data
			var figure = new PathFigure(sour, points.Select(x => new LineSegment(x, true)), false);
			Path = new PathGeometry([figure]);
		}
		#endregion
	}

	private void SetRoute(object sender, RoutedEventArgs e)
	{
		var menu = ((MenuItem)sender).Parent as ContextMenu;
		var item = menu.PlacementTarget as Edge;

		SetRouteThrough?.Invoke(this, item);
	}
	#endregion

	#region Private Fields
	bool _isDragging;
	Point _mouseOffset;
	Point _original;
	readonly TranslateTransform Transform;

	private BnsCustomImageWidget? NodeTemplate;
	private BnsCustomImageWidget? HorizontalRulerItemTemplate;
	private BnsCustomImageWidget? VerticalRulerItemTemplate;

	private BnsCustomImageWidget? Starting;
	private BnsCustomImageWidget? Destination;
	#endregion
}


#region Helper
public class ItemGraphRouteHelper(BnsCustomGraphMapWidget.Edge[] route)
{
	public BnsCustomGraphMapWidget.Edge[] Edges = route;

	public void SwitchHighlight(bool status)
	{
		Edges.ForEach(x =>
		{
			x.Highlight = status;
		});
	}

	public override string ToString() => Edges.Aggregate("", (a, n) => n.Data.StartItem.Value.Name + " -> ") + Edges.FirstOrDefault()?.Data.EndItem.Value.Name;

	public IReadOnlyDictionary<Item, int> Ingredients
	{
		get
		{
			var items = new Dictionary<Item, int>();
			void AddItem(Item item, int count)
			{
				if (item is null) return;

				items.TryAdd(item, 0);
				items[item] += count;
			}

			foreach (var edge in Edges)
			{
				var recipe = edge.Data.Recipe;
				if (recipe is null) continue;

				AddItem(recipe.MainItem, recipe.MainItemCount);
				recipe.SubItemList?.ForEach(sub => AddItem(sub.Item1, sub.Item2));
			}

			return items;
		}
	}
}

public enum PriorityMode
{
	Definite,
	Inexpensive,
}
#endregion