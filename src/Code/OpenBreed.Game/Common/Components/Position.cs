using OpenBreed.Game.Entities;
using OpenTK;
using System;

namespace OpenBreed.Game.Common.Components
{
    public class Position : IEntityComponent
    {
        #region Public Constructors

        public Position(Vector2 data)
        {
            Data = data;
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2 Data { get; set; }

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