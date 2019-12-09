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

        /// <summary>
        /// Property for user purpose data
        /// </summary>
        object Tag { get; set; }

        #endregion Public Properties

        #region Public Methods

        StateMachine AddFSM(string name);

        /// <summary>
        /// This will run script with specifc name,
        /// </summary>
        /// <param name="name">Name of the script</param>
        /// <param name="args">script arguments</param>
        /// <returns></returns>
        bool RunScript(string name, params object[] args);

        /// <summary>
        /// Post message of specific type
        /// </summary>
        /// <param name="message"></param>
        void PostMsg(IMsg message);

        /// <summary>
        /// Enqueue an event of specific type and arguments
        /// </summary>
        /// <param name="eventType">Type of event to enqueue</param>
        /// <param name="eventArgs">Arguments of event</param>
        void EnqueueEvent(string eventType, EventArgs eventArgs);

        /// <summary>
        /// Subscribe to particular event
        /// </summary>
        /// <param name="eventType">event type to subscribe to</param>
        /// <param name="callback">event callback</param>
        void Subscribe(string eventType, Action<object, EventArgs> callback);

        /// <summary>
        /// Unsubscribe from particular event
        /// </summary>
        /// <param name="eventType">event type to unsubscribe from</param>
        /// <param name="callback">event callback to unsubscribe</param>
        void Unsubscribe(string eventType, Action<object, EventArgs> callback);

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