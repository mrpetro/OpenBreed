using System;
using System.Collections.Generic;
using System.Data.Common;
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

        #region Public Methods

        public static MyPoint operator +(MyPoint a, MyPoint b)
        {
            return new MyPoint(a.X + b.X, a.Y + b.Y);
        }

        public static MyPoint operator -(MyPoint a, MyPoint b)
        {
            return new MyPoint(a.X - b.X, a.Y - b.Y);
        }

        public static MyPoint operator /(MyPoint a, int d)
        {
            return new MyPoint(a.X / d, a.Y / d);
        }

        public static MyPoint operator *(MyPoint a, int d)
        {
            return new MyPoint(a.X * d, a.Y * d);
        }

        public static MyPoint operator *(MyPoint a, MyPoint b)
        {
            return new MyPoint(a.X * b.X, a.Y * b.Y);
        }

        #endregion Public Methods
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

        #region Public Methods

        public static MyPointF operator +(MyPointF a, MyPointF b)
        {
            return new MyPointF(a.X + b.X, a.Y + b.Y);
        }

        public static MyPointF operator -(MyPointF a, MyPointF b)
        {
            return new MyPointF(a.X - b.X, a.Y - b.Y);
        }

        public static MyPointF operator /(MyPointF a, float d)
        {
            return new MyPointF(a.X / d, a.Y / d);
        }

        public static MyPointF operator *(MyPointF a, float d)
        {
            return new MyPointF(a.X * d, a.Y * d);
        }

        public static MyPointF operator *(MyPointF a, MyPointF b)
        {
            return new MyPointF(a.X * b.X, a.Y * b.Y);
        }

        #endregion Public Methods
    }
}