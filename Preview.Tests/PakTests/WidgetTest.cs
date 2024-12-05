using System.Xml.Linq;
using CUE4Parse.BNS;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Objects.Core;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Tests.Extensions;

namespace Xylia.Preview.Tests;
public partial class PakTest
{
	[TestMethod]
	[DataRow("bnsr/content/art/ui/v3/scene/game_party/passiveeffect/playerpassiveeffectpanel_neo.uasset")]
	[DataRow("bnsr/content/art/ui/v3/scene/game_party/passiveeffect/commonbufflistwidget.uasset")]
	[DataRow("bnsr/content/art/ui/v3/scene/game_party/passiveeffect/commondebufflistwidget.uasset")]
	[DataRow("bnsr/content/art/ui/v3/scene/game_party/passiveeffect/commondisableeffectlistwidget.uasset")]
	[DataRow("bnsr/content/art/ui/v3/scene/game_party/passiveeffect/commonskillrecyclelistwidget.uasset")]
	[DataRow("bnsr/content/art/ui/v3/common/contentswidget/passiveeffecticon/commonbuffeffecticon.uasset")]
	[DataRow("bnsr/content/art/ui/v3/common/contentswidget/passiveeffecticon/commondebuffeffecticon.uasset")]
	[DataRow("bnsr/content/art/ui/v3/common/contentswidget/passiveeffecticon/commondisableeffecticon.uasset")]
	public void WidgetTest(string AssetPath)
	{
		using var provider = new GameFileProvider(IniHelper.Instance.GameFolder);
		var blueprint = provider.LoadAllObjects(AssetPath).OfType<UWidgetBlueprintGeneratedClass>().First();

		var root = new XElement("temp");
		new WidgetDump().LoadBlueprint(blueprint, root);

		#region Output
		// get class name
		var FullClassName = AssetPath.SubstringBeforeLast(".").Replace("/", ".");

		var FirstNode = (XElement)root.FirstNode;
		FirstNode.Attribute("Name").Value = "#TEMP#";
		Console.WriteLine(FirstNode.ToString().Replace(" Name=\"#TEMP#\"",
			$"""
				x:Class="{FullClassName}"
				xmlns="https://github.com/xyliaup/bns-preview-tools"
				xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
				xmlns:s="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
			"""));
		#endregion
	}
}

public class WidgetDump
{
	public void LoadBlueprint(UWidgetBlueprintGeneratedClass blueprint, XElement parent)
	{
		var bAllowTemplate = blueprint.GetOrDefault<bool>("bAllowTemplate");
		var bValidTemplate = blueprint.GetOrDefault<bool>("bValidTemplate");
		var bClassRequiresNativeTick = blueprint.GetOrDefault<bool>("bClassRequiresNativeTick");
		var DefaultObject = blueprint.ClassDefaultObject;
		var TemplateAsset = blueprint.GetOrDefault<FSoftObjectPath>("TemplateAsset");
		//var WidgetTree = blueprint.GetOrDefault<UWidgetTree>("WidgetTree");
		//this.LoadWidget(WidgetTree.RootWidget.Load(), null, Root);

		var WidgetTree0 = TemplateAsset.Load().GetOrDefault<UWidgetTree>("WidgetTree");
		this.LoadWidget(WidgetTree0.RootWidget.Load(), null, parent);
	}

	public void LoadWidget(UObject obj, UObject widgetslot, XElement parent)
	{
		if (obj.Template != null)
		{
			var WidgetTree0 = obj.GetOrDefault<UWidgetTree>("WidgetTree");
			this.LoadWidget(WidgetTree0.RootWidget.Load(), null, parent);
			return;
		}

		#region Content
		var el = parent.AddElement(obj.ExportType);
		el.Add(new XAttribute("Name", obj.Name));

		if (widgetslot != null)
		{
			var LayoutData = widgetslot.GetOrDefault<FLayout>("LayoutData");
			el.AddAttribute("LayoutData.Anchors", LayoutData.Anchors);
			el.AddAttribute("LayoutData.Offsets", LayoutData.Offsets);
			el.AddAttribute("LayoutData.Alignments", LayoutData.Alignments);
		}

		if (obj is UBnsCustomBaseWidget widget)
		{
			el.AddAttribute("MetaData", widget.MetaData);

			if (widget.HorizontalResizeLink != null) el.AddElement($"{el.Name}.HorizontalResizeLink").Write(widget.HorizontalResizeLink);
			if (widget.VerticalResizeLink != null) el.AddElement($"{el.Name}.VerticalResizeLink").Write(widget.VerticalResizeLink);
			if (widget.StringProperty != null) el.AddElement($"{el.Name}.String").Write(widget.StringProperty);
			if (widget.BaseImageProperty != null) el.AddElement($"{el.Name}.BaseImageProperty").Write(widget.BaseImageProperty);

			if (widget is UBnsCustomLabelButtonWidget buttonWidget)
			{
				el.AddElement($"{el.Name}.NormalImageProperty").Write(buttonWidget.NormalImageProperty);
				//Write(el, buttonWidget.ActivatedImageProperty);
			}

			el.Write(widget.ExpansionComponentList, "ExpansionComponentList");
		}
		#endregion

		#region Slots
		foreach (var slot in obj.GetOrDefault<UObject[]>("Slots", []))
		{
			var Parent = slot.GetOrDefault<FPackageIndex>("Parent");
			var Content = slot.GetOrDefault<FPackageIndex>("Content");
			var LayoutData = slot.GetOrDefault<FLayout>("LayoutData");

			LoadWidget(Content.Load(), slot, el);
		}
		#endregion
	}
}