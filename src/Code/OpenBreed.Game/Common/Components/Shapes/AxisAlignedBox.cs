using OpenBreed.Game.Entities;
using OpenBreed.Game.Physics.Helpers;
using OpenTK;
using System;

namespace OpenBreed.Game.Common.Components.Shapes
{
    /// <summary>
    /// Box which is always axis aligned (can't be oriented)
    /// </summary>
    public class AxisAlignedBox : IShapeComponent
    {
        #region Public Constructors

        public AxisAlignedBox(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public Box2 Aabb
        {
            get
            {
                return new Box2
                {
                    Left = 0,
                    Bottom = 0,
                    Right = Width,
                    Top = Height,
                };
            }
        }

        #endregion Public Constructors

        #region Public Properties

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