﻿using System.Windows;

namespace Xylia.Preview.UI.Documents;
public class BR : BaseElement<Data.Models.Document.BR>
{
	protected override Size MeasureCore(Size availableSize) => new(0, FontSize);
}