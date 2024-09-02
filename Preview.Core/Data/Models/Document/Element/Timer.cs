using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using static Xylia.Preview.Data.Common.DataStruct.MsecFormat;

namespace Xylia.Preview.Data.Models.Document;
public class Timer : HtmlElementNode
{
    #region Fields
    public int Id { get => GetAttributeValue<int>(); set => SetAttributeValue(value); }

    public MsecFormatType Type { get => GetAttributeValue<MsecFormatType>(); set => SetAttributeValue(value); }
	#endregion

	#region Methods
	public string GetText(IDictionary<int, Time64> timers)
	{
		var span = timers.GetOrDefault(Id) - DateTime.Now;
		return span.ToString(Type);
	}
	#endregion
}