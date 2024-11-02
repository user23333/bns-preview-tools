namespace Xylia.Preview.UI.Common;
public class NameObject<T>(T? value, string text)
{
    public T? Value { get; } = value;

	public string Name { get; } = text;

	internal bool Flag { get; set; }
}