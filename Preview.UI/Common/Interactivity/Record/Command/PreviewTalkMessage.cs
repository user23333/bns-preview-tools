using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Views;

namespace Xylia.Preview.UI.Common.Interactivity;
public class PreviewTalkMessage : RecordCommand
{
	protected override List<string> Type => ["npctalkmessage"];

	protected override void Execute(Record record)
	{
		switch (record.OwnerName)
		{
			case "npctalkmessage":
				var message = record.To<NpcTalkMessage>();
				var show = message.StepShow[0].LoadObject<UShowObject>();
				if (show != null) Application.Current.Dispatcher.Invoke(() => new ShowObjectPlayer(show).Show());
				break;

			default: throw new NotSupportedException();
		}
	}
}