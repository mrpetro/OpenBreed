using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Physics.Events
{
    /// <summary>
    /// Event args for event that occurs when entity body collision enables
    /// </summary>
    public class BodyOnEventArgs : EventArgs
    {
        #region Public Fields

        #endregion Public Fields

        #region Public Constructors

        public BodyOnEventArgs(Entity entity)
        {
            Entity = entity;
        }

        #endregion Public Constructors

        #region Public Properties

        public Entity Entity { get; }

        #endregion Public Properties
    }
}
