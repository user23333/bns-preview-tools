using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HandyControl.Controls;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Controls.Helpers;
using Xylia.Preview.UI.Views.Editor;

namespace Xylia.Preview.UI.Controls;
[TemplatePart(Name = ElementRoot, Type = typeof(Grid))]
[TemplatePart(Name = ElementTextBox, Type = typeof(WatermarkTextBox))]
[TemplatePart(Name = ElementButton, Type = typeof(Button))]
public partial class ReferenceBox : Control
{
	#region Fields
	private const string ElementRoot = "PART_Root";
	private const string ElementTextBox = "PART_TextBox";
	private const string ElementButton = "PART_Button";

	private WatermarkTextBox? _textBox;
	private Button? _button;
	#endregion 

	#region Constructor
	public ReferenceBox()
	{
		CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenCommand, CanExecuteOpen));
	}
	#endregion

	#region Properties
	public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(ReferenceBox));

	public bool IsReadOnly
	{
		get => (bool)GetValue(IsReadOnlyProperty);
		set => SetValue(IsReadOnlyProperty, BooleanBoxes.Box(value));
	}
	#endregion

	#region Methods
	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();

		_textBox = GetTemplateChild(ElementTextBox) as WatermarkTextBox;
		_button = GetTemplateChild(ElementButton) as Button;

		if (_textBox != null)
		{
			_textBox.Text = DataContext?.ToString();
		}
	}

	private void CanExecuteOpen(object sender, CanExecuteRoutedEventArgs e)
	{
		e.CanExecute = DataContext is Record;
	}

	private void OpenCommand(object sender, RoutedEventArgs e)
	{
		if (DataContext is Record record)
		{
			new PropertyEditor() { Source = record }.Show();
		}
	}
	#endregion
}