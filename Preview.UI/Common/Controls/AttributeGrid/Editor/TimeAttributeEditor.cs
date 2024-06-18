using System.Globalization;
using System.Windows;
using System.Windows.Data;
using HandyControl.Controls;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.UI.Controls;
internal class TimeAttributeEditor : PropertyEditorBase, IValueConverter
{
	#region Methods
	public override FrameworkElement CreateElement(PropertyItem propertyItem) => new DateTimePicker
	{
		IsEnabled = !propertyItem.IsReadOnly
	};

	public override DependencyProperty GetDependencyProperty() => DateTimePicker.SelectedDateTimeProperty;

	protected override IValueConverter GetConverter(PropertyItem propertyItem) => this;

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value switch
		{
			Time64 time => DateTime.Parse(time.ToString()),
			TimeUniversal time => DateTime.Parse(time.ToString()),
			_ => throw new NotImplementedException(),
		};
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is DateTime time)
		{
			if (targetType == typeof(Time64)) return (Time64)time;
			if (targetType == typeof(TimeUniversal)) return (TimeUniversal)time;
		}

		throw new NotImplementedException();
	}
	#endregion
}