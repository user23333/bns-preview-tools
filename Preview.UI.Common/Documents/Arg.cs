using System.Windows;
using CUE4Parse.BNS.Assets.Exports;

namespace Xylia.Preview.UI.Documents;
public class Arg : BaseElement<Data.Models.Document.Arg>
{
	#region Methods
	protected override Size MeasureCore(Size availableSize)
	{
		this.Children = [];

		var result = Element!.GetObject(Arguments);
		if (result is null) return new Size();
		else if (result is ImageProperty property) Children.Add(new Image(property));
		else if (result is int @int) Children.Add(new Run() { Text = @int.ToString("N0") });
		else if (result is not null) Children.Add(new P(result.ToString()));

		return base.MeasureCore(availableSize);
	}
	#endregion
}