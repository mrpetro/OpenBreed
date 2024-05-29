using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Drawing.Wrappers
{
    public class ImageWrapper : IImage
    {
        #region Private Fields

        private ColorPaletteWrapper _palette;

        #endregion Private Fields

        #region Public Constructors

        public ImageWrapper(System.Drawing.Bitmap wrapped)
        {
            Wrapped = wrapped;
        }

        #endregion Public Constructors

        #region Public Properties

        public System.Drawing.Bitmap Wrapped { get; }

        public IColorPalette Palette
        {
            get
            {
                return new ColorPaletteWrapper(Wrapped.Palette);
            }

            set
            {
                Wrapped.Palette = ((ColorPaletteWrapper)value).Wrapped;
            }
        }

        public int Height => Wrapped.Height;

        public int Width => Wrapped.Width;

        #endregion Public Properties

        #region Public Methods

        public void Dispose() => Wrapped.Dispose();

        public byte[] GetBytes()
        {
            return BitmapHelper.ToBytes(Wrapped);
        }

        public void Save(MemoryStream ms, MyImageFormat format)
        {
            Wrapped.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        #endregion Public Methods
    }
}
