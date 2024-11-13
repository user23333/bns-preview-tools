namespace Xylia.Preview.Data.Common.Abstractions;
public interface ITextProvider
{
	string this[string key] { get; }

	/// <summary>
	/// Gets text and replaces the format item in a specified string with the string representation of a corresponding object in a specified array.
	/// </summary>
	/// <param name="key">Target text resource key</param>
	/// <param name="args">An object array that contains zero or more objects to format.</param>
	/// <returns></returns>
	public string Get(string key, params object[] args)
	{
		if (this[key] is string s)
		{
			return string.Format(s, args);
		}

		return key;
	}
}