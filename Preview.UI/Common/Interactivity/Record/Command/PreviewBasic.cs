using System.Text;
using System.Windows;
using Xylia.Preview.Data.Engine.BinData.Serialization;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Views.Editor;

namespace Xylia.Preview.UI.Common.Interactivity;
/// <summary>
///  Provide a command to show record attributes
/// </summary>
public class PreviewBasic : RecordCommand
{
	protected override List<string>? Type => null;

	protected override void Execute(Record record) => Execute(record, false);

	public static void Execute(Record record, bool mode)
	{
		Application.Current.Dispatcher.Invoke(() =>
		{
			// Warning: is not original text
			if (record.HasChildren || mode)
			{
				var settings = new TableWriterSettings() { Encoding = Encoding.UTF8 };
				new TextEditor
				{
					Data = record.HasChildren ? null : record.Data,
					Text = settings.Encoding.GetString(record.Owner.WriteXml(settings, record)),
				}.Show();
			}
			else
			{
				new AttributeEditor() { Source = record }.Show();
			}
		});
	}
}