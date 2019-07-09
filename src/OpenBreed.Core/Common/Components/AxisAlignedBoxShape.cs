using OpenBreed.Core.Entities;
using OpenTK;
using System;

namespace OpenBreed.Core.Common.Systems.Components
{
    /// <summary>
    /// Box shape which is always axis aligned (can't be oriented)
    /// </summary>
    public class AxisAlignedBoxShape : IShapeComponent
    {
        #region Public Constructors

        public AxisAlignedBoxShape(float width, float height)
        {
            Width = width;
            Height = height;

            Aabb = new Box2(- Width / 2, Height / 2, Width / 2, - Height / 2);
        }

        #endregion Public Constructors

        #region Public Properties

        public Box2 Aabb { get; }
        public float Width { get; }
        public float Height { get; }

        #endregion Public Properties
    }
}