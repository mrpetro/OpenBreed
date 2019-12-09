using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;
using System;

namespace OpenBreed.Core.Modules.Physics.Events
{
    /// <summary>
    /// Event arguments that are passed with BODY_OFF event
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