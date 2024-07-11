using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Drawing
{
    public interface IBitmapData
    {
        #region Public Properties

        public int Height { get; set; }
        public int Width { get; set; }
        public MyPixelFormat PixelFormat { get; set; }
        public int Reserved { get; set; }
        public System.IntPtr Scan0 { get; set; }
        public int Stride { get; set; }

        #endregion Public Properties
    }


}