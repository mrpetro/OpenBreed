using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Windows.Extensions
{
    public static class MyRectangleExtensions
    {
        #region Public Methods

        public static Rectangle ToRectangle(this MyRectangle rectangle)
        {
            return new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static MyRectangle ToMyRectangle(this Rectangle rectangle)
        {
            return new MyRectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        #endregion Public Methods
    }
}