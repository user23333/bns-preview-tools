using System.ComponentModel;
using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Views;

namespace Xylia.Preview.UI.Common.Interactivity;
internal class PreviewShowObject : RecordCommand
{
	protected override List<string> Type => ["social"];

	protected override bool CanExecute(Record record)
	{
		if (record.OwnerName == "social")
		{
			if (record.Attributes["show"] != null) return true;
		}

		return false;
	}

	protected override void Execute(Record record)
	{
		switch (record.OwnerName)
		{
			case "social":
			{
				var source = FileCache.Provider.LoadObject<UShowObject>(record.Attributes["show"]?.ToString());
				if (source is null) throw new WarningException("no data"); 

				Application.Current.Dispatcher.Invoke(() => new ShowObjectPlayer(source).Show());
			}
			break;

			default: throw new NotSupportedException();
		}
	}
}