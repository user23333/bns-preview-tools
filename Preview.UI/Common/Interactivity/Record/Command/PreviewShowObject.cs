using System.Diagnostics;
using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Views;

namespace Xylia.Preview.UI.Common.Interactivity;
internal class PreviewShowObject : RecordCommand
{
	protected override List<string> Type => ["social"];

	protected override void Execute(Record record)
	{
		switch (record.OwnerName)
		{
			case "social":
			{
				var source = FileCache.Provider.LoadObject<UShowObject>(record.Attributes["show"]?.ToString());
				if (source is null)
				{
					Debug.WriteLine("no data");
					return;
				}

				Application.Current.Dispatcher.Invoke(() =>
				{
					new ShowObjectPlayer { Source = source }.Show();
				});
			}
			break;

			default: throw new NotSupportedException();
		}
	}
}