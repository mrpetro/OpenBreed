using OpenBreed.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Physics.Events
{
    /// <summary>
    /// Event args for event that occurs when entity body collision enables
    /// </summary>
    public class BodyOnEventArgs : EventArgs
    {
        #region Public Fields

        #endregion Public Fields

        #region Public Constructors

        public BodyOnEventArgs(IEntity entity)
        {
            Entity = entity;
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntity Entity { get; }

        #endregion Public Properties
    }
}
