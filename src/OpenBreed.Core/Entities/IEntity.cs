using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Modules.Animation.Systems;
using System;
using System.Collections.ObjectModel;

namespace OpenBreed.Core.Entities
{
    public delegate void EntityPerform(string actionName, params object[] arguments);

    public delegate void SystemEventDelegate(IWorldSystem system, ISystemEvent systemEvent);

    /// <summary>
    /// Entity interface
    /// </summary>
    public interface IEntity
    {
        #region Public Properties

        EntityPerform PerformDelegate { get; set; }

        /// <summary>
        /// System event handle delegate
        /// </summary>
        SystemEventDelegate HandleSystemEvent { get; set; }

        /// <summary>
        /// Core reference
        /// </summary>
        ICore Core { get; }

        /// <summary>
        /// World that this entity is currently in
        /// </summary>
        World World { get; }

        /// <summary>
        /// Read-olny list of components for this entity
        /// </summary>
        ReadOnlyCollection<IEntityComponent> Components { get; }

        /// <summary>
        /// Identification number of this entity
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Property for various debug data
        /// </summary>
        object DebugData { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Post message of specific type
        /// </summary>
        /// <param name="message"></param>
        void PostMessage(IEntityMsg message);

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