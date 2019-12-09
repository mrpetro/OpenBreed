using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core.Events
{
    /// <summary>
    /// Event arguments that are passed with ENTITY_ADDED_TO_WORLD event
    /// </summary>
    public class EntityAddedToWorldEventArgs : EventArgs
    {
        #region Public Constructors

        public EntityAddedToWorldEventArgs(IEntity entity, World world)
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