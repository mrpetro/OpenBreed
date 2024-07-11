using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Physics.Interface.Extensions
{
    public static class BoxShapeExtensions
    {
        #region Public Methods

        public static Box2 ToBox2(this IBoxShape shape)
        {
            return new Box2(shape.X, shape.Y, shape.X + shape.Width, shape.Y + shape.Height);
        }

        #endregion Public Methods
    }
}