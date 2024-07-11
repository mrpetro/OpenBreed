using System;

namespace OpenBreed.Common.Interface.Drawing
{
    public struct MyRectangle
    {
        #region Public Fields

        public int X;
        public int Y;
        public int Width;
        public int Height;

        #endregion Public Fields

        #region Public Constructors

        public MyRectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public MyRectangle(MyPoint position, MySize size)
        {
            X = position.X;
            Y = position.Y;
            Width = size.Width;
            Height = size.Height;
        }

        #endregion Public Constructors

        #region Public Properties

        public readonly int Left => X;
        public readonly int Top => Y;
        public readonly int Right => X + Width;
        public readonly int Bottom => Y + Height;

        #endregion Public Properties

        #region Public Methods

        public static MyRectangle FromLTRB(int left, int top, int right, int bottom) =>
            new MyRectangle(left, top, unchecked(right - left), unchecked(bottom - top));

        public static bool operator ==(MyRectangle left, MyRectangle right) =>
            left.X == right.X && left.Y == right.Y && left.Width == right.Width && left.Height == right.Height;

        public static bool operator !=(MyRectangle left, MyRectangle right) => !(left == right);

        public bool Contains(MyPoint point) => Contains(point.X, point.Y);

        public override readonly bool Equals(object obj) => obj is MyRectangle && Equals((MyRectangle)obj);

        public readonly bool Equals(MyRectangle other) => this == other;

        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Width, Height);

        public bool Contains(int x, int y)
        {
            return X <= x && x < X + Width && Y <= y && y < Y + Height;
        }

        #endregion Public Methods
    }

    public struct MyRectangleF
    {
        #region Public Fields

        public float X;
        public float Y;
        public float Width;
        public float Height;

        #endregion Public Fields

        #region Public Constructors

        public MyRectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        #endregion Public Constructors

        #region Public Properties

        public readonly float Left => X;
        public readonly float Top => Y;
        public readonly float Right => X + Width;
        public readonly float Bottom => Y + Height;

        #endregion Public Properties

        #region Public Methods

        public static MyRectangleF FromLTRB(int left, int top, int right, int bottom) =>
            new MyRectangleF(left, top, unchecked(right - left), unchecked(bottom - top));

        public bool Contains(MyPointF point) => Contains(point.X, point.Y);

        public bool Contains(float x, float y)
        {
            return X <= x && x < X + Width && Y <= y && y < Y + Height;
        }

        #endregion Public Methods
    }
}