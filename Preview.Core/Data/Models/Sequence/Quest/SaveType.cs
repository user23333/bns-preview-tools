using Xylia.Preview.Common.Attributes;

namespace Xylia.Preview.Data.Models.Sequence;
public enum SaveType
{
	All,

	/// <summary>
	/// 25000~25500
	/// </summary>
	Nothing,

	/// <summary>
	/// 20000~23000
	/// </summary>
	[Name("except-completion")]
	ExceptCompletion,

	/// <summary>
	/// 28000~
	/// </summary>
	[Name("except-completion-and-logout-save")]
	ExceptCompletionAndLogoutSave,
}