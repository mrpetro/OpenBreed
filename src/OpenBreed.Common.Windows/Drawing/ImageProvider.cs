using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Tools;
using OpenBreed.Common.Windows.Drawing.Wrappers;
using OpenBreed.Common.Windows.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Drawing
{
    internal class ImageProvider : IImageProvider
    {
        public IImage FromFile(string filePath)
        {
            return new ImageWrapper((Bitmap)Bitmap.FromFile(filePath));
        }
    }
}
