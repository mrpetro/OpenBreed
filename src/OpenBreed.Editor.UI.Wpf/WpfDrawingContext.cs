using OpenBreed.Editor.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Tools.Collections;
using System.Globalization;

namespace OpenBreed.Editor.UI.Wpf
{
    internal class WpfDrawingContext : IDrawingContext
    {
        #region Private Fields

        private readonly DrawingContext dc;

        private readonly Dictionary<IImage, BitmapSource> mapper = new Dictionary<IImage, BitmapSource>();

        #endregion Private Fields

        #region Public Constructors

        public WpfDrawingContext(DrawingContext dc)
        {
            this.dc = dc;
        }

        public IMatrix Transform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion Public Constructors

        #region Public Methods

        public void Clear(MyColor color)
        {
        }

        public void Dispose()
        {
            dc.Close();
        }

        public void DrawImage(IBitmap bitmap, int x, int y, MyRectangle srcRect)
        {
            var bitmapSource = GetBitmapSource(bitmap);

            var cropRect = DrawingHelpers.ToInt32Rect(srcRect);
            var croppedBi = new CroppedBitmap(bitmapSource, cropRect);
            var rect = new Rect(x, y, srcRect.Width, srcRect.Height);
            dc.DrawImage(croppedBi, rect);
        }

        public void DrawImage(IBitmap bitmap, int x, int y, int width, int height)
        {
            var bitmapSource = GetBitmapSource(bitmap);

            var rect = new Rect(x, y, width, height);
            dc.DrawImage(bitmapSource, rect);
        }

        public void DrawImage(IImage bitmap, float x, float y, float width, float height)
        {
            var bitmapSource = GetBitmapSource(bitmap);

            var rect = new Rect(x, y, width, height);
            dc.DrawImage(bitmapSource, rect);
        }

        public void DrawImage(IImage bitmap, int x, int y, MyRectangle srcRect)
        {
            var bitmapSource = GetBitmapSource(bitmap);

            var cropRect = DrawingHelpers.ToInt32Rect(srcRect);
            var croppedBi = new CroppedBitmap(bitmapSource, cropRect);
            var rect = new Rect(x, y, srcRect.Width, srcRect.Height);
            dc.DrawImage(croppedBi, rect);
        }

        public void DrawImage(IImage bitmap, float x, float y, MyRectangle srcRect)
        {
            var bitmapSource = GetBitmapSource(bitmap);

            var cropRect = DrawingHelpers.ToInt32Rect(srcRect);
            var croppedBi = new CroppedBitmap(bitmapSource, cropRect);
            var rect = new Rect(x, y, srcRect.Width, srcRect.Height);
            dc.DrawImage(croppedBi, rect);
        }

        public void DrawRectangle(IPen pen, MyRectangle rectangle)
        {
            var colorM = System.Windows.Media.Color.FromArgb(pen.Color.A, pen.Color.R, pen.Color.G, pen.Color.B);
            var brushM = new System.Windows.Media.SolidColorBrush(colorM);
            var penM = new System.Windows.Media.Pen(brushM, 1);
            var rectangleM = DrawingHelpers.ToRect(rectangle);
            dc.DrawRectangle(null, penM, rectangleM);
        }

        public void DrawString(string text, IFont font, IBrush brush, float x, float y)
        {
            var color = Color.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);

            var brushM = new SolidColorBrush(color);

            var formattedText = new FormattedText(
                    text,
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface(font.Name),
                    font.Size,
                    brushM);

            dc.DrawText(formattedText, new System.Windows.Point(x, y));
        }

        public void FillRectangle(IBrush brush, MyRectangle rectangle)
        {
            var color = Color.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);
            var brushM = new SolidColorBrush(color);
            var rectangleM = DrawingHelpers.ToRect(rectangle);

            dc.DrawRectangle(brushM, null, rectangleM);
        }

        public void Flush()
        {

        }

        public void Setup()
        {
            //dc.PixelOffsetMode = PixelOffsetMode.Half;
            //dc.CompositingQuality = CompositingQuality.AssumeLinear;
            //dc.CompositingMode = CompositingMode.SourceOver;
            //dc.SmoothingMode = SmoothingMode.None;
            //dc.InterpolationMode = InterpolationMode.NearestNeighbor;
        }

        #endregion Public Methods

        #region Private Methods

        private BitmapSource GetBitmapSource(IImage image)
        {
            if (!mapper.TryGetValue(image, out BitmapSource bitmapSource))
            {
                bitmapSource = DrawingHelpers.ToBitmapImage(image);
                mapper.Add(image, bitmapSource);
            }

            return bitmapSource;
        }

        #endregion Private Methods
    }
}