using OpenBreed.Core.Entities;
using OpenTK;
using System;

namespace OpenBreed.Core.Modules.Physics.Events
{
    /// <summary>
    /// Event args for event that occurs when two entities are colliding
    /// </summary>
    public class CollisionEventArgs : EventArgs
    {
        #region Public Constructors

        public CollisionEventArgs(Entity entity, Vector2 projection)
        {
            Entity = entity;
            Projection = projection;
        }

        #endregion Public Constructors

        #region Public Properties

        public Entity Entity { get; }

        public Vector2 Projection { get; }

        #endregion Public Properties
    }
}