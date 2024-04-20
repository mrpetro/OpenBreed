using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Drawing
{
    public struct MySize
    {
        #region Public Constructors

        public MySize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        #endregion Public Constructors

        #region Private Properties

        public int Width { get; set; }
        public int Height { get; set; }

        #endregion Private Properties
    }
}