using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Drawing
{
    public struct MyPoint
    {
        #region Public Fields

        public int X;
        public int Y;

        #endregion Public Fields

        #region Public Constructors

        public MyPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        #endregion Public Constructors
    }

    public struct MyPointF
    {
        #region Public Fields

        public float X;
        public float Y;

        #endregion Public Fields

        #region Public Constructors

        public MyPointF(float x, float y)
        {
            X = x;
            Y = y;
        }

        #endregion Public Constructors
    }
}