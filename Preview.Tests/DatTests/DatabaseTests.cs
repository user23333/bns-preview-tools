using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Engine.BinData.Serialization;
using Xylia.Preview.Data.Engine.Definitions;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Tests.DatTests;
[TestClass]
public partial class DatabaseTests
{
	[TestMethod]
	public void AddTest()
	{
		var table = Globals.GameData.Provider.GetTable("worldbossspawn");
		var recordDef = table.Definition.DocumentElement.Children[0];
		var records = table.Records;

		// check required
		var record = new Record(table, recordDef);
		record.Attributes = new(record);
		record.Attributes["id"] = 7;
		records.Add(record);

		var settings = new TableWriterSettings() { Encoding = Encoding.UTF8 };
		Console.WriteLine(settings.Encoding.GetString(table.WriteXml(settings)));
	}

	[TestMethod]
	public void GenerateTest()
	{
		var definitions = Globals.Definition;
		var element = definitions["effect"].Elements[1];

		foreach (var attribute in element.ExpandedAttributes)
		{
			Console.WriteLine("{0:x}	{1}", attribute.Offset, attribute.Name);
		}

		foreach (var attribute in element.Attributes)
		{
			#region type
			var type = attribute.Type switch
			{
				AttributeType.TInt8 => "signed char",
				AttributeType.TInt16 => "__int16",
				AttributeType.TInt32 => "__int32",
				AttributeType.TInt64 => "__int64",
				AttributeType.TFloat32 => "float",
				AttributeType.TBool => "bool",
				AttributeType.TString => "wchar_t*",
				AttributeType.TSeq or AttributeType.TProp_seq => "signed char",
				AttributeType.TSeq16 or AttributeType.TProp_field => "__int16",
				AttributeType.TRef => "Ref",
				AttributeType.TTRef => "TRef",
				AttributeType.TSub => "__int16",
				AttributeType.TSu => "__int16",
				AttributeType.TVector16 => "Vector16",
				AttributeType.TVector32 => "Vector32",
				AttributeType.TIColor => "IColor",
				AttributeType.TFColor => "FColor",
				AttributeType.TBox => throw new NotImplementedException(),
				AttributeType.TAngle => throw new NotImplementedException(),
				AttributeType.TMsec => "__int32",
				AttributeType.TDistance => "__int16",
				AttributeType.TVelocity => "unsigned __int16",
				//AttributeType.TProp_seq => throw new NotImplementedException(),
				//AttributeType.TProp_field => throw new NotImplementedException(),
				AttributeType.TScript_obj => "Script_obj",
				AttributeType.TNative => "Native",
				AttributeType.TVersion => throw new NotImplementedException(),
				AttributeType.TIcon => "Icon",
				AttributeType.TTime32 => "__int32",
				AttributeType.TTime64 => "__int64",
				AttributeType.TXUnknown1 => "__int64",
				AttributeType.TXUnknown2 => "wchar_t*",
				_ => throw new NotImplementedException()
			};
			#endregion

			Console.WriteLine("\t\t{0} {1}{2};", type,
				attribute.Name.Replace('-', '_'),
				attribute.Repeat > 1 ? $"[{attribute.Repeat}]" : null);
		}
	}
}