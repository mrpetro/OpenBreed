using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Drawing
{
    public enum MyImageFormat
    {
        Bmp
    }

    public interface IImage : IDisposable
    {
        #region Public Properties

        int Height { get; }
        int Width { get; }

        IColorPalette Palette { get; set; }

        #endregion Public Properties

        #region Public Methods

        byte[] GetBytes();

        void Save(MemoryStream ms, MyImageFormat format);

        #endregion Public Methods
    }
}