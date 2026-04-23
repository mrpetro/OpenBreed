using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Abstractions.Events
{
    /// <summary>
    /// Abstract event that occurs on particular world.
    /// </summary>
    public abstract class WorldEvent : EventArgs
    {
        #region Protected Constructors

        protected WorldEvent(int worldId)
        {
            WorldId = worldId;
        }

        #endregion Protected Constructors

        #region Public Properties

        public int WorldId { get; }

        #endregion Public Properties
    }
}