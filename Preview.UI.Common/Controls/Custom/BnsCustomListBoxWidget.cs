using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Xylia.Preview.UI.Controls.Helpers;
using Xylia.Preview.UI.Controls.Primitives;

namespace Xylia.Preview.UI.Controls;
public class BnsCustomListBoxWidget : BnsCustomSourceBaseWidget
{

}

public class BnsCustomListBoxItemWidget : UserWidget
{
	#region Constructors
	/// <summary>
	///     Default DependencyObject constructor
	/// </summary>
	public BnsCustomListBoxItemWidget() : base()
	{
	}

	static BnsCustomListBoxItemWidget()
	{
		DefaultStyleKeyProperty.OverrideMetadata(typeof(BnsCustomListBoxItemWidget), new FrameworkPropertyMetadata(typeof(BnsCustomListBoxItemWidget)));
		KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(BnsCustomListBoxItemWidget), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
		KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(BnsCustomListBoxItemWidget), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));

		IsEnabledProperty.OverrideMetadata(typeof(BnsCustomListBoxItemWidget), new UIPropertyMetadata(new PropertyChangedCallback(OnVisualStatePropertyChanged)));
		//IsMouseOverPropertyKey.OverrideMetadata(typeof(BnsCustomListBoxItemWidget), new UIPropertyMetadata(new PropertyChangedCallback(OnVisualStatePropertyChanged)));
		//Selector.IsSelectionActivePropertyKey.OverrideMetadata(typeof(BnsCustomListBoxItemWidget), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnVisualStatePropertyChanged)));
		AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(BnsCustomListBoxItemWidget), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
	}

	#endregion

	#region Public Properties

	/// <summary>
	///     Indicates whether this BnsCustomListBoxItemWidget is selected.
	/// </summary>
	public static readonly DependencyProperty IsSelectedProperty =
			Selector.IsSelectedProperty.AddOwner(typeof(BnsCustomListBoxItemWidget),
					new FrameworkPropertyMetadata(BooleanBoxes.FalseBox,
							FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
							new PropertyChangedCallback(OnIsSelectedChanged)));

	/// <summary>
	///     Indicates whether this BnsCustomListBoxItemWidget is selected.
	/// </summary>
	[Bindable(true), Category("Appearance")]
	public bool IsSelected
	{
		get { return (bool)GetValue(IsSelectedProperty); }
		set { SetValue(IsSelectedProperty, BooleanBoxes.Box(value)); }
	}

	private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		//BnsCustomListBoxItemWidget listItem = d as BnsCustomListBoxItemWidget;
		//bool isSelected = (bool)e.NewValue;

		//Selector parentSelector = listItem.ParentSelector;
		//if (parentSelector != null)
		//{
		//	parentSelector.RaiseIsSelectedChangedAutomationEvent(listItem, isSelected);
		//}

		//if (isSelected)
		//{
		//	listItem.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, listItem));
		//}
		//else
		//{
		//	listItem.OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, listItem));
		//}

		//listItem.UpdateVisualState();
	}

	/// <summary>
	///     Event indicating that the IsSelected property is now true.
	/// </summary>
	/// <param name="e">Event arguments</param>
	protected virtual void OnSelected(RoutedEventArgs e)
	{
		HandleIsSelectedChanged(true, e);
	}

	/// <summary>
	///     Event indicating that the IsSelected property is now false.
	/// </summary>
	/// <param name="e">Event arguments</param>
	protected virtual void OnUnselected(RoutedEventArgs e)
	{
		HandleIsSelectedChanged(false, e);
	}

	private void HandleIsSelectedChanged(bool newValue, RoutedEventArgs e)
	{
		RaiseEvent(e);
	}

	#endregion

	#region Events

	/// <summary>
	///     Raised when the item's IsSelected property becomes true.
	/// </summary>
	public static readonly RoutedEvent SelectedEvent = Selector.SelectedEvent.AddOwner(typeof(BnsCustomListBoxItemWidget));

	/// <summary>
	///     Raised when the item's IsSelected property becomes true.
	/// </summary>
	public event RoutedEventHandler Selected
	{
		add
		{
			AddHandler(SelectedEvent, value);
		}
		remove
		{
			RemoveHandler(SelectedEvent, value);
		}
	}

	/// <summary>
	///     Raised when the item's IsSelected property becomes false.
	/// </summary>
	public static readonly RoutedEvent UnselectedEvent = Selector.UnselectedEvent.AddOwner(typeof(BnsCustomListBoxItemWidget));

	/// <summary>
	///     Raised when the item's IsSelected property becomes false.
	/// </summary>
	public event RoutedEventHandler Unselected
	{
		add
		{
			AddHandler(UnselectedEvent, value);
		}
		remove
		{
			RemoveHandler(UnselectedEvent, value);
		}
	}

	#endregion

	#region Protected Methods

	internal override void ChangeVisualState(bool useTransitions)
	{
		// Change to the correct state in the Interaction group
		if (!IsEnabled)
		{
			// [copied from SL code]
			// If our child is a control then we depend on it displaying a proper "disabled" state.  If it is not a control
			// (ie TextBlock, Border, etc) then we will use our visuals to show a disabled state.
			VisualStateManager.GoToState(this, Children.Count > 0 ? VisualStates.StateNormal : VisualStates.StateDisabled, useTransitions);
		}
		else if (IsMouseOver)
		{
			VisualStateManager.GoToState(this, VisualStates.StateMouseOver, useTransitions);
		}
		else
		{
			VisualStateManager.GoToState(this, VisualStates.StateNormal, useTransitions);
		}

		// Change to the correct state in the Selection group
		if (IsSelected)
		{
			if (Selector.GetIsSelectionActive(this))
			{
				VisualStateManager.GoToState(this, VisualStates.StateSelected, useTransitions);
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, VisualStates.StateSelectedUnfocused, VisualStates.StateSelected);
			}
		}
		else
		{
			VisualStateManager.GoToState(this, VisualStates.StateUnselected, useTransitions);
		}

		if (IsKeyboardFocused)
		{
			VisualStateManager.GoToState(this, VisualStates.StateFocused, useTransitions);
		}
		else
		{
			VisualStateManager.GoToState(this, VisualStates.StateUnfocused, useTransitions);
		}

		base.ChangeVisualState(useTransitions);
	}

	/// <summary>
	/// Creates AutomationPeer (<see cref="UIElement.OnCreateAutomationPeer"/>)
	/// </summary>
	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return null;
		//return new BnsCustomListBoxItemWidgetWrapperAutomationPeer(this);
	}


	/// <summary>
	///     This is the method that responds to the MouseButtonEvent event.
	/// </summary>
	/// <param name="e">Event arguments</param>
	protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
	{
		if (!e.Handled)
		{
			// 
			e.Handled = true;
			HandleMouseButtonDown(MouseButton.Left);
		}
		base.OnMouseLeftButtonDown(e);
	}

	/// <summary>
	///     This is the method that responds to the MouseButtonEvent event.
	/// </summary>
	/// <param name="e">Event arguments</param>
	protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
	{
		if (!e.Handled)
		{
			// 
			e.Handled = true;
			HandleMouseButtonDown(MouseButton.Right);
		}
		base.OnMouseRightButtonDown(e);
	}

	private void HandleMouseButtonDown(MouseButton mouseButton)
	{
		//if (Selector.UiGetIsSelectable(this) && Focus())
		//{
		//	ListBox parent = ParentListBox;
		//	if (parent != null)
		//	{
		//		parent.NotifyListItemClicked(this, mouseButton);
		//	}
		//}
	}

	/// <summary>
	/// Called when IsMouseOver changes on this element.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnMouseEnter(MouseEventArgs e)
	{
		// abort any drag operation we have queued.
		if (parentNotifyDraggedOperation != null)
		{
			parentNotifyDraggedOperation.Abort();
			parentNotifyDraggedOperation = null;
		}

		if (IsMouseOver)
		{
			//var parent = ParentListBox;

			//if (parent != null && Mouse.LeftButton == MouseButtonState.Pressed)
			//{
			//	parent.NotifyListItemMouseDragged(this);
			//}
		}
		base.OnMouseEnter(e);
	}

	/// <summary>
	/// Called when IsMouseOver changes on this element.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnMouseLeave(MouseEventArgs e)
	{
		// abort any drag operation we have queued.
		if (parentNotifyDraggedOperation != null)
		{
			parentNotifyDraggedOperation.Abort();
			parentNotifyDraggedOperation = null;
		}

		base.OnMouseLeave(e);
	}

	/// <summary>
	/// Called when the visual parent of this element changes.
	/// </summary>
	/// <param name="oldParent"></param>
	protected override void OnVisualParentChanged(DependencyObject oldParent)
	{
		ItemsControl oldItemsControl = null;

		if (VisualTreeHelper.GetParent(this) == null)
		{
			if (IsKeyboardFocusWithin)
			{
				// This BnsCustomListBoxItemWidget had focus but was removed from the tree.
				// The normal behavior is for focus to become null, but we would rather that
				// focus go to the parent ListBox.

				// Use the oldParent to get a reference to the ListBox that this BnsCustomListBoxItemWidget used to be in.
				// The oldParent's ItemsOwner should be the ListBox.
				oldItemsControl = ItemsControl.GetItemsOwner(oldParent);
			}
		}

		base.OnVisualParentChanged(oldParent);

		// If earlier, we decided to set focus to the old parent ListBox, do it here
		// after calling base so that the state for IsKeyboardFocusWithin is updated correctly.
		if (oldItemsControl != null)
		{
			oldItemsControl.Focus();
		}
	}


	#endregion

	#region Private Fields

	DispatcherOperation parentNotifyDraggedOperation = null;

	#endregion
}