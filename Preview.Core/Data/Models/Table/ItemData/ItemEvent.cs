using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed class ItemEvent : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public Time64 EventExpirationTime { get; set; }

	public Ref<Text> Name2 { get; set; }
	#endregion

	#region Methods
	public bool IsExpiration => EventExpirationTime < DateTime.Now;
	#endregion
}