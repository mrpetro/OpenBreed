using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Editor.VM.Renderer
{
    public static class DrawingConversion
    {
        #region Public Methods

        public static Rectangle ToRectangle(MyRectangle rectangle)
        {
            return new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static MyRectangle ToMyRectangle(Rectangle rectangle)
        {
            return new MyRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        #endregion Public Methods
    }
}