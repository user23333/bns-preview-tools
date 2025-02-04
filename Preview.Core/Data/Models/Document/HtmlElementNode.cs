using System.Diagnostics;
using System.Reflection;

namespace Xylia.Preview.Data.Models.Document;
public abstract class HtmlElementNode : HtmlNode
{
	#region Constructors
	private static readonly Dictionary<string, Type> _classes = new(StringComparer.OrdinalIgnoreCase);

	static HtmlElementNode()
	{
		var baseType = typeof(HtmlElementNode);

		foreach (var definedType in Assembly.GetExecutingAssembly().GetTypes())
		{
			if (!definedType.IsAbstract &&
				!definedType.IsInterface &&
				baseType.IsAssignableFrom(definedType))
				_classes[definedType.Name.ToLower()] = definedType;
		}
	}

	protected HtmlElementNode() : base(HtmlNodeType.Element, new HtmlDocument(), 0)
	{

	}
	#endregion

	#region Methods
	internal static HtmlNode CreateNode(string name, HtmlDocument ownerdocument, int index)
	{
		if (_classes.TryGetValue(name, out var type))
		{
			var node = (HtmlElementNode)Activator.CreateInstance(type);
			node._ownerdocument = ownerdocument;
			node._outerstartindex = index;
			return node;
		}
		else
		{
			Debug.WriteLine("unknown tag: " + name);
			return new HtmlNode(HtmlNodeType.Element, ownerdocument, index);
		}
	}

	internal void Initialize()
	{
		foreach (var field in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
		{
			var value = GetAttributeValue(field.Name, field.FieldType, null);
			field.SetValue(this, value);
		}
	}
	#endregion
}