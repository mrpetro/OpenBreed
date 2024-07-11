using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Drawing
{
    public interface IBitmapProvider
    {
        IImage ErrorIcon { get; }


        #region Public Methods

        IImage FromBytes(int width, int height, byte[] bytes);

        void SetPaletteColors(IImage sourceImage, MyColor[] data);

        byte[] ToBytes(IImage sourceImage, MyRectangle sourceRectangle);

        #endregion Public Methods
    }
}