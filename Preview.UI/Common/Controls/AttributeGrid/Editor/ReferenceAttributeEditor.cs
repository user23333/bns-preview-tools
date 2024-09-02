using System.Windows;
using System.Windows.Data;
using HandyControl.Controls;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.UI.Controls;
internal class ReferenceAttributeEditor(string? reference, IDataProvider provider) : PropertyEditorBase
{
	#region Constructors
	protected IDataProvider Provider { get; } = provider;

	protected Table? ReferedTable { get; } = provider.Tables[reference];
	#endregion

	#region Methods
	public override FrameworkElement CreateElement(PropertyItem propertyItem) => new ReferenceBox
	{
		IsReadOnly = propertyItem.IsReadOnly
	};

	//public override FrameworkElement CreateElement(PropertyItem propertyItem)
	//{
	//	var element = new AutoCompleteTextBox
	//	{
	//		IsEnabled = !propertyItem.IsReadOnly,
	//	};

	//	// set source
	//	BindingOperations.SetBinding(element, ItemsControl.ItemsSourceProperty,
	//		new Binding("Records")
	//		{
	//			Source = ReferedTable,
	//			Mode = BindingMode.OneWay,
	//			UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
	//			IsAsync = true,
	//			Delay = 100,
	//		});

	//	// set tooltip
	//	BindingOperations.SetBinding(element, Selector.ToolTipProperty,
	//		new Binding("SelectedItem")
	//		{
	//			Source = element,
	//			Mode = BindingMode.OneWay, 
	//			Converter = new RecordNameConverter()
	//		});

	//	return element;
	//}

	public override DependencyProperty GetDependencyProperty() => FrameworkElement.DataContextProperty;

	public override UpdateSourceTrigger GetUpdateSourceTrigger(PropertyItem propertyItem) => UpdateSourceTrigger.LostFocus;
	#endregion
}