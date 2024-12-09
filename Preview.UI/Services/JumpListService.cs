using System.Windows;
using System.Windows.Shell;

namespace Xylia.Preview.UI.Services;
internal class JumpListService : IService
{
	public void Register()
	{
		#region Items
		var items = new List<JumpItem>
		{
			new JumpTask
			{
				ApplicationPath = Environment.ProcessPath,
				Arguments = "-command=query -type=ue",
				Title = StringHelper.Get("Command_QueryAsset"),
				IconResourcePath = null,
			},
			new JumpTask
			{
				ApplicationPath = Environment.ProcessPath,
				Arguments = "-command=output",
				Title = StringHelper.Get("Command_Output"),
				IconResourcePath = null,
			}
		};
		#endregion

		// Create a jump-list and assign it to the current application
		var jumpList = new JumpList(items, true, false);
		JumpList.SetJumpList(Application.Current, jumpList);
		jumpList.Apply();
	}
}