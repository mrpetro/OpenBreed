using System;
using System.Drawing;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class TextMeasurer : IDisposable
    {
        #region Private Fields

        private bool disposedValue;
        private Bitmap bmp;
        private Graphics gfx;

        #endregion Private Fields

        #region Public Constructors

        public TextMeasurer()
        {
            bmp = new Bitmap(512, 512);
            gfx = Graphics.FromImage(bmp);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Internal Methods

        internal float MeasureWidth(Font font, char c)
        {
            var stringFormat = new StringFormat(StringFormat.GenericTypographic)
            {
                FormatFlags = StringFormatFlags.MeasureTrailingSpaces
            };
            return gfx.MeasureString(c.ToString(), font, 0, stringFormat).Width;
        }

        internal SizeF MeasureSize(Font font, char c)
        {
            return gfx.MeasureString(c.ToString(), font);
        }

        internal float MeasureHeight(Font font)
        {
            return font.GetHeight(gfx);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    gfx.Dispose();
                    gfx = null;
                    bmp.Dispose();
                    bmp = null;
                }

                disposedValue = true;
            }
        }

        #endregion Protected Methods
    }
}