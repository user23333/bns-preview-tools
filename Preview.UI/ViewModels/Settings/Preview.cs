using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Controls;

namespace Xylia.Preview.UI.ViewModels;
internal partial class UserSettings
{
	public IEnumerable<JobSeq> Jobs => Data.Models.Job.PcJobs;

	/// <summary>
	/// Gets or sets <see cref="BnsCustomLabelWidget"/> Copy Mode
	/// </summary>
	public CopyMode CopyMode
	{
		get => (CopyMode)GetValue<int>();
		set
		{
			SetValue((int)value);
			BnsCustomLabelWidget.CopyMode = value;
		}
	}

	public string? TextView_OldPath { get => GetValue<string>(); set => SetValue(value); }
	public string? TextView_NewPath { get => GetValue<string>(); set => SetValue(value); }

	public bool TextEditor_WordWrap { get => GetValue<bool>(); set => SetValue(value); }
	public bool TextEditor_ShowEndOfLine { get => GetValue<bool>(); set => SetValue(value); }
}