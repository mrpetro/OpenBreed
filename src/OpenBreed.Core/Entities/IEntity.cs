using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Helpers;
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
        #region Public Events

        /// <summary>
        /// Called when entity is removed from world
        /// </summary>
        event EventHandler<World> RemovedFromWorld;

        #endregion Public Events

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

        StateMachine AddFSM(string name);

        /// <summary>
        /// Post message of specific type
        /// </summary>
        /// <param name="message"></param>
        void PostMsg(IEntityMsg message);

        /// <summary>
        /// Raise event of specific type
        /// </summary>
        /// <param name="ev"></param>
        void RaiseEvent(IEvent ev);

        /// <summary>
        /// Subscribe to particular event
        /// </summary>
        /// <param name="eventType">event type to subscribe to</param>
        /// <param name="callback">event callback</param>
        void Subscribe(string eventType, Action<object, IEvent> callback);

        /// <summary>
        /// Unsubscribe from particular event
        /// </summary>
        /// <param name="eventType">event type to unsubscribe from</param>
        /// <param name="callback">event callback to unsubscribe</param>
        void Unsubscribe(string eventType, Action<object, IEvent> callback);

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