using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using HandyControl.Data;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Common.Converters;
using Xylia.Preview.UI.Common.Interactivity;
using Xylia.Preview.UI.Controls;

namespace Xylia.Preview.UI.Views;
public partial class TableView
{
	#region Constructors
	public TableView()
	{
		InitializeComponent();

		ItemMenu = (ContextMenu)this.TryFindResource("ItemMenu");
		TooltipHolder = (ContentControl)this.TryFindResource("TooltipHolder");
	}
	#endregion

	#region Properties
	public Table Table
	{
		set
		{
			this.Title = StringHelper.Get("TableView_Name", value.Name);
			this.ColumnList.ItemsSource = _source = CollectionViewSource.GetDefaultView(value.Records);

			CreateItemMenu(value.Name);
		}
	}
	#endregion

	#region Methods
	private void CreateItemMenu(string name)
	{
		ItemMenu.Items.Clear();

		RecordCommand.Find(name, (command) =>
		{
			var item = new MenuItem()
			{
				Header = StringHelper.Get(command.Name),
				Command = command,
			};
			item.SetBinding(MenuItem.CommandParameterProperty, new Binding("SelectedItem") { Source = ColumnList });

			ItemMenu.Items.Add(item);
		});
	}

	/// <summary>
	/// filter item
	/// </summary>	
	private bool Filter(object item, string rule)
	{
		if (item is Record record)
		{
			if (record.ToString().Contains(rule, StringComparison.OrdinalIgnoreCase)) return true;
			if (record.PrimaryKey.ToString().Contains(rule, StringComparison.OrdinalIgnoreCase)) return true;
			if (RecordNameConverter.Convert(record)?.Contains(rule, StringComparison.OrdinalIgnoreCase) ?? false) return true;
		}

		return false;
	}

	/// <summary>
	/// search item
	/// </summary>
	private void SearchStarted(object sender, FunctionEventArgs<string> e)
	{
		_source.Filter = (o) => Filter(o, e.Info);
		_source.Refresh();
		_source.MoveCurrentToFirst();

		ColumnList.ScrollIntoView(_source.CurrentItem);
	}

	protected override void OnPreviewKeyDown(KeyEventArgs e)
	{
		switch (e.Key == Key.System ? e.SystemKey : e.Key)
		{
			case Key.LeftShift when TooltipHolder != null:
			{
				(TooltipHolder.Content as BnsCustomWindowWidget)?.Show();
				break;
			}
		}
	}
	#endregion


	#region Data
	private readonly ContextMenu ItemMenu;
	private readonly ContentControl TooltipHolder;

	private ICollectionView? _source;
	#endregion
}