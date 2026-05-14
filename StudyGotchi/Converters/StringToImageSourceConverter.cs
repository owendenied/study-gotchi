using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows;

namespace StudyGotchi.Converters
{
    /// <summary>
    /// Converts a file path string to a BitmapImage for use with WpfAnimatedGif's AnimatedSource.
    /// </summary>
    public class StringToImageSourceConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
            {
                try
                {
                    var uri = new Uri(path, UriKind.RelativeOrAbsolute);
                    if (!uri.IsAbsoluteUri)
                        uri = new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));

                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = uri;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    return bitmap;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
