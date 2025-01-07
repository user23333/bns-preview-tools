using System.Reflection;
using System.Xml;
using Xylia.Preview.Common;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.Properties;

namespace Xylia.Preview.Data.Models;
public abstract class ModelElement : IElement, IArgument
{
	#region IElement
	public Record Source { get; private set; }

	public ElementType ElementType => ElementType.Element;

	public Ref PrimaryKey => Source.PrimaryKey;

	public AttributeCollection Attributes { get; private set; }

	public IDataProvider Provider => Source.Owner.Owner;
	#endregion

	#region Methods
	public override string ToString() => Source?.ToString();

	internal void Initialize(Record source)
	{
		// initialize data
		Source = source;
		Attributes = source.Attributes;

		// initialize property
		foreach (var prop in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
		{
			if (!prop.CanWrite) continue;

			var type = prop.PropertyType;
			var name = prop.GetAttribute<NameAttribute>()?.Name ?? prop.Name.TitleLowerCase();
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(LazyList<>))
			{
				var toType = typeof(List<>).MakeGenericType(type.GetGenericArguments()[0]);
				var value = Activator.CreateInstance(type, new Func<object>(() => AttributeConverter.Convert(source, name, toType)));

				prop.SetValue(this, value);
			}
			else
			{
				prop.SetValue(this, AttributeConverter.Convert(source, name, type));
			}
		}

		LoadHiddenField();
	}

	protected internal virtual void Load(XmlNode node, string tableDefName/*, AliasTable aliasTable*/)
	{
		//GameDataTableUtil.CheckAttribute(node, new string[]
		//  {
		//		"id",
		//		"alias",
		//		"zone"
		//  });
		//int num = Convert.ToInt32(node.Attributes["zone"].Value);
		//short num2 = Convert.ToInt16(node.Attributes["id"].Value);
		//string alias = AliasTable.MakeKey(tableDefName, node.Attributes["alias"].Value);
		//long area = GameDataTableUtil.LoadMultiKeyRef(node.Attributes["area"], "ZoneArea", aliasTable, new ZoneAreaDataRefGenerator());
		//ZoneRespawnData zoneRespawnData = new ZoneRespawnData(num, num2, alias, area);
		//if (this._table.ContainsKey(zoneRespawnData.Key))
		//{
		//	throw new Exception(string.Format("already contains key {0}.{1}.", num, num2));
		//}
		//base.checkAlias(zoneRespawnData.Key, alias, aliasTable);
		//this._table.Add(zoneRespawnData.Key, zoneRespawnData);
	}

	protected virtual void LoadHiddenField()
	{

	}

	bool IArgument.TryGet(string name, out object value)
	{
		if (Attributes.TryGetValue(name, out var pair))
		{
			value = pair.Value;

			if (value is Record record && record.OwnerName == "text")
				value = record.Attributes["text"];

			return true;
		}

		value = null;
		return false;
	}
	#endregion
}

public struct Ref<TElement> where TElement : ModelElement
{
	#region Constructors
	public Ref(Record value)
	{
		source = value;
	}

	public Ref(TElement value)
	{
		_value = value;
		source = value.Source;
	}

	internal Ref(string value)
	{
		// Prevent designer request to load data
		if (!Settings.Default.Text_LoadData && !Globals.GameData.IsInitialized) return;

		// get available provider
		var provider = Globals.GameData.Provider;

		// get source
		if (value is null) return;
		else if (value.Contains(':')) source = provider.Tables.GetRecord(value);
		else source = provider.Tables.GetRecord(typeof(TElement).Name, value);
	}
	#endregion

	#region Fields
	private readonly Record source;

	private TElement _value;
	public TElement Value => _value ??= source?.To<TElement>();
	#endregion

	#region	Methods
	public static implicit operator TElement(Ref<TElement> value) => value.Value;
	public static implicit operator Ref<TElement>(TElement value) => new(value);
	public static implicit operator Ref<TElement>(Record value) => new(value);

	public static bool operator ==(Ref<TElement> left, Ref<TElement> right) => left.Equals(right);
	public static bool operator !=(Ref<TElement> left, Ref<TElement> right) => !(left == right);

	public override readonly int GetHashCode() => source?.GetHashCode() ?? 0;
	public override readonly bool Equals(object obj) => obj is Ref<TElement> other && this.source == other.source;

	public readonly bool HasValue => source != null;

	public override string ToString() => Value?.ToString();
	#endregion
}