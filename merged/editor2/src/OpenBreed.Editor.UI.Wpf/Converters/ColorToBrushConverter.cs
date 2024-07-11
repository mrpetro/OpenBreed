using OpenBreed.Common.Interface.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OpenBreed.Editor.UI.Wpf.Converters
{
    internal class ColorToBrushConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not MyColor color)
            {
                return new SolidColorBrush();
            }

            return new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion Public Methods
    }
}