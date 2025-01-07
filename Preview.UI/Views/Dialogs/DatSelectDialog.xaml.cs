using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.UI.Views.Dialogs;
public partial class DatSelectDialog : IDatSelect
{
	#region Constructor
	private IEnumerable<FileInfo>? list_xml;
	private IEnumerable<FileInfo>? list_local;
	private Locale locale;

	private DatSelectDialog()
	{
		InitializeComponent();

		CountDown = new DispatcherTimer();
		CountDown.Interval = new TimeSpan(500);
		CountDown.Tick += CountDown_Tick;

		NoResponse = new DispatcherTimer();
		NoResponse.Interval = new TimeSpan(1000);
		NoResponse.Tick += NoResponse_Tick;
	}
	#endregion

	#region CountDown
	private readonly DispatcherTimer CountDown;
	private readonly DispatcherTimer NoResponse;

	DateTime StartTime = DateTime.Now;
	DateTime LastActTime = DateTime.Now;

	const int CountDownSec = 10;
	const int NoResponseSec = 15;

	private void StartCountDown()
	{
		TimeInfo.Text = null;
		StartTime = DateTime.Now;

		this.CountDown.IsEnabled = true;
		this.TimeInfo.Visibility = Visibility.Visible;
	}

	private void StopCountDown()
	{
		this.CountDown.IsEnabled = false;
		this.TimeInfo.Visibility = Visibility.Hidden;
	}

	private void CountDown_Tick(object? sender, EventArgs e)
	{
		var RemainSec = CountDownSec - (int)DateTime.Now.Subtract(StartTime).TotalSeconds;
		TimeInfo.Text = StringHelper.Get("DatSelector_CountDown", RemainSec);

		if (RemainSec <= 0) Confirm_Click(null, null);
	}

	private void NoResponse_Tick(object? sender, EventArgs e)
	{
		var CurNoResponseSec = (int)DateTime.Now.Subtract(LastActTime).TotalSeconds;
		if (CurNoResponseSec >= NoResponseSec)
		{
			StartCountDown();
			LastActTime = DateTime.Now;
		}
	}
	#endregion

	#region Methods
	private void Window_MouseEnter(object sender, MouseEventArgs e)
	{
		StopCountDown();
		LastActTime = DateTime.Now;
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		Load(comboBox1, list_xml);
		Load(comboBox2, list_local, locale);

		StartCountDown();
		LastActTime = DateTime.Now;
		NoResponse.IsEnabled = true;
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		base.OnClosing(e);

		e.Cancel = true;
		Visibility = Visibility.Collapsed;
	}

	private void Confirm_Click(object sender, RoutedEventArgs e)
	{
		// stop timer
		NoResponse.Stop();
		CountDown.Stop();

		SelectedXml = new FileInfo(comboBox1.Text.Replace("...", @"contents\Local"));
		SelectedLocal = new FileInfo(comboBox2.Text.Replace("...", @"contents\Local"));

		TimeInfo.Visibility = Visibility.Hidden;
		DialogResult = Status = true;
	}

	private void Cancel_Click(object sender, EventArgs e)
	{
		DialogResult = Status = false;
	}

	private static void Load(ComboBox control, IEnumerable<FileInfo>? files, Locale locale = default)
	{
		control.Items.Clear();
		control.SelectedIndex = 0;

		if (files is null)
		{
			control.IsEnabled = false;
			return;
		}

		foreach (var f in files)
		{
			var s = f.FullName.Replace(@"contents\Local", "...");
			control.Items.Add(s);

			if (locale.Language != 0 && s.Contains(locale.Language + "\\data\\", StringComparison.OrdinalIgnoreCase))
				control.SelectedItem = s;
		}

		control.IsEnabled = control.Items.Count != 1;
	}
	#endregion


	#region IDatSelect
	public bool Status;
	public FileInfo? SelectedXml;
	public FileInfo? SelectedLocal;

	DefaultProvider IDatSelect.Show(IEnumerable<FileInfo> xmls, IEnumerable<FileInfo> locals, Locale locale)
	{
		this.list_xml = xmls;
		this.list_local = locals;
		this.locale = locale;

		return Application.Current.Dispatcher.Invoke(() =>
		{
			ShowDialog();
			if (Status) return new DefaultProvider(SelectedXml, SelectedLocal);
			throw new OperationCanceledException();
		});
	}

	internal static DatSelectDialog Instance => Application.Current.Dispatcher.Invoke(() => new DatSelectDialog());
	#endregion
}