using System.Reflection;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.UI.Helpers.Output;

namespace Xylia.Preview.UI.Helpers;
internal static partial class Commands
{
	public static void TableOutput(string? type)
	{
		var sets = Find();
		var intance = sets.FirstOrDefault(x => x.Name.Equals(type, StringComparison.OrdinalIgnoreCase));
		if (intance is null)
		{
			sets = sets.Where(x => x.Visible);

			EnterNumber:
			int idx = 0;
			Console.WriteLine("Enter specified number to continue...");
			sets.ForEach(x => Console.WriteLine("   [{0}] {1}", idx++, x.Name));

			// retry
			if (!int.TryParse(Console.ReadLine()!, out var i) || (intance = sets.ElementAtOrDefault(i)) is null)
			{
				Console.WriteLine();
				goto EnterNumber;
			}
		}

		intance.Execute();
	}

	private static IEnumerable<OutSet> Find()
	{
		var assembly = Assembly.GetExecutingAssembly();
		var baseType = typeof(OutSet);

		foreach (var definedType in assembly.DefinedTypes)
		{
			if (definedType.IsAbstract || definedType.IsInterface || !baseType.IsAssignableFrom(definedType)) continue;

			if (Activator.CreateInstance(definedType) is OutSet instance)
				yield return instance;
		}

		yield break;
	}
}