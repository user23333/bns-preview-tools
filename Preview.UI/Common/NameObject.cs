namespace Xylia.Preview.UI.Common;
public class NameObject<T>(string text, T? value)
{
	public T? Value { get; } = value;

	public string Name { get; } = text;

	public bool Flag { get; set; }
}