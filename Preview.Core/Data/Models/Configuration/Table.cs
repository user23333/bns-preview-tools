using System.Xml;
using Xylia.Preview.Common.Extension;

namespace Xylia.Preview.Data.Models.Configuration;
public class Table : Element
{
	#region Attributes
	public ushort MajorVersion;
	public ushort MinorVersion;
	public string Moudle;
	public string ReleaseMoudle;
	public string ReleaseSide;
	#endregion

	#region Methods		
	internal override void Load(XmlElement element)
	{
		MajorVersion = element.GetAttribute<ushort>("major-version");
		MinorVersion = element.GetAttribute<ushort>("minor-version");
		Moudle = element.GetAttribute<string>("module");
		ReleaseMoudle = element.GetAttribute<string>("release-module");
		ReleaseSide = element.GetAttribute<string>("release-side");

		base.Load(element);
	}

	public static T LoadFrom<T>(Stream stream) where T : Table , new()
	{
		var doc = new XmlDocument();
		doc.Load(stream);

		var table = new T();
		table.Load(doc.DocumentElement);
		return table;
	}

	public static T LoadFrom<T>(string xml) where T : Table, new()
	{
		var doc = new XmlDocument();
		doc.LoadXml(xml);

		var table = new T();
		table.Load(doc.DocumentElement);   
		return table;
	}
	#endregion
}


public abstract class Element
{
	public string Name;
	public List<Element> Children = [];

	internal virtual void Load(XmlElement element)
	{
		Name = element.GetAttribute<string>("name");

		foreach (XmlElement child in element.ChildNodes)
		{
			Children.Add(child.Name switch
			{
				"option" => new Option(),
				"group" => new Group(),
				_ => throw new NotSupportedException(),
			});
		}
	}

	public Element this[string index] => Children.First(x => x.Name == index);

	public virtual string Value => null;
}

public class Group : Element
{
	internal override void Load(XmlElement element)
	{
		base.Load(element);


	}
}

public class Option : Element
{
	private string value;
	public override string Value => value;

	internal override void Load(XmlElement element)
	{
		Name = element.GetAttribute<string>("name");
		value = element.GetAttribute<string>("value");
	}
}