using OpenBreed.Core.Entities;
using OpenTK;
using System;

namespace OpenBreed.Core.Modules.Physics.Events
{
    /// <summary>
    /// Event arguments that are passed with COLLISION_OCCURRED event
    /// </summary>
    public class CollisionEventArgs : EventArgs
    {
        #region Public Constructors

        public CollisionEventArgs(IEntity entity, Vector2 projection)
        {
            Entity = entity;
            Projection = projection;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }

        public Vector2 Projection { get; }

        #endregion Public Properties
    }
}