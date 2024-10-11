using System.ComponentModel;
using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports;
using FModel.Framework;
using FModel.Views.Snooper;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Xylia.Preview.Data.Helpers;
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
		"npc",
		"item",
		"pet",
		"vehicle-appearance",
	];

	protected override void Execute(Record record)
	{
		List<ModelData> models = [];
		Load(record, models).Wait();

		// show
		var view = ModelViewer;
		lock (view)
		{
			view.Models = [.. models];
			if (view.TryLoadExport(default)) view.Run();
			else throw new WarningException(StringHelper.Get("PreviewModel_Empty"));
		}
	}

	private static async Task Load(Record record, List<ModelData> models)
	{
		if (record is null) return;

		switch (record.OwnerName)
		{
			case "creatureappearance":
			{
				models.Add(new()
				{
					Export = await FileCache.Provider.LoadObjectAsync<UObject>(record.Attributes["body-mesh-name"]?.ToString()),
					Cols = new string?[] { record.Attributes.Get<string>("body-material-name") },
				});
			}
			break;

			case "npc":
			{
				var appearance = record.Attributes.Get<Record>("appearance");
				if (appearance is null) return;

				models.Add(new()
				{
					Export = await FileCache.Provider.LoadObjectAsync<UObject>(appearance.Attributes["body-mesh-name"]?.ToString()),
					Cols = new string?[] { appearance.Attributes.Get<string>("body-material-name") },
					AnimSet = await FileCache.Provider.LoadObjectAsync<UAnimSet>(record.Attributes["animset"]?.ToString()),
				});
			}
			break;

			case "item":
			{
				void LoadModel(string Mesh, string Col = null)
				{
					Col ??= Mesh + "-col-";
					models.Add(new ModelData()
					{
						DisplayName = Mesh,
						Export = FileCache.Provider.LoadObject<UObject>(record.Attributes[Mesh]?.ToString()),
						Cols = new string?[] { record.Attributes.Get<string>(Col + 1), record.Attributes.Get<string>(Col + 2), record.Attributes.Get<string>(Col + 3) },
					});
				}

				var MeshId = record.Attributes["mesh-id"];
				if (!string.IsNullOrEmpty(MeshId?.ToString()))
				{
					//"mesh-id"
					//"mesh-id-2"
					//"mesh-col-1"
					//"mesh-col-2"
					//"mesh-col-3"
					//"mesh-animset"
					//"mesh-attach"
					//"mesh-animtree"

					LoadModel("mesh-id", "mesh-col");

					models.Add(new ModelData()
					{
						Export = FileCache.Provider.LoadObject<UObject>(record.Attributes["talk-mesh"]?.ToString()),
						AnimSet = FileCache.Provider.LoadObject<UAnimSet>(record.Attributes["talk-animset"]?.ToString()),
					});
				}
				else
				{
					LoadModel("kun-mesh");
					LoadModel("gon-male-mesh");
					LoadModel("gon-female-mesh");
					LoadModel("lyn-male-mesh");
					LoadModel("lyn-female-mesh");
					LoadModel("jin-male-mesh");
					LoadModel("jin-female-mesh");
					LoadModel("cat-mesh");

					var temp = models.Where(model => model.Export != null);
					if (temp.Any())
					{
						models = temp.ToList();
						return;
					}

					else if (record.Name == "weapon")
					{
						var pet = record.Attributes.Get<Record>("pet");
						await Load(pet, models);

						var equipshow = record.Attributes["equip-show"];
						if (!string.IsNullOrEmpty(equipshow?.ToString()))
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
					Export = await FileCache.Provider.LoadObjectAsync<UObject>(record.Attributes["mesh-name"]?.ToString()),
					AnimSet = await FileCache.Provider.LoadObjectAsync<UAnimSet>(record.Attributes["anim-set-name"]?.ToString()),
					Cols = [record.Attributes.Get<string>("material-name-1"), record.Attributes.Get<string>("material-name-2"), record.Attributes.Get<string>("material-name-3")],
				});
				break;
			}

			case "vehicle-appearance":
			{
				models.Add(new ModelData()
				{
					Export = FileCache.Provider.LoadObject<UObject>(record.Attributes["mesh-name"]?.ToString()),
					AnimSet = FileCache.Provider.LoadObject<UAnimSet>(record.Attributes["anim-set-name"]?.ToString()),
					Cols = [record.Attributes.Get<string>("material-name-1"), record.Attributes.Get<string>("material-name-2"), record.Attributes.Get<string>("material-name-3")],
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