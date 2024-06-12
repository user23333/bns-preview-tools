using System.Text;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Engine.Definitions;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Services;

namespace Xylia.Preview.UI.ViewModels;
internal partial class DatabaseStudioViewModel : ObservableObject
{
    #region ToolBar
    [ObservableProperty]
    private bool _connectStatus;

    [ObservableProperty]
    private Table? _currentTable;

    [ObservableProperty]
    private TableDefinition? _currentDefinition;

#pragma warning disable CA1822
    public string SaveDataPath => UserSettings.Default.OutputFolder + "\\data";

    public bool UseImport => UserService.Instance?.Role >= UserRole.Advanced;
#pragma warning restore CA1822
	#endregion

	#region SQL Result
	[ObservableProperty]
    internal bool _isGlobalData = false;

    [ObservableProperty]
    private int _limitNum = 2000;

    [ObservableProperty]
    private bool _indentText = true;


    private List<AttributeValue>? QueryResult { get; set; }

    public void ReadResult(IDataReader reader)
    {
        this.QueryResult = [];

        while (reader.Read())
        {
            this.QueryResult.Add(reader.Current);
        }
    }

    public void BindData(DataGrid grd)
    {
        using var dt = new System.Data.DataTable();

        foreach (var value in QueryResult!)
        {
            var row = dt.NewRow();

            var doc = value.IsDocument ?
                value.AsDocument :
                new AttributeDocument { ["[value]"] = value };

            if (doc.Count == 0) doc["[root]"] = "{}";

            foreach (var key in doc)
            {
                var col = dt.Columns[key.Key];
                if (col is null)
                {
                    dt.Columns.Add(key.Key);
                }
            }

            foreach (var key in doc)
            {
                row[key.Key] = value.IsDocument ? value[key.Key] : value;
            }

            dt.Rows.Add(row);
        }

        if (dt.Rows.Count == 0)
        {
            dt.Columns.Add("no-data");
            dt.Rows.Add("[no result]");
        }

        grd.ItemsSource = dt.DefaultView;
    }

    public void BindData(ICSharpCode.AvalonEdit.TextEditor editor)
    {
        var index = 0;
        var builder = new StringBuilder();
        var settings = new JsonSerializerSettings()
        {
            Formatting = IndentText ? Formatting.Indented : Formatting.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        if (QueryResult!.Count == 0)
        {
            builder.AppendLine("no result");
        }
        else
        {
            foreach (var value in QueryResult)
            {
                builder.AppendLine($"/* {index++ + 1} */");
                builder.AppendLine(JsonConvert.SerializeObject(value, settings));
                builder.AppendLine();
            }

            // LimitExceeded
            if (false)
            {
                builder.AppendLine();
                builder.AppendLine("/* Limit exceeded */");
            }
        }

        editor.Text = builder.ToString();
    }
    #endregion
}