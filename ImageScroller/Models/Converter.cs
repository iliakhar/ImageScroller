using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System.Globalization;
using System.IO;

namespace ImageScroller.Models
{


    public class Converter : IValueConverter
    {
        public static Converter Instance = new Converter();
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value is string rawUri && targetType.IsAssignableFrom(typeof(Bitmap)))
            {
                if (File.Exists(rawUri)) return new Bitmap(rawUri);
                else return null;
            }
            throw new NotSupportedException();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
