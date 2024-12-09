namespace Xylia.Preview.UI.Services;
internal class UserService : IService
{
	#region Fields
	public int UserId;
	public string? Name;
	public UserRole Role = default;
	#endregion

	#region Methods
	public static UserService? Instance { get; private set; }

	public void Register() => throw new NotImplementedException();
	#endregion
}

public enum UserRole
{
	None,
	Normal,
	Advanced,
}