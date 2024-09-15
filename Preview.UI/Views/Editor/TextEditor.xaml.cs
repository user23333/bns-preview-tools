using System.Windows;
using System.Windows.Input;
using System.Xml;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using ICSharpCode.AvalonEdit.Search;
using Xylia.Preview.Common.Extension;

namespace Xylia.Preview.UI.Views.Editor;
public partial class TextEditor : Window
{
	#region Constructor
	private readonly FoldingManager foldingManager;
	private readonly XmlFoldingStrategy foldingStrategy;

	public TextEditor()
	{
		InitializeComponent();
		RegisterCommands(this.CommandBindings);

		SearchPanel.Install(Editor);
		foldingManager = FoldingManager.Install(Editor.TextArea);
		foldingStrategy = new XmlFoldingStrategy();
	}
	#endregion

	#region Properties
	internal byte[]? Data { get; set; }

	public string Text
	{
		get => Editor.Text;
		set => Editor.Text = value;
	}
	#endregion

	#region Methods
	private void TextChanged(object sender, EventArgs e)
	{
		foldingStrategy.UpdateFoldings(foldingManager, Editor.Document);
	}


	// Commands 
	private void RegisterCommands(CommandBindingCollection commandBindings)
	{
		commandBindings.Add(new CommandBinding(ApplicationCommands.Properties, ChangeOptionCommand));
		commandBindings.Add(new CommandBinding(ApplicationCommands.Copy, CopyCommand));
		commandBindings.Add(new CommandBinding(ApplicationCommands.Save, CopyDataCommand, CanExecuteCopyData));
	}

	private void ChangeOptionCommand(object sender, ExecutedRoutedEventArgs e)
	{
		var prop = Editor.Options.GetProperty(e.Parameter?.ToString());
		if (prop is null) return;

		var value = prop.GetValue(Editor.Options);
		if (value is bool flag) prop.SetValue(Editor.Options, !flag);
	}

	private void CopyCommand(object sender, RoutedEventArgs e)
	{
		Clipboard.SetText(Editor.Text);
	}

	private void CanExecuteCopyData(object sender, CanExecuteRoutedEventArgs e)
	{
		e.CanExecute = Data != null;
	}

	private void CopyDataCommand(object sender, RoutedEventArgs e)
	{
		Clipboard.SetText(Data.ToHex());
	}


	public static void Register(string name)
	{
		var resource = new Uri($"pack://application:,,,/Preview.UI;component/Resources/Xshd/{name}.xshd");
		using var stream = Application.GetResourceStream(resource).Stream;
		using var reader = new XmlTextReader(stream);

		var definition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
		HighlightingManager.Instance.RegisterHighlighting("SQL", [".sql"], definition);
	}
	#endregion
}