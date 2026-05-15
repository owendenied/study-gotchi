using System;
using System.Collections.Generic;
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
        private static readonly Dictionary<string, BitmapImage> Cache = new();
        private static readonly object CacheLock = new();

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
            {
                try
                {
                    lock (CacheLock)
                    {
                        if (Cache.TryGetValue(path, out var cached))
                        {
                            return cached;
                        }
                    }

                    var uri = new Uri(path, UriKind.RelativeOrAbsolute);
                    if (!uri.IsAbsoluteUri)
                        uri = new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));

                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = uri;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    if (bitmap.CanFreeze) bitmap.Freeze();
                    lock (CacheLock)
                    {
                        Cache[path] = bitmap;
                    }
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
