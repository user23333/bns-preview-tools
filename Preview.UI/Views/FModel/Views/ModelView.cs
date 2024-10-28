using OpenTK.Windowing.Desktop;
using Xylia.Preview.UI.FModel.Views;

namespace FModel.Views.Snooper;
public class ModelView : Snooper
{
	public ModelView(GameWindowSettings gwSettings, NativeWindowSettings nwSettings) : base(gwSettings, nwSettings)
	{
		_gui = new ModelGui(ClientSize.X, ClientSize.Y);
	}


	public ModelData[]? Models { get; set; }
	public ModelData? SelectedData { get; private set; }

	public bool TryLoadExport(CancellationToken token, ModelData? models = null)
	{
		SelectedData = models ?? Models?.FirstOrDefault();
		if (SelectedData is null) return false;

		// render
		Renderer.Load(token, SelectedData.Export!);
		if (!Renderer.Options.TryGetModel(out var model)) return false;

		// transform
		SelectedData.Materials?.ForEach(Renderer.Swap);
		model.Transforms.First().Rotation.Z = 1F;
		return true;
	}
}