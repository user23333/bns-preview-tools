using System.Windows;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.UI.Documents;
public class Timer : BaseElement<Data.Models.Document.Timer>
{
	#region Methods
	protected override Size MeasureCore(Size availableSize)
	{
		// HACK: 
		this.Children = [new Run() { Text = Element!.GetText(Timers) }];

		return base.MeasureCore(availableSize);
	}

	public static bool Valid(DayOfWeek DayOfWeek, int ResetTime, out Time64 Time)
	{
		var now = DateTime.Now;

		int days = DayOfWeek - now.DayOfWeek;
		if (days < 0) days += 7;

		// reset hour not arrived
		if (days == 6 && now.Hour < ResetTime) days -= 7;

		// get time range
		var startTime = now.Date.AddDays(days).AddHours(ResetTime);
		var endTime = startTime.AddDays(1);


		bool status = startTime <= now && endTime > now;
		Time = (Time64)(status ? endTime : startTime);
		return status;
	}
	#endregion
}