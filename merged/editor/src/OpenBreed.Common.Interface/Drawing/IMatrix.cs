using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Drawing
{
    public interface IMatrix
    {
        #region Public Properties

        float[] Elements { get; }

        #endregion Public Properties

        #region Public Methods

        IMatrix Clone();

        void Invert();

        void Reset();

        void Scale(float x, float y);

        void TransformPoints(MyPointF[] points);

        void TransformPoints(MyPoint[] points);

        void Translate(float x, float y);

        #endregion Public Methods
    }
}