using System.Xml;

namespace Xylia.Preview.Common.Extension;
public static class XmlExtension
{
	/// <summary>
	/// Returns the value for the attribute with the specified name.
	/// </summary>
	/// <param name="name">The name of the attribute to retrieve. </param>
	/// <param name="def">The default value to return if not found.</param>
	/// <returns>The value of the specified attribute. An empty string is returned if a matching attribute is not found or if the attribute does not have a specified or default value.</returns>
	public static T GetAttribute<T>(this XmlElement element, string name, T def = default)
	{
		try
		{	
			var value = element.GetAttribute(name);
			if (string.IsNullOrEmpty(value)) return def;

			return value.As<T>();
		}
		catch
		{
			return default;
		}
	}
}