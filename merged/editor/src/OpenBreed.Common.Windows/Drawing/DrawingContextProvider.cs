using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Common.Windows.Drawing.Wrappers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Drawing
{
    internal class DrawingContextProvider : IDrawingContextProvider
    {
        public IDrawingContext FromImage(IBitmap bitmap)
        {
            return new DrawingContext(Graphics.FromImage(((ImageWrapper)bitmap).Wrapped));
        }
    }
}
