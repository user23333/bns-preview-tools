namespace Xylia.Preview.Data.Models.Document;
public class Replace : HtmlElementNode
{
    public string P { get => GetAttributeValue<string>(); set => SetAttributeValue(value); }
}