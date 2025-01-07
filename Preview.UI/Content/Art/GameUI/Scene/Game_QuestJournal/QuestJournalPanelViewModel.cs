using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Common.Interactivity;
using static Xylia.Preview.Data.Models.Quest;

namespace Xylia.Preview.UI.GameUI.Scene.Game_QuestJournal;
internal class QuestJournalPanelViewModel : ObservableObject, IRecordFilter
{
	#region Properties
	private ICollectionView? _source;
	public ICollectionView Source
	{
		get => _source;
		set
		{
			_source = value;
			_source.Filter += OnFilter;
		}
	}

	private string? _searchRule;
	public string? SearchRule
	{
		get => _searchRule;
		set
		{
			SetProperty(ref _searchRule, value);
			_source?.Refresh();
		}
	}
	#endregion

	#region Methods
	public void SetFlag(Enum seq, bool status)
	{
		seq.SetFlag(ref FilterMask, seq switch
		{
			CategorySeq => MASK_Category,
			ContentTypeSeq => MASK_ContentType,
			ResetTypeSeq => MASK_ResetType,
			_ => throw new NotSupportedException()
		}, status);
	}

	public bool OnFilter(object obj)
	{
		if (obj is not Quest quest) return false;

		// filter
		var category = (short)(FilterMask >> MASK_Category);
		if (category != 0 && !quest.Category.InFlag(category)) return false;

		var contentType = (short)(FilterMask >> MASK_ContentType);
		if (contentType != 0 && !quest.ContentType.InFlag(contentType)) return false;

		var resetType = (byte)(FilterMask >> MASK_ResetType);
		if (resetType != 0 && !quest.ResetType.InFlag(resetType)) return false;

		// check rule
		var IsEmpty = string.IsNullOrEmpty(_searchRule);
		if (IsEmpty) return true;

		// filter rule 
		if (int.TryParse(_searchRule, out int id)) return quest.PrimaryKey == id;
		if (quest.Attributes.Get<string>("alias")?.Equals(_searchRule, StringComparison.OrdinalIgnoreCase) ?? false) return true;
		if (quest.Name?.Contains(_searchRule, StringComparison.OrdinalIgnoreCase) ?? false) return true;
		if (quest.Group?.Contains(_searchRule, StringComparison.OrdinalIgnoreCase) ?? false) return true;
		return false;
	}
	#endregion

	#region Fields
	private long FilterMask;

	private const int MASK_Category = 0;     //short (flags gather than 255)
	private const int MASK_ContentType = 16; //short (flags gather than 255)
	private const int MASK_ResetType = 32;   //byte
	#endregion
}