using OpenBreed.Wecs.Entities;
using System;

namespace OpenBreed.Wecs.Systems.Physics.Events
{
    /// <summary>
    /// Event args for event that occurs when entity body collision disables
    /// </summary>
    public class BodyOffEventArgs : EventArgs
    {
        #region Public Constructors

        public BodyOffEventArgs(IEntity entity)
        {
            Entity = entity;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }

        #endregion Public Properties
    }
}