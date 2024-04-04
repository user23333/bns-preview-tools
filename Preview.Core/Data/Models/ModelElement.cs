using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.Definitions;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Properties;

namespace Xylia.Preview.Data.Models;
public abstract class ModelElement : IElement
{
	#region IElement
	public Record Source { get; private set; }

	public ElementType ElementType => Source.ElementType;

	public Ref PrimaryKey => Source.PrimaryKey;

	public AttributeCollection Attributes => Source.Attributes;
	#endregion


	#region Override Methods
	public override string ToString() => Source.ToString();

	public override int GetHashCode() => Source?.GetHashCode() ?? base.GetHashCode();

	public bool Equals(ModelElement other)
	{
		return other != null && this.Source == other.Source;
	}
	#endregion

	#region Methods
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

	protected internal virtual void LoadHiddenField()
	{

	}



	/// <summary>
	/// Convert original record to model instance
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source"></param>
	/// <param name="element"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	internal static T As<T>(Record source, T element) where T : ModelElement
	{
		element.Source = source;

		#region	instance
		foreach (var prop in element.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
		{
			if (!prop.CanWrite) continue;

			var type = prop.PropertyType;
			var name = (prop.GetAttribute<NameAttribute>()?.Name ?? prop.Name).TitleLowerCase();
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(LazyList<>))
			{
				var toType = typeof(List<>).MakeGenericType(type.GetGenericArguments()[0]);
				var value = Activator.CreateInstance(type, new Func<object>(() => Convert(element, name, toType)));

				prop.SetValue(element, value);
			}
			else
			{
				prop.SetValue(element, Convert(element, name, type));
			}
		}
		#endregion

		return element;
	}

	/// <summary>
	/// Convert Attribute to Field
	/// </summary>
	/// <param name="element"></param>
	/// <param name="name"></param>
	/// <param name="type"></param>
	/// <param name="repeat"></param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	private static object Convert(ModelElement element, string name, Type type)
	{
		var record = element.Source;

		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
		{
			// valid
			var recordType = type.GetGenericArguments()[0];
			if (!record.Children.TryGetValue(name, out var children)) throw new InvalidDataException($"No `{name}` child element in definition");
			if (!typeof(ModelElement).IsAssignableFrom(recordType)) throw new InvalidCastException($"{recordType} unable cast to {typeof(ModelElement)}");

			// create instance
			var records = Activator.CreateInstance(type);
			var add = records.GetType().GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			children.ForEach(child => add.Invoke(records, [child.As<ModelElement>(recordType)]));

			return records;
		}

		// attribute
		var attribute = record.Definition.GetAttribute(name);
		if (attribute is null || attribute.Repeat == 1)
		{
			return AttributeConverter.Convert(record.Attributes[name], type);
		}
		else if (!type.IsArray)
		{
			throw new Exception($"Repeatable object must to use array type: {element.GetType()} -> {name}");
		}
		else
		{
			type = type.GetElementType();
			var value = Array.CreateInstance(type, attribute.Repeat);

			for (int i = 0; i < attribute.Repeat; i++)
				value.SetValue(AttributeConverter.Convert(record.Attributes[$"{name}-{i + 1}"], type), i);

			return value;
		}
	}
	#endregion


	#region Helper
	internal class TypeHelper
	{
		#region Helper
		private static readonly MessageManager message = [];
		private static readonly Dictionary<Type, TypeHelper> helpers = [];

		public static TypeHelper Get(Type baseType, string name = null)
		{
			lock (helpers)
			{
				if (!helpers.TryGetValue(baseType, out var subs))
				{
					subs = helpers[baseType] = new TypeHelper();
					subs.GetSubType(baseType);
				}

				// Convert to real type
				if (baseType == typeof(ModelElement))
				{
					Debug.Assert(name != null);

					baseType = subs._subs[name];
					return Get(baseType);
				}

				return subs;
			}
		}


		private Type BaseType;
		private readonly Dictionary<string, Type> _subs = new(TableNameComparer.Instance);

		private void GetSubType(Type baseType)
		{
			var flag = baseType == typeof(ModelElement);
			this.BaseType = baseType;

			foreach (var instance in Assembly.GetExecutingAssembly().GetTypes())
			{
				if ((flag || !instance.IsAbstract) && baseType.IsAssignableFrom(instance) && instance != baseType)
					_subs[instance.Name.TitleLowerCase()] = instance;
			}
		}

		public T CreateInstance<T>(string type)
		{
			Type _type = null;

			if (!string.IsNullOrWhiteSpace(type) && !_subs.TryGetValue(type, out _type))
			{
				message.Warning($"cast object subclass failed: {BaseType} -> {type}");
			}

			return (T)Activator.CreateInstance(_type ?? BaseType);
		}
		#endregion
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


	/* Not recommended to use the constructor */
	public Ref(string value)
	{
		// Prevent designer request to load data
		if (!Settings.Default.PreviewLoadData && !FileCache.Data.IsInitialized) return;

		// get available package
		var provider = FileCache.Data.Provider;

		// get source
		if (value.Contains(':')) source = provider.Tables.GetRecord(value);
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


	public override readonly int GetHashCode() => source?.GetHashCode() ?? 0;

	public override readonly bool Equals(object obj) => obj is Ref<TElement> other && this.source == other.source;

	public static bool operator ==(Ref<TElement> left, Ref<TElement> right) => left.Equals(right);

	public static bool operator !=(Ref<TElement> left, Ref<TElement> right) => !(left == right);

	public override string ToString() => Instance?.ToString();
	#endregion
}