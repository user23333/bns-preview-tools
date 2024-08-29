using System.ComponentModel;

namespace Xylia.Preview.UI.Views.Selector;
[DesignTimeVisible(false)]
public partial class UpdateLogDialog
{
	public UpdateLogDialog()
	{
		InitializeComponent();
		Holder.Text = StringHelper.Get("Application_UpdateLog");
	}
}