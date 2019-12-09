using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Events
{
    /// <summary>
    /// Event arguments that are passed with ENTITY_REMOVED_FROM_WORLD event
    /// </summary> 
    public class EntityRemovedFromWorldEventArgs : EventArgs
    {
        #region Public Constructors

        public EntityRemovedFromWorldEventArgs(IEntity entity, World world)
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
