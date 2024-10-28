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
			object obj = null;
			var ps = P?.Split(':');

			for (int i = 0; i < ps.Length; i++)
			{
				var p = ps[i];

				// source
				if (i == 0)
				{
					switch (p)
					{
						case "id":
							obj = new Ref<ModelElement>(Id).Instance;
							break;

						case "seq":
							var seqs = Seq?.Split(':');
							obj = seqs[1].CastSeq(seqs[0]);
							break;

						default:
							if (!byte.TryParse(p, out var id))
								throw new InvalidCastException("bad argument id, must be byte value: " + p);

							obj = arguments?[id - 1];
							break;

					}

					if (obj is null) return null;
				}
				// child
				else
				{
					var args = ArgItem.GetArgs(p);
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
			}

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
			if (Target is null || value is null) return;

			switch (Target.ToLower())
			{
				case "string": value = value.ToString(); break;
				case "integer": value = value.To<Integer>(); break;
				case "item-name" when value is Item item: value = item.ItemName; break;
				case "skill" when value is Skill3: break;

				default:
				{
					if (value is Integer integer && IArgument.TryGet(integer, Target, out value)) return;

					if (!TableNameComparer.Instance.Equals(target, value.GetBaseType(typeof(ModelElement)).Name))
						throw new InvalidCastException($"valid failed: {Target} >> {value.GetType()}");

					break;
				}
			}
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
			else if (value.GetType().IsClass && IArgument.TryGet(value, Target, out var temp)) value = temp;
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
		#endregion
	}
	#endregion
}

public interface IArgument
{
	bool TryGet(string name, out object value);

	internal static bool TryGet<T>(T instance, string name, out object value)
	{
		if (name == instance.GetType().Name)
		{
			value = instance;
			return true;
		}

		// property
		var prop = instance.GetProperty(name);
		if (prop != null)
		{
			value = prop.GetValue(instance);
			if (value is Ref<Text> text) value = text.GetText();

			return true;
		}

		// interface
		if (instance is IArgument provider && provider.TryGet(name, out value)) return true;

		value = null;
		return false;
	}
}