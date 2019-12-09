using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core.Modules.Physics.Events
{
    /// <summary>
    /// Event arguments that are passed with COLLISION_OCCURRED event
    /// </summary>
    public class CollisionEventArgs : EventArgs
    {
        #region Public Constructors

        public CollisionEventArgs(IEntity entity)
        {
            Entity = entity;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }

        #endregion Public Properties
    }
}