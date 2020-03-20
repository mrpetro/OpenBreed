using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;

using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.States;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenBreed.Core.Entities
{
    /// <summary>
    /// Entity interface
    /// </summary>
    public interface IEntity
    {
        #region Public Properties

        /// <summary>
        /// Read-only list of state machines
        /// </summary>
        ReadOnlyCollection<StateMachine> FsmList { get; }

        /// <summary>
        /// Enumeration of all current state names
        /// </summary>
        IEnumerable<string> CurrentStateNames { get; }

        /// <summary>
        /// Core reference
        /// </summary>
        ICore Core { get; }

        /// <summary>
        /// World that this entity is currently in
        /// </summary>
        World World { get; }

        /// <summary>
        /// Gets component of specific type if it exists
        /// </summary>
        /// <typeparam name="T">Type of component to get</typeparam>
        /// <returns>Entity component if exists, null if not</returns>
        T TryGetComponent<T>();

        /// <summary>
        /// Gets component of specific type
        /// </summary>
        /// <typeparam name="T">Type of component to get </typeparam>
        /// <returns>Entity component if exists, throws exception if not</returns>
        T GetComponent<T>();

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

        /// <summary>
        /// Property for user purpose data
        /// </summary>
        object Tag { get; set; }

        #endregion Public Properties

        #region Public Methods

        StateMachine AddFSM(string name);

        /// <summary>
        /// Post command of specific type
        /// </summary>
        /// <param name="cmd">Command to post</param>
        void PostCommand(ICommand cmd);

        /// <summary>
        /// Enqueue an event of specific type and arguments
        /// </summary>
        /// <param name="eventType">Type of event to enqueue</param>
        /// <param name="eventArgs">Arguments of event</param>
        void RaiseEvent<T>(T eventArgs) where T : EventArgs;

        /// <summary>
        /// Subscribe to particular event
        /// </summary>
        /// <param name="eventType">event type to subscribe to</param>
        /// <param name="callback">event callback</param>
        void Subscribe<T>(Action<object, T> callback) where T : EventArgs;

        /// <summary>
        /// Unsubscribe from particular event
        /// </summary>
        /// <param name="eventType">event type to unsubscribe from</param>
        /// <param name="callback">event callback to unsubscribe</param>
        void Unsubscribe<T>(Action<object, T> callback) where T : EventArgs;

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