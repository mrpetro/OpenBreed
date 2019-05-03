using OpenBreed.Game.Common.Components;
using System;
using System.Collections.Generic;

namespace OpenBreed.Game.Entities
{
    /// <summary>
    /// Generic entity interface
    /// </summary>
    public interface IEntity
    {
        #region Public Properties

        /// <summary>
        /// List of components for this entity
        /// </summary>
        List<IEntityComponent> Components { get; }

        /// <summary>
        /// Unique identification number of this entity
        /// </summary>
        Guid Guid { get; }

        #endregion Public Properties
    }
}