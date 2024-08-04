namespace Xylia.Preview.Data.Models.Document;
public class SelFont : HtmlElementNode
{
    #region Fields
    public string Name1 { get => GetAttributeValue<string>(); set => SetAttributeValue(value); }
    public string Name2 { get => GetAttributeValue<string>(); set => SetAttributeValue(value); }
    public string P { get => GetAttributeValue<string>(); set => SetAttributeValue(value); }
    #endregion
}