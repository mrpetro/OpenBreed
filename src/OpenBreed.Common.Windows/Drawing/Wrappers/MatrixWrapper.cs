using OpenBreed.Common.Interface.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Common.Windows.Drawing.Converters;

namespace OpenBreed.Common.Windows.Drawing.Wrappers
{
    public class MatrixWrapper : IMatrix
    {
        #region Public Constructors

        public MatrixWrapper(Matrix matrix)
        {
            Wrapped = matrix;
        }

        #endregion Public Constructors

        #region Public Properties

        public float[] Elements => Wrapped.Elements;

        #endregion Public Properties

        #region Internal Properties

        internal Matrix Wrapped { get; }

        #endregion Internal Properties

        #region Public Methods

        public IMatrix Clone() => new MatrixWrapper(Wrapped.Clone());

        public void Invert()
        {
            Wrapped.Invert();
        }

        public void Reset()
        {
            Wrapped.Reset();
        }

        public void Scale(float x, float y)
        {
            Wrapped.Scale(x, y);
        }

        public void TransformPoints(MyPoint[] points)
        {
            var pts = points.Select(item => new Point(item.X, item.Y)).ToArray();
            Wrapped.TransformPoints(pts);

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new MyPoint(pts[i].X, pts[i].Y);
            }
        }

        public void TransformPoints(MyPointF[] points)
        {
            var pts = points.Select(item => new PointF(item.X, item.Y)).ToArray();
            Wrapped.TransformPoints(pts);

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new MyPointF(pts[i].X, pts[i].Y);
            }
        }

        public void Translate(float x, float y)
        {
            Wrapped.Translate(x, y);
        }

        #endregion Public Methods
    }
}