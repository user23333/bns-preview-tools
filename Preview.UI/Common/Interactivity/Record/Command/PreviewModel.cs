using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using FModel.Framework;
using FModel.Views.Snooper;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.FModel.Views;

namespace Xylia.Preview.UI.Common.Interactivity;
/// <summary>
/// Provide a command to show model
/// </summary>
internal class PreviewModel : RecordCommand
{
	#region Methods
	protected override List<string> Type =>
	[
		"creatureappearance",
		"fielditem",
		"item",
		"npc",
		"pet",
		"vehicle-appearance",
	];

	protected override void Execute(Record record)
	{
		List<ModelData> models = [];
		Load(record, models).Wait();

		var view = ModelViewer;
		lock (view)
		{
			view.Models = [.. models];
			if (view.TryLoadExport(default)) view.Run();
			else throw new Exception(StringHelper.Get("Exception_InvalidModel"));
		}
	}

	private static async Task Load(Record? record, List<ModelData> models)
	{
		if (record is null) return;

		switch (record.OwnerName)
		{
			case "creatureappearance":
			{
				models.Add(new()
				{
					Export = record.Attributes.Get<ObjectPath>("body-mesh-name").LoadObject(),
					Cols = [record.Attributes.Get<ObjectPath>("body-material-name")],
				});
			}
			break;

			case "npc":
			{
				var appearance = record.Attributes.Get<Record>("appearance");
				if (appearance is null) return;

				models.Add(new()
				{
					Export = appearance.Attributes.Get<ObjectPath>("body-mesh-name").LoadObject(),
					Cols = [appearance.Attributes.Get<ObjectPath>("body-material-name")],
					AnimSet = record.Attributes.Get<ObjectPath>("animset").LoadObject<UAnimSet>()
				});
			}
			break;

			case "fielditem":
			{
				models.Add(new()
				{
					Export = record.Attributes.Get<ObjectPath>("mesh-id").LoadObject(),
					Cols = [record.Attributes.Get<ObjectPath>("mesh-col")],
					AnimSet = record.Attributes.Get<ObjectPath>("animset-name").LoadObject<UAnimSet>()
				});
			}
			break;

			case "item":
			{
				void LoadModel(string mesh, string col)
				{
					var model = new ModelData()
					{
						DisplayName = mesh,
						Export = record.Attributes.Get<ObjectPath>(mesh).LoadObject(),
						Cols = record.Attributes.Get<ObjectPath[]>(col),
					};
					if (model.Export != null) models.Add(model);
				}

				var MeshId = record.Attributes.Get<ObjectPath>("mesh-id");
				if (MeshId.IsValid)
				{
					//"mesh-animset"
					//"mesh-attach"
					//"mesh-animtree"

					LoadModel("mesh-id", "mesh-col");
					LoadModel("mesh-id-2", "mesh-col");

					models.Add(new ModelData()
					{
						Export = record.Attributes.Get<ObjectPath>("talk-mesh").LoadObject(),
						AnimSet = record.Attributes.Get<ObjectPath>("talk-animset").LoadObject<UAnimSet>(),
					});
				}
				else
				{
					LoadModel("kun-mesh", "kun-mesh-col");
					LoadModel("gon-male-mesh", "gon-male-mesh-col");
					LoadModel("gon-female-mesh", "gon-female-mesh-col");
					LoadModel("lyn-male-mesh", "lyn-male-mesh-col");
					LoadModel("lyn-female-mesh", "lyn-female-col");
					LoadModel("jin-male-mesh", "jin-male-mesh-col");
					LoadModel("jin-female-mesh", "jin-female-mesh-col");
					LoadModel("cat-mesh", "cat-mesh-col");

					if (models.Count > 0) return;
					else if (record.Name == "weapon")
					{
						var pet = record.Attributes.Get<Record>("pet");
						await Load(pet, models);

						var equipshow = record.Attributes.Get<ObjectPath>("equip-show");
						if (equipshow.IsValid)
						{
							//var EquipShow = FileCache.Pakitem.LoadObject<UShowObject>(equipshow);
						}
					}
					else if (record.Name == "accessory")
					{
						var VehicleDetail = record.Attributes.Get<Record>("vehicle-detail");
						var VehicleAppearance = VehicleDetail?.Attributes.Get<Record>("appearance");
						await Load(VehicleAppearance, models);
					}
				}

				break;
			}

			case "pet":
			{
				models.Add(new()
				{
					Export = record.Attributes.Get<ObjectPath>("mesh-name").LoadObject(),
					Cols = record.Attributes.Get<ObjectPath[]>("material-name"),
					AnimSet = record.Attributes.Get<ObjectPath>("anim-set-name").LoadObject<UAnimSet>(),
				});
				break;
			}

			case "vehicle-appearance":
			{
				models.Add(new ModelData()
				{
					Export = record.Attributes.Get<ObjectPath>("mesh-name").LoadObject(),
					Cols = record.Attributes.Get<ObjectPath[]>("material-name"),
					AnimSet = record.Attributes.Get<ObjectPath>("anim-set-name").LoadObject<UAnimSet>(),
				});
				break;
			}

			default: throw new NotSupportedException();
		}
	}
	#endregion

	#region Viewer
	private static Snooper _snooper;
	public static Snooper SnooperViewer
	{
		get
		{
			if (_snooper != null) return _snooper;

			return Application.Current.Dispatcher.Invoke(() =>
			{
				var scale = ImGuiController.GetDpiScale();
				var htz = Snooper.GetMaxRefreshFrequency();

				return _snooper = new Snooper(
				new GameWindowSettings { UpdateFrequency = htz },
				new NativeWindowSettings
				{
					ClientSize = new OpenTK.Mathematics.Vector2i(
						Convert.ToInt32(SystemParameters.MaximizedPrimaryScreenWidth * .75 * scale),
						Convert.ToInt32(SystemParameters.MaximizedPrimaryScreenHeight * .85 * scale)),
					NumberOfSamples = 4,
					WindowBorder = WindowBorder.Resizable,
					Flags = ContextFlags.ForwardCompatible,
					Profile = ContextProfile.Core,
					Vsync = VSyncMode.Adaptive,
					APIVersion = new Version(4, 6),
					StartVisible = false,
					StartFocused = false,
					Title = "3D Viewer"
				});
			});
		}
	}


	private static ModelView _model;
	public static ModelView ModelViewer
	{
		get
		{
			if (_model != null) return _model;

			return Application.Current.Dispatcher.Invoke(() =>
			{
				var scale = ImGuiController.GetDpiScale();
				var htz = Snooper.GetMaxRefreshFrequency();
				return _model = new ModelView(
					new GameWindowSettings { UpdateFrequency = htz },
					new NativeWindowSettings
					{
						ClientSize = new OpenTK.Mathematics.Vector2i(
							Convert.ToInt32(SystemParameters.MaximizedPrimaryScreenWidth * .45 * scale),
							Convert.ToInt32(SystemParameters.MaximizedPrimaryScreenHeight * .85 * scale)),
						NumberOfSamples = 4,
						WindowBorder = WindowBorder.Resizable,
						Flags = ContextFlags.ForwardCompatible,
						Profile = ContextProfile.Core,
						Vsync = VSyncMode.Adaptive,
						APIVersion = new Version(4, 6),
						StartVisible = false,
						StartFocused = false,
						Title = "Model",
					});
			});
		}
	}
	#endregion
}