using OpenBreed.Game.Entities;
using OpenTK;
using System;

namespace OpenBreed.Game.Common.Components
{
    public class Direction : IEntityComponent
    {
        #region Public Constructors

        public Direction(Vector2 value)
        {
            X = value.X;
            Y = value.Y;
        }

        public Direction(float x, float y)
        {
            X = x;
            Y = y;
        }

        #endregion Public Constructors

        #region Public Properties

        public float X { get; set; }
        public float Y { get; set; }
        public Type SystemType { get { return null; } }

        #endregion Public Properties

        #region Public Methods

        public void Deinitialize(IEntity entity)
        {
        }

        public void Initialize(IEntity entity)
        {
        }

        #endregion Public Methods
    }
}