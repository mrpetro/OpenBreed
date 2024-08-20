using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace OpenBreed.Editor.UI.Wpf.Converters
{
    public class EventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case MouseButtonEventArgs mb:

                    var pos = mb.GetPosition((IInputElement)mb.OriginalSource);
                    return ((int)pos.X, (int)pos.Y);
                default:
                    break;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
