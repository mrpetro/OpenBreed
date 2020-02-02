using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core.Events
{
    /// <summary>
    /// Event arguments that are passed with ENTITY_ENTERED_WORLD event
    /// </summary>
    public class EntityEnteredWorldEventArgs : EventArgs
    {
        #region Public Constructors

        public EntityEnteredWorldEventArgs(IEntity entity, World world)
        {
            Entity = entity;
            World = world;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }
        public World World { get; }

        #endregion Public Properties
    }
}