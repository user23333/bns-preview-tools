using System.ComponentModel;
using System.Windows.Controls;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Views.Selector;
[DesignTimeVisible(false)]
public partial class UpdateLogDialog
{
	public UpdateLogDialog()
	{
		InitializeComponent();

		//Holder.SelectedIndex = 0;
		Holder.ItemsSource = StringHelper.Get("Application_UpdateLog").Trim()
			.Split("[", StringSplitOptions.RemoveEmptyEntries)
			.Select(s => new SubLog(s.Split(']', 2, StringSplitOptions.TrimEntries)));
	}

	private void Holder_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		Holder.ScrollIntoView(e.AddedItems[0]);
	}


	private class SubLog
	{
		public Version? Version { get; set; }

		public DateTime? Time { get; set; }

		public string? Description { get; set; }


		public SubLog(string title, string description)
		{
			var strings = title.Split(',')
				.Where(x => x.Contains('='))
				.ToDictionary(
					x => x[..x.IndexOf('=')].ToLower(),
					x => x[(x.IndexOf('=') + 1)..]);

			// Replace with detail version
			var ver = strings.GetValueOrDefault("ver", title);
			Version = ver == VersionHelper.Version ? VersionHelper.InternalVersion : Version.Parse(ver);
			Description = description;

			// MetaData
			if (strings.TryGetValue("time", out var time)) Time = DateTime.Parse(time);
		}

		public SubLog(string[] strings) : this(
			strings.ElementAt(0), 
			strings.ElementAt(1)) { }
	}
}