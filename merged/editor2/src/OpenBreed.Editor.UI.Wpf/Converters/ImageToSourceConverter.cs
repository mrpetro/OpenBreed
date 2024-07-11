using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OpenBreed.Editor.UI.Wpf.Converters
{
    public class ImageToSourceConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (value is not IImage image)
            {
                return null;
            }

            return DrawingHelpers.ToBitmapImage(image);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
