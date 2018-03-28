using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace FileExplorer
{
    /// <summary>
    /// Converts a full path to a specific image type of a drive, folder or file
    /// </summary>
    [ValueConversion(typeof(DirectoryItemType), typeof(BitmapImage))]
    class HeaderToImageConverter : IMultiValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var isExpanded = (bool)value[1];

            // By default, it will be presumed to be an image
            var image = "Images/file.png";

            switch ((DirectoryItemType)value[0])
            {
                case DirectoryItemType.Drive:
                    image = "Images/drive.png";
                    break;

                case DirectoryItemType.Folder:
                    image = isExpanded == true ? "Images/folder-opened.png" : "Images/folder-closed.png";
                    break;
            }

            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
