using System.Diagnostics;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models.Document;
public class Arg : HtmlElementNode
{
	#region Fields
	public string P { get => GetAttributeValue<string>(); set => SetAttributeValue(value); }
	public string Id { get => GetAttributeValue<string>(); set => SetAttributeValue(value); }
	public string Seq { get => GetAttributeValue<string>(); set => SetAttributeValue(value); }
	public bool Link { get => GetAttributeValue<bool>(); set => SetAttributeValue(value); }
	#endregion

	#region Methods
	public object GetObject(TextArguments arguments)
	{
		try
		{
			object obj;

			#region source
			var ps = P?.Split(':');
			var type = ps?[0];

			switch (type)
			{
				case "id":
					obj = new Ref<ModelElement>(Id).Instance;
					break;

				case "seq":
					var seqs = Seq?.Split(':');
					obj = seqs[1].CastSeq(seqs[0]);
					break;

				default:
					if (!byte.TryParse(type, out var id))
						throw new InvalidCastException("bad argument id, must be byte value: " + type);

					obj = arguments?[id - 1];
					break;

			}

			if (obj is null) return null;
			#endregion

			#region child
			foreach (var pl in ps.Skip(1))
			{
				var args = ArgItem.GetArgs(pl);
				for (int x = 0; x < args.Length; x++)
				{
					if (x == 0) args[0].ValidType(ref obj);
					else
					{
						args[x].GetObject(ref obj, out var handle);
						if (handle) break;
					}
				}
			}
			#endregion

			return obj;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"handle arg failed: {P}\n\t{ex.Message}");
			return null;
		}
	}
	#endregion

	#region Helpers
	[DebuggerDisplay("{Target}")]
	class ArgItem(string target)
	{
		#region Properties
		public string Target => target;

		public ArgItem Prev { get; private set; }

		public ArgItem Next { get; private set; }
		#endregion

		#region Methods	
		internal void ValidType(ref object value)
		{
			var target = Target?.ToLower();
			if (target is null || value is null) return;

			// convert type
			var type = value.GetBaseType(typeof(ModelElement));
			if (target == "string")
			{
				if (type != typeof(string)) value = value.ToString();
				return;
			}
			else if (target == "integer") value = new Integer(Convert.ToDouble(value));
			else if (value is Integer integer && TryGetArgument(integer, target, out var temp)) value = temp;
			else if (value is Item item && target.Equals("item-name")) value = item.ItemName;
			else if (value is Skill3 && target.Equals("skill")) return;
			else if (TableNameComparer.Instance.Equals(target, type.Name)) return;
			else throw new InvalidCastException($"valid failed: {Target} >> {type}");
		}

		internal void GetObject(ref object value, out bool handle)
		{
			handle = false;

			if (value is null) return;
			else if (value is string) return;
			else if (value is ImageProperty image)
			{
				if (Target == "scale")
				{
					image.ImageScale = short.Parse(Next.Target) * 0.01F;
					handle = true;
				}
			}
			else if (value.GetType().IsClass && TryGetArgument(value, Target, out var param)) value = param;
			else
			{
				Debug.WriteLine($"not supported class: {value} ({value.GetType().Name} > {Target})");
				value = null;
			}
		}

		internal static ArgItem[] GetArgs(string text)
		{
			var args = text.Split('.').Select(o => new ArgItem(o)).ToArray();
			for (int x = 0; x < args.Length; x++)
			{
				if (x != 0)
					args[x].Prev = args[x - 1];

				if (x != args.Length - 1)
					args[x].Next = args[x + 1];
			}

			return args;
		}

		internal static bool TryGetArgument<T>(T instance, string name, out object value)
		{
			if (name == instance!.GetType().Name)
			{
				value = instance;
				return true;
			}

			// property
			var member = instance.GetProperty(name);
			if (member != null)
			{
				value = member.GetValue(instance);
				if (value is Ref<Text> text) value = text.GetText();

				return true;
			}

			// attribute
			if (instance is ModelElement element && element.Attributes.TryGetValue(name, out var pair))
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
	#endregion
}