using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Systems.Physics.Events
{
    /// <summary>
    /// Event args for event that occurs when entity body collision disables
    /// </summary>
    public class BodyOffEventArgs : EventArgs
    {
        #region Public Constructors

        public BodyOffEventArgs(Entity entity)
        {
            Entity = entity;
        }

        #endregion Public Constructors

        #region Public Properties

        public Entity Entity { get; }

        #endregion Public Properties
    }
}