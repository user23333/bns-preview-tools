﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using HandyControl.Controls;
using SkiaSharp.Views.WPF;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Controls;
[TemplatePart(Name = ElementPopup, Type = typeof(ButtonBase))]
[TemplatePart(Name = ElementPopup, Type = typeof(Popup))]
public partial class IconPicker : AutoCompleteTextBox
{
	#region Constructors
	public IconPicker()
	{
		_image = new CropImage();
		_image.PreviewMouseRightButtonDown += DropDownButton_Click;
		BindingOperations.SetBinding(this, SelectedIndexProperty, new Binding(nameof(_image.SelectedIndex)) { Source = _image });
	}
	#endregion

	#region Fields
	private const string ElementButton = "PART_Button";
	private const string ElementPopup = "PART_Popup";

	private ButtonBase? _dropDownButton;
	private CropImage? _image;
	private Popup? _popup;
	#endregion

	#region Public Properties
	public static new readonly DependencyProperty SelectedIndexProperty = DependencyProperty.RegisterAttached("SelectedIndex",
		typeof(int), typeof(IconPicker), new PropertyMetadata(OnSelectedChanged));

	private static void OnSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		var picker = (IconPicker)d;
		picker.Text = string.Concat(picker.Text?.Split(',').First(), ",", e.NewValue);
	}
	#endregion

	#region Private Methods
	public override void OnApplyTemplate()
	{
		if (_popup != null)
		{
			//_popup.PreviewMouseLeftButtonDown -= PopupPreviewMouseLeftButtonDown;
			//_popup.Opened -= PopupOpened;
			//_popup.Closed -= PopupClosed;
			_popup.Child = null;
		}

		if (_dropDownButton != null)
		{
			//_dropDownButton.Click -= DropDownButton_Click;
			//_dropDownButton.MouseLeave -= DropDownButton_MouseLeave;
		}

		base.OnApplyTemplate();

		_popup = GetTemplateChild(ElementPopup) as Popup;
		_dropDownButton = GetTemplateChild(ElementButton) as Button;

		//_popup.PreviewMouseLeftButtonDown += PopupPreviewMouseLeftButtonDown;
		_popup.Opened += PopupOpened;
		_popup.Closed += PopupClosed;
		_popup.Child = _image;

		_dropDownButton.Click += DropDownButton_Click;
		//_dropDownButton.MouseLeave += DropDownButton_MouseLeave;
	}

	private void PopupPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if (sender is Popup { StaysOpen: false })
		{
			if (_dropDownButton?.InputHitTest(e.GetPosition(_dropDownButton)) != null)
			{

			}
		}
	}

	private void PopupOpened(object sender, EventArgs e)
	{
		//if (!IsDropDownOpen)
		//{
		//	SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.TrueBox);
		//}

		//_calendarWithClock?.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));

		//OnPickerOpened(new RoutedEventArgs());
	}

	private void PopupClosed(object sender, EventArgs e)
	{
		//if (IsDropDownOpen)
		//{
		//	SetCurrentValue(IsDropDownOpenProperty, ValueBoxes.FalseBox);
		//}

		//if (_calendarWithClock.IsKeyboardFocusWithin)
		//{
		//	MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
		//}

		//OnPickerClosed(new RoutedEventArgs());
	}

	private void DropDownButton_Click(object sender, RoutedEventArgs e) => TogglePopup();

	private void TogglePopup()
	{
		if (_popup.IsOpen)
		{
			_popup.SetCurrentValue(Popup.IsOpenProperty, false);
		}
		else
		{
			SetData();

			this.SetCurrentValue(IsDropDownOpenProperty, false);
			_popup.SetCurrentValue(Popup.IsOpenProperty, true);
		}
	}

	private void SetData()
	{
		string Text = this.Text?.Split(',').First();

		var record = Globals.GameData.Provider.GetTable<IconTexture>()[Text];
		if (record is null) return;

		_image.CellSize = new Size(record.IconWidth, record.IconHeight);
		_image.Source = record!.GetImage(0).Image?.ToWriteableBitmap();
	}
	#endregion
}