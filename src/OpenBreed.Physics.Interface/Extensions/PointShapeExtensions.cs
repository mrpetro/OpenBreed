using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Physics.Interface.Extensions
{
    public static class PointShapeExtensions
    {
        #region Public Methods

        public static Vector2 ToVector2(this IPointShape shape)
        {
            return new Vector2(shape.X, shape.Y);
        }

        #endregion Public Methods
    }
}
