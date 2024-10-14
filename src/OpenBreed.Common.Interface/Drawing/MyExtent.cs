using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Drawing
{
    public class MyExtent
    {
        #region Public Fields

        public MyPoint Min = new MyPoint(int.MaxValue, int.MaxValue);

        public MyPoint Max = new MyPoint(int.MinValue, int.MinValue);

        public MyPoint Size => new MyPoint(Max.X - Min.X + 1, Max.Y - Min.Y + 1);

        #endregion Public Fields

        #region Public Constructors

        public MyExtent()
        {

        }

        public MyExtent(MyPoint min, MyPoint max)
        {
            Min = min;
            Max = max;
        }

        #endregion Public Constructors

        #region Public Methods

        public MyPoint Center
        {
            get
            {
                return (Min + Max) / 2;
            }
        }

        public void Expand(MyPoint point)
        {
            Min.X = Math.Min(Min.X, point.X);
            Min.Y = Math.Min(Min.Y, point.Y);
            Max.X = Math.Max(Max.X, point.X);
            Max.Y = Math.Max(Max.Y, point.Y);
        }

        #endregion Public Methods
    }

    public class MyExtentF
    {
        #region Public Fields

        public MyPointF Min = new MyPointF(int.MaxValue, int.MaxValue);

        public MyPointF Max = new MyPointF(int.MinValue, int.MinValue);

        public MyPointF Size => new MyPointF(Max.X - Min.X + 1, Max.Y - Min.Y + 1);

        #endregion Public Fields

        #region Public Constructors

        public MyExtentF()
        {

        }

        public MyExtentF(MyPointF min, MyPointF max)
        {
            Min = min;
            Max = max;
        }

        #endregion Public Constructors

        #region Public Methods

        public MyPointF Center
        {
            get
            {
                return (Min + Max) / 2;
            }
        }

        public void Expand(float x, float y)
        {
            Min.X = Math.Min(Min.X, x);
            Min.Y = Math.Min(Min.Y, y);
            Max.X = Math.Max(Max.X, x);
            Max.Y = Math.Max(Max.Y, y);
        }

        public void Expand(MyPointF point) => Expand(point.X, point.Y);

        #endregion Public Methods
    }
}