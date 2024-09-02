using System.Windows;
using System.Windows.Input;
using HandyControl.Interactivity;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Common.Interactivity;

namespace Xylia.Preview.UI.Views.Editor;
public partial class AttributeEditor
{
	public AttributeEditor()
	{
		InitializeComponent();
		RegisterCommands(this.CommandBindings);
	}

	#region Methods
	public Record Source
	{
		get => attributeGrid.SelectedObject;
		set => attributeGrid.SelectedObject = value;
	}

	private void RegisterCommands(CommandBindingCollection commandBindings)
	{
		commandBindings.Add(new CommandBinding(ControlCommands.More, ViewSourceCommand));
	}

	private void ViewSourceCommand(object sender, RoutedEventArgs e)
	{
		PreviewBasic.Execute(Source, true);
	}
	#endregion
}