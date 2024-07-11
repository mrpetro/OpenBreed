using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OpenBreed.Editor.UI.Wpf
{
    public static class DrawingHelpers
    {
        #region Public Methods

        public static BitmapImage ToBitmapImage(IImage image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, MyImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();
                return bi;
            }
        }

        public static Int32Rect ToInt32Rect(MyRectangle rectangle)
        {
            return new Int32Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static Rect ToRect(MyRectangle rectangle)
        {
            return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static MyPoint ToPoint(System.Windows.Point point)
        {
            return new MyPoint((int)point.X, (int)point.Y);
        }

        #endregion Public Methods
    }
}