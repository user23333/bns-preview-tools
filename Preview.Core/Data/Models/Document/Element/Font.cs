namespace Xylia.Preview.Data.Models.Document;
public class Font : HtmlElementNode
{
    public string Name { get => GetAttributeValue<string>(); set => SetAttributeValue(value); }
}