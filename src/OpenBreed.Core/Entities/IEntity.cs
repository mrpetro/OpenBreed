using OpenBreed.Core.Systems.Common.Components;
using System;
using System.Collections.ObjectModel;

namespace OpenBreed.Core.Entities
{
    public delegate void EntityPerform(string actionName, params object[] arguments);

    /// <summary>
    /// Entity interface
    /// </summary>
    public interface IEntity
    {
        #region Public Properties

        EntityPerform PerformDelegate { get; set; }

        /// <summary>
        /// World that this entity is currently in
        /// </summary>
        World CurrentWorld { get; }

        /// <summary>
        /// Read-olny list of components for this entity
        /// </summary>
        ReadOnlyCollection<IEntityComponent> Components { get; }

        /// <summary>
        /// Unique identification number of this entity
        /// </summary>
        Guid Guid { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Add component to entity
        /// </summary>
        /// <param name="component">Component to add</param>
        void Add(IEntityComponent component);

        /// <summary>
        /// Remove component from entity
        /// </summary>
        /// <param name="component"></param>
        /// <returns>True if component remove successfuly, false otherwise</returns>
        bool Remove(IEntityComponent component);

        #endregion Public Methods
    }
}