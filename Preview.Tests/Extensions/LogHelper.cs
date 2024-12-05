using System.IO;
using System.Text;

namespace Xylia.Preview.Tests.Extensions;
internal class LogHelper : TextWriter
{
	#region Constructor
	private readonly RichTextBox _output;

	public LogHelper(RichTextBox output)
	{
		_output = output;
		Console.SetOut(this);
	}
	#endregion

	#region Methods
	public override Encoding Encoding => Encoding.UTF8;

	public override void WriteLine(string value)
	{
		var builder = new StringBuilder();
		builder.Append(DateTime.Now.ToString("T"));
		builder.Append(' ');

		if (!string.IsNullOrEmpty(value))
		{
			builder.Append(value.Replace("\n", "\n" + new string(' ', 10)) + "\r\n");
		}

		_output.AppendText(builder.ToString());
	}

	public override void WriteLine(bool value) => WriteLine(value.ToString());

	public override void WriteLine(int value) => WriteLine(value.ToString());

	public override void WriteLine(uint value) => WriteLine(value.ToString());

	public override void WriteLine(long value) => WriteLine(value.ToString());

	public override void WriteLine(ulong value) => WriteLine(value.ToString());

	public override void WriteLine(float value) => WriteLine(value.ToString());

	public override void WriteLine(double value) => WriteLine(value.ToString());

	public override void WriteLine(decimal value) => WriteLine(value.ToString());
	#endregion
}