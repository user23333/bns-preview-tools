﻿using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Tests.DatTests;

[TestClass]
public partial class Tables
{
	readonly BnsDatabase Database = new(new FolderProvider(@"D:\资源\客户端相关\Auto\data"));

	[TestMethod]
	public void SerializeTest()
	{
		using RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
		var PrivateKey = rsa.ToXmlString(true);
		var PublicKey = rsa.ToXmlString(false);
		var Parameter = rsa.ExportParameters(true);

		Console.WriteLine(PrivateKey);
		Console.WriteLine(PublicKey);
	}

	[TestMethod]
	public void ClassicLevel()
	{
		int day = 0, UsedExp = 0;
		var table = Database.Provider.GetTable<Level>();
		foreach (var record in table)
		{
			// get exp for next level
			var vitality = record.TencentVitalityMax[0];
			int exp = (table[record.level + 1]?.Exp ?? 0) - record.Exp;

			// get result
			if (UsedExp + exp < vitality)
			{
				UsedExp += exp;
			}
			else
			{
				day++;

				var Exceed = vitality - UsedExp;
				UsedExp = exp - Exceed;

				Console.WriteLine($"{day} | {record.level} | {(float)Exceed / exp:P0}");
			}
		}
	}


	[TestMethod]
	public void Test()
	{
		//foreach (var pair in Database.Provider.GetTable<Npc>()
		//	.Select(x => (x, x.Attributes.Get<int>("fatigability-consume-amount")))
		//	.Where(x => x.Item2 != 0))
		//{
		//	Console.WriteLine($"{pair.x.Text} {pair.Item2}");
		//}


		//var table = Database.Provider.GetTable<Reward>();
		//table["General_Grocery_Event_Box_12"].TestMethods();


		var Job = JobSeq.귀검사;
		var RandomOptionGroupId = Database.Provider.GetTable<Item>()["General_Accessory_Belt_3021_031"].RandomOptionGroupId;
		var ItemRandomOptionGroup = Database.Provider.GetTable<ItemRandomOptionGroup>()[RandomOptionGroupId + ((long)Job << 32)];

		ItemRandomOptionGroup.TestMethod();
	}
}