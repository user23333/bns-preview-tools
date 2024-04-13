using System.Windows;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.GameUI.Scene.Game_ItemGrowth2;

namespace Xylia.Preview.UI.Common.Interactivity;
public class PreviewTransform : RecordCommand
{
	protected override List<string> Type => ["item"];

	protected override bool CanExecute(Record record)
	{
		if (record.OwnerName == "item" && record.Attributes.Get<int>("improve-id") != 0)
			return true;

		return false;
	}

	protected override void Execute(Record record)
	{
		switch (record.OwnerName)
		{
			case "item":
				Application.Current.Dispatcher.Invoke(() => { new ItemGrowth2TooltipPanel { DataContext = record }.Show(); });
				break;

			default: throw new NotSupportedException();
		}
	}
}