using System.ComponentModel;
using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Data.Common.DataStruct;
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
			return record.Attributes.Get<ObjectPath>("show").IsValid;
		}

		return false;
	}

	protected override void Execute(Record record)
	{
		switch (record.OwnerName)
		{
			case "social":
			{
				var source = record.Attributes.Get<ObjectPath>("show").LoadObject<UShowObject>() ?? throw new WarningException(StringHelper.Get("Exception_InvalidData"));
				Application.Current.Dispatcher.Invoke(() => new ShowObjectPlayer(source).Show());
			}
			break;

			default: throw new NotSupportedException();
		}
	}
}