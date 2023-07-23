﻿using CUE4Parse.BNS.Exports;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.BuildData;

using Xylia.Preview.Data.Helper;
using Xylia.Preview.Data.Models.DatData.DataProvider;
using Xylia.Preview.Data.Record;
using Xylia.Preview.UI.Custom;
using Xylia.Preview.UI.FModel.Views;

using Application = System.Windows.Forms.Application;


namespace Xylia.Preview.UI;
public partial class DebugFrm : Form
{
	#region Constructor	   	
	public DebugFrm() => InitializeComponent();

	[STAThread]
	static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetHighDpiMode(HighDpiMode.SystemAware);
		Application.SetCompatibleTextRenderingDefault(false);


		// register
		Helper.Register.Main();
		//TextExtension.data_replace = new LocalDataTableSet(@"C:\腾讯游戏\Blade_and_Soul\新建文件夹\local64 - copy.dat");
		FileCache.Data.Provider = new FolderProvider(@"D:\资源\客户端相关\Auto\data");


		Application.Run(new DebugFrm());
		//new DebugFrm2().ShowDialog();
	}
	#endregion



	private void DebugFrm_Load(object sender, EventArgs e)
	{
		//TestTooltip2.SetTooltip(this.contentPanel2, "<p justification=\"true\" justificationtype=\"linefeedbywidgetarea\"><link id=\"none\"/> </p><p horizontalalignment=\"center\"><br/><image enablescale=\"false\" imagesetpath=\"00027918.InterD_ChungGakjiBu\"/><br/><image enablescale=\"true\" imagesetpath=\"00009499.Field_Boss\" scalerate=\"1.4\"/>铁傀王<br/><br/>中原的海盗组织——冲角团的平南舰队支部。<br/>支部长是啸四海。</p>");

		//new SearcherResult(FileCache.Data.Npc).ShowDialog();


		var Pet = FileCache.Data.Pet["Pet_JewelFly_Lv1"];


		new ModelData
		{
			Export = FileCache.PakData.LoadObject<UObject>(Pet.MeshName.Path),
			AnimSet = FileCache.PakData.LoadObject<UAnimSet>(Pet.AnimSetName.Path)

		}.Run();
	}

	private void TestMap(string name)
	{
		var MapRegistry = FileCache.PakData.LoadObject<UMapBuildDataRegistry>($"/Game/bns/Package/World/Area/{name}_BuiltData");
		throw new Exception(MapRegistry.ToString());
	}
}