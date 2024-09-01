﻿using System.Reflection;
using System.Xml;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Properties;

namespace Xylia.Preview.Data.Models;
public abstract class ModelElement : IElement
{
	#region IElement
	public Record Source { get; private set; }

	public ElementType ElementType => ElementType.Element;

	public Ref PrimaryKey => Source.PrimaryKey;

	public AttributeCollection Attributes => Source.Attributes;

	protected IDataProvider Provider => Source.Owner.Owner;
	#endregion

	#region Methods
	public override string ToString() => Source.ToString();

	internal void Initialize(Record source)
	{
		this.Source = source;

		foreach (var prop in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
		{
			if (!prop.CanWrite) continue;

			var type = prop.PropertyType;
			var name = (prop.GetAttribute<NameAttribute>()?.Name ?? prop.Name).TitleLowerCase();
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(LazyList<>))
			{
				var toType = typeof(List<>).MakeGenericType(type.GetGenericArguments()[0]);
				var value = Activator.CreateInstance(type, new Func<object>(() => Convert(source, name, toType)));

				prop.SetValue(this, value);
			}
			else
			{
				prop.SetValue(this, Convert(source, name, type));
			}
		}

		LoadHiddenField();
	}

	private static object Convert(Record record, string name, Type type)
	{
		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
		{
			// valid
			var recordType = type.GetGenericArguments()[0];
			if (!record.Children.TryGetValue(name, out var children)) throw new InvalidDataException($"No `{name}` child element in definition");
			if (!typeof(ModelElement).IsAssignableFrom(recordType)) throw new InvalidCastException($"{recordType} unable cast to {typeof(ModelElement)}");

			// create instance
			var records = Activator.CreateInstance(type);
			var add = records.GetType().GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			children.ForEach(child => add.Invoke(records, [child.As(recordType)]));

			return records;
		}

		// attribute
		var attribute = record.Definition.GetAttribute(name);
		if (attribute is null) return null;
		else if (attribute.Repeat == 1)
		{
			return AttributeConverter.Convert(name, record.Attributes[name], type);
		}
		else if (!type.IsArray)
		{
			throw new Exception($"Repeatable object must to use array type: {record.Name} -> {name}");
		}
		else
		{
			type = type.GetElementType();
			var value = Array.CreateInstance(type, attribute.Repeat);

			for (int i = 0; i < value.Length; i++)
				value.SetValue(AttributeConverter.Convert(name, record.Attributes[$"{name}-{i + 1}"], type), i);

			return value;
		}
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
		_instance = value;
		source = value.Source;
	}

	[Obsolete("Not recommended to use the constructor")]
	public Ref(string value)
	{
		// Prevent designer request to load data
		if (!Settings.Default.Text_LoadData && !FileCache.Data.IsInitialized) return;

		// get available provider
		var provider = FileCache.Data.Provider;

		// get source
		if (value is null) return;
		else if (value.Contains(':')) source = provider.Tables.GetRecord(value);
		else source = provider.Tables.GetRecord(typeof(TElement).Name, value);
	}
	#endregion


	#region	Instance
	private readonly Record source;

	private TElement _instance;
	public TElement Instance => _instance ??= source?.As<TElement>();


	public static implicit operator TElement(Ref<TElement> value) => value.Instance;

	public static implicit operator Ref<TElement>(TElement value) => new(value);
	public static implicit operator Ref<TElement>(Record value) => new(value);

	public static bool operator ==(Ref<TElement> left, Ref<TElement> right) => left.Equals(right);
	public static bool operator !=(Ref<TElement> left, Ref<TElement> right) => !(left == right);


	public readonly bool HasValue => source != null;

	public override readonly int GetHashCode() => source?.GetHashCode() ?? 0;

	public override readonly bool Equals(object obj) => obj is Ref<TElement> other && this.source == other.source;

	public override string ToString() => Instance?.ToString();
	#endregion
}