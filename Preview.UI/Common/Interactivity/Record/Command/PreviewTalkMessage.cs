using System.Windows;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.GameUI.Scene.Game_NpcTalk;

namespace Xylia.Preview.UI.Common.Interactivity;
internal class PreviewTalkMessage : RecordCommand
{
	protected override List<string> Type => ["npcresponse", "npctalkmessage"];

	protected override void Execute(Record record)
	{
		switch (record.OwnerName)
		{
			case "npcresponse":
				var talkmessage = record.To<NpcResponse>().TalkMessage;
				if (!talkmessage.HasValue) throw new Exception(StringHelper.Get("Exception_InvalidTalkMessage"));

				Application.Current.Dispatcher.Invoke(() => new NpcTalkPanel() { DataContext = talkmessage.Value }.Show());
				break;

			case "npctalkmessage":
				Application.Current.Dispatcher.Invoke(() => new NpcTalkPanel() { DataContext = record.To<NpcTalkMessage>() }.Show());
				break;

			default: throw new NotSupportedException();
		}
	}
}