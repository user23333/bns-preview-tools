namespace Xylia.Preview.UI.Common;
public class NameObject<T>(string text, T? value)
{
	public T? Value { get; } = value;

	public string Name { get; } = text;

	internal bool Flag { get; set; }
}