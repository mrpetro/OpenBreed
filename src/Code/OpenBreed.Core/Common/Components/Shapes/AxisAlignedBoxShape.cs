using OpenBreed.Core.Entities;
using OpenTK;
using System;

namespace OpenBreed.Core.Common.Components.Shapes
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

            Aabb = new Box2(0, Height, Width, 0);
        }

        #endregion Public Constructors

        #region Public Properties

        public Box2 Aabb { get; }
        public float Width { get; }
        public float Height { get; }

        public Type SystemType { get { return null; } }

        #endregion Public Properties

        #region Public Methods

        public void Initialize(IEntity entity)
        {
        }

        public void Deinitialize(IEntity entity)
        {
        }

        #endregion Public Methods
    }
}