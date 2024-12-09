using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Common.Interactivity;
internal class PreviewTalkMessage : RecordCommand
{
	protected override List<string> Type => [];

	protected override void Execute(Record record)
	{
		switch (record.OwnerName)
		{
			case "npctalkmessage":
				break;

			default: throw new NotSupportedException();
		}
	}
}