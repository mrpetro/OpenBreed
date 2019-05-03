using OpenBreed.Game.Entities;
using OpenTK;
using System;

namespace OpenBreed.Game.Common.Components
{
    public class Position : IEntityComponent
    {
        #region Public Constructors

        public float X { get; set; }
        public float Y { get; set; }

        public Position(Vector2 value)
        {
            X = value.X;
            Y = value.Y;
        }

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        #endregion Public Constructors

        #region Public Properties

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