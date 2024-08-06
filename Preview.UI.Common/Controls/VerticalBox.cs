using System.Windows;

namespace Xylia.Preview.UI.Controls;

// layout by slot in UMG, we ignore this temporarily
public class CanvasPanel : UserWidget
{
	// base
}

public class Overlay : UserWidget
{
	// FLayoutData.Offset Padding;
	// VerticalAlignment
	// HorizontalAlignment

	protected override Size MeasureOverride(Size constraint)
	{
		Size desiredSize = new Size();

		foreach (UIElement child in Children)
		{
			if (child == null) continue;

			// Measure the child.
			child.Measure(constraint);

			desiredSize.Width = Math.Max(desiredSize.Width, child.DesiredSize.Width);
			desiredSize.Height += child.DesiredSize.Height;
		}

		return desiredSize;
	}

	protected override Size ArrangeOverride(Size arrangeSize)
	{
		Rect rcChild = new Rect(arrangeSize);
		double previousChildSize = 0.0;

		foreach (UIElement child in Children)
		{
			if (child == null) continue;
			if (child is FrameworkElement element)
			{
				element.HorizontalAlignment = this.HorizontalAlignment;
				element.VerticalAlignment = this.VerticalAlignment;
			}

			rcChild.Y += previousChildSize;
			rcChild.Height = previousChildSize = child.DesiredSize.Height;
			rcChild.Width = Math.Max(arrangeSize.Width, child.DesiredSize.Width);

			child.Arrange(rcChild);
		}

		return arrangeSize;
	}
}

public class VerticalBox : UserWidget
{
	// VerticalAlignment
	// Size: Auto / Fill

	protected override Size MeasureOverride(Size constraint)
	{
		Size desiredSize = new Size();
		Size layoutSlotSize = constraint with { Height = double.PositiveInfinity };

		foreach (UIElement child in Children)
		{
			if (child == null) continue;

			// Measure the child.
			child.Measure(layoutSlotSize);

			desiredSize.Width = Math.Max(desiredSize.Width, child.DesiredSize.Width);
			desiredSize.Height += child.DesiredSize.Height;
		}

		return desiredSize;
	}

	protected override Size ArrangeOverride(Size arrangeSize)
	{
		Rect rcChild = new Rect(arrangeSize);
		double previousChildSize = 0.0;

		foreach (UIElement child in Children)
		{
			if (child == null) continue;

			rcChild.Y += previousChildSize;
			rcChild.Height = previousChildSize = child.DesiredSize.Height;
			rcChild.Width = Math.Max(arrangeSize.Width, child.DesiredSize.Width);

			child.Arrange(rcChild);
		}

		return arrangeSize;
	}
}

public class HorizontalBox : UserWidget
{
	// HorizontalAlignment
	// Size: Auto / Fill

	protected override Size MeasureOverride(Size constraint)
	{
		Size desiredSize = new Size();
		Size layoutSlotSize = constraint with { Width = double.PositiveInfinity };

		foreach (UIElement child in Children)
		{
			if (child == null) continue;

			// Measure the child.
			child.Measure(layoutSlotSize);

			desiredSize.Width += child.DesiredSize.Width;
			desiredSize.Height = Math.Max(desiredSize.Height, child.DesiredSize.Height);
		}

		return desiredSize;
	}

	protected override Size ArrangeOverride(Size arrangeSize)
	{
		Rect rcChild = new Rect(arrangeSize);
		double previousChildSize = 0.0;

		foreach (UIElement child in Children)
		{
			if (child == null) continue;

			rcChild.X += previousChildSize;
			rcChild.Width = previousChildSize = child.DesiredSize.Width;
			rcChild.Height = Math.Max(arrangeSize.Height, child.DesiredSize.Height);

			child.Arrange(rcChild);
		}

		return arrangeSize;
	}
}