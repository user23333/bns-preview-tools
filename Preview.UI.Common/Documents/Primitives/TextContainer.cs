using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Documents.Primitives;
/// <summary>
/// implementation of text documents.
/// </summary>
public class TextContainer
{
	#region Constructors
	/// <summary>
	/// Creates a new TextContainer instance.
	/// </summary>
	/// <param name="parent">
	/// A DependencyObject used to supply inherited property values for
	/// TextElements contained within this TextContainer.
	///
	/// parent may be null.
	///
	/// If the object is FrameworkElement or FrameworkContentElement, it will be
	/// the parent of all top-level TextElements.
	/// </param>
	internal TextContainer(DependencyObject parent)
	{

	}
	#endregion


	#region Internal Properties
	public TextArguments Arguments
	{
		get => _arguments;
		set
		{
			_arguments = value;
			//ChangedHandler?.Invoke(this, EventArgs.Empty);
		}
	}
	#endregion

	#region Internal Methods
	internal void BeginChange()
	{
		BeginChange(true /* undo */);
	}

	internal void BeginChangeNoUndo()
	{
		BeginChange(false /* undo */);
	}

	private void BeginChange(bool undo)
	{

	}

	internal void EndChange()
	{
		EndChange(false /* skipEvents */);
	}

	internal void EndChange(bool skipEvents)
	{
		if (ChangedHandler != null && !skipEvents)
		{
			ChangedHandler(this, null);
		}
	}
	#endregion

	#region Private Fields
	public EventHandler ChangedHandler;

	internal P Document = new();

	internal StringProperty StringProperty;

	private TextArguments _arguments;
	#endregion


	#region Static Methods
	public static string? Cut(string text)
	{
		if (text is null) return null;

		// remove tags
		var CopyTxt = WebUtility.HtmlDecode(text);
		CopyTxt = new Regex(@"<\s*br\s*/\s*>").Replace(CopyTxt, "\n");
		return new Regex(@"<.*?>").Replace(CopyTxt, "");
	}
	#endregion
}