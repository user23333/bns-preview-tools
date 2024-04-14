using System.Windows.Media.Imaging;

namespace Xylia.Preview.UI.Views.Editor;
internal static class ImageHelper
{
	public static BitmapImage Table { get; } = new BitmapImage(new Uri("/Resources/Images/table2.png", UriKind.Relative));

	public static BitmapImage TableSys { get; } = new BitmapImage(new Uri("/Resources/Images/table_set.png", UriKind.Relative));

	public static BitmapImage Database { get; } = new BitmapImage(new Uri("/Resources/Images/database.png", UriKind.Relative));

	public static BitmapImage Folder { get; } = new BitmapImage(new Uri("/Resources/Images/folder.png", UriKind.Relative));
}