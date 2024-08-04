namespace Xylia.Preview.Data.Models.Document;
public class Link : HtmlElementNode
{
    #region Fields
    public bool IgnoreInput { get => GetAttributeValue<bool>(); set => SetAttributeValue(value); }

    public bool Editable { get => GetAttributeValue<bool>(); set => SetAttributeValue(value); }

    public string Id { get => GetAttributeValue<string>(); set => SetAttributeValue(value); }
	#endregion
}