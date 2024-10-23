using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Views;

namespace Xylia.Preview.UI.Common.Interactivity;
/// <summary>
/// command for <see cref="Record"/>
/// will be automatically registered to <see cref="TableView"/>
/// </summary>
public abstract class RecordCommand : MarkupExtension, ICommand
{
	#region Override Methods   
	public override object ProvideValue(IServiceProvider serviceProvider) => this;

	public event EventHandler? CanExecuteChanged;

	public bool CanExecute(object? parameter)
	{
		// get available command instances base on table name
		if (parameter is string name) return Type is null || Type.Contains(name.ToLower());
		// process the source element
		else if (parameter is Record record) return CanExecute(record);
		else if (parameter is ModelElement model) return CanExecute(model.Source);
		else return false;
	}

	public void Execute(object? parameter) => Task.Run(() =>
	{
		try
		{
			if (parameter is Record record) Execute(record);
			if (parameter is ModelElement model) Execute(model.Source);
		}
		catch (Exception ex)
		{
			App.SendMessage(ex.Message);
		}
	});

	public void NotifyCanExecuteChanged()
	{
		this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
	#endregion

	#region Instance Methods
	/// <summary>
	/// Display text
	/// </summary>
	public virtual string Name => "Name." + GetType().Name;

	/// <summary>
	/// Supported table type, <see langword="Null"/> means all table. 
	/// </summary>
	protected abstract List<string>? Type { get; }

	/// <summary>
	/// Defines the method that determines whether the command can execute in its current state.
	/// </summary>
	/// <returns></returns>
	protected virtual bool CanExecute(Record record) => true;

	/// <summary>
	/// Defines the method to be called when the command is invoked.
	/// </summary>
	/// <param name="record"></param>
	protected abstract void Execute(Record record);
	#endregion

	#region Static Methods
	/// <summary>
	/// Search command by table name.
	/// </summary>
	/// <param name="name"></param>
	/// <param name="action"></param>
	public static void Find(string name, Action<RecordCommand> action)
	{
		var assembly = Assembly.GetExecutingAssembly();
		var baseType = typeof(RecordCommand);

		foreach (var definedType in assembly.DefinedTypes)
		{
			if (definedType.IsAbstract || definedType.IsInterface || !baseType.IsAssignableFrom(definedType)) continue;

			var instance = Activator.CreateInstance(definedType);
			if (instance is RecordCommand command && command.CanExecute(name)) action.Invoke(command);
		}
	}

	/// <summary>
	/// Bind menu
	/// </summary>
	/// <param name="command"></param>
	/// <param name="menu"></param>
	public static void Bind(RecordCommand command, ContextMenu menu)
	{
		var item = new MenuItem()
		{
			Header = StringHelper.Get(command.Name),
			Command = command,
		};
		item.SetBinding(MenuItem.CommandParameterProperty, new Binding("DataContext") { Source = menu });

		menu.Items.Add(item);
	}
	#endregion
}