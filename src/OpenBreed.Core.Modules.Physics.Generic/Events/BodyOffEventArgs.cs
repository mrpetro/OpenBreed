using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core.Modules.Physics.Events
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