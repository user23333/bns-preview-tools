namespace Xylia.Preview.UI.Services;
internal class UserService : IService
{
	#region Methods
	public void Register()
	{
		throw new NotImplementedException();
	}
	#endregion

	#region User
	internal static User Instance = default;
	internal struct User
	{
		public int UserId;
		public string? Name;
		public bool IsAdmin;
	}
	#endregion
}