using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;
using Attribute = Xylia.Preview.Data.Models.AttributeCollection;

namespace Xylia.Preview.Data.Engine.BinData.Helpers;
internal class ElementConverter : TypeConverter
{
	#region Constructors
	public ElementConverter()
	{

	}

	private ElementConverter(Type baseType)
	{
		BaseType = baseType;
		var flag = baseType == typeof(ModelElement);

		foreach (var instance in Assembly.GetExecutingAssembly().GetTypes())
		{
			if ((flag || !instance.IsAbstract) && instance.BaseType == baseType)
				Subs[instance.Name.TitleLowerCase()] = instance;
		}
	}
	#endregion

	#region Methods
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		if (typeof(ModelElement).IsAssignableFrom(destinationType)) return true;

		return base.CanConvertTo(context, destinationType);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (value is Record record)
		{
			if (!typeof(ModelElement).IsAssignableFrom(destinationType)) throw new InvalidCastException();

			return record.Model ??= Get(destinationType, record.Owner.Name)?.CreateInstance(record);
		}

		return base.ConvertTo(context, culture, value, destinationType);
	}

	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (typeof(ModelElement).IsAssignableFrom(sourceType)) return true;

		return base.CanConvertFrom(context, sourceType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is ModelElement element) return element.Source;

		return base.ConvertFrom(context, culture, value);
	}
	#endregion

	#region Helpers
	private static readonly Dictionary<Type, ElementConverter> helpers = [];
	private readonly Type BaseType;
	private readonly Dictionary<string, Type> Subs = new(TableNameComparer.Instance);

	public static ElementConverter Get(Type type, string name = null)
	{
		lock (helpers)
		{
			if (!helpers.TryGetValue(type, out var converter))
			{
				converter = helpers[type] = new ElementConverter(type);
			}

			// convert to real type
			if (type == typeof(ModelElement))
			{
				Debug.Assert(name != null);

				if (!converter.Subs.TryGetValue(name, out var subType))
				{
					Debug.WriteLine($"model of {name} was not defined!");
					return null;
				}

				return Get(subType);
			}

			return converter;
		}
	}

	private ModelElement CreateInstance(Record record)
	{
		#region Type
		var subclass = record.Attributes.Get<string>(Attribute.s_type);
		Type type = null;

		if (!string.IsNullOrWhiteSpace(subclass) && !Subs.TryGetValue(subclass, out type))
		{
			Debug.WriteLine($"cast object subclass failed: {BaseType} -> {subclass}");
		}
		#endregion

		#region Instance
		var element = (ModelElement)Activator.CreateInstance(type ?? BaseType);
		element.Initialize(record);
		return element;
		#endregion
	}
	#endregion
}