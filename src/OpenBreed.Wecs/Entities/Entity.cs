using OpenBreed.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Components;
using OpenBreed.Core.Managers;

namespace OpenBreed.Wecs.Entities
{
    /// <summary>
    /// Entity interface implementation
    /// </summary>
    public class Entity
    {
        private readonly IEventsMan eventsMan;
        #region Private Fields

        private readonly List<IEntityComponent> components = new List<IEntityComponent>();

        #endregion Private Fields

        #region Internal Constructors

        internal Entity(IEventsMan eventsMan, List<IEntityComponent> initialComponents)
        {
            this.eventsMan = eventsMan;

            components = initialComponents ?? new List<IEntityComponent>();
            Components = new ReadOnlyCollection<IEntityComponent>(components);
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Read-olny list of components for this entity
        /// </summary>
        public ReadOnlyCollection<IEntityComponent> Components { get; }

        /// <summary>
        /// Property for user purpose data
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Id of world which this entity is part of 
        /// </summary>
        public int WorldId { get; internal set; } = -1;

        /// <summary>
        /// Identification number of this entity
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// Property for various debug data
        /// </summary>
        public object DebugData { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets component of specific type if it exists
        /// </summary>
        /// <typeparam name="T">Type of component to get</typeparam>
        /// <returns>Entity component if exists, null if not</returns>
        public T TryGet<T>()
        {
            return components.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Checks if entity contains component of specific type
        /// </summary>
        /// <typeparam name="T">Type of component to check</typeparam>
        /// <returns>true if entity contains the component, false if not</returns>
        public bool Contains<T>()
        {
            return components.OfType<T>().Any();
        }

        /// <summary>
        /// Gets component of specific type
        /// </summary>
        /// <typeparam name="T">Type of component to get </typeparam>
        /// <returns>Entity component if exists, throws exception if not</returns>
        public T Get<T>()
        {
            Debug.Assert(components.OfType<T>().Any(), $"Expected to get component '{typeof(T).Name}'.");

            return components.OfType<T>().First();
        }

        /// <summary>
        /// Enqueue an event of specific type and arguments
        /// </summary>
        /// <param name="eventArgs">Arguments of event</param>
        public void RaiseEvent<T>(T eventArgs) where T : EventArgs
        {
            eventsMan.Raise(this, eventArgs);
        }

        /// <summary>
        /// Subscribe to particular event
        /// </summary>
        /// <param name="callback">event callback</param>
        public void Subscribe<T>(Action<object, T> callback) where T : EventArgs
        {
            eventsMan.Subscribe(this, callback);
        }

        /// <summary>
        /// Unsubscribe from particular event
        /// </summary>
        /// <param name="callback">event callback to unsubscribe</param>
        public void Unsubscribe<T>(Action<object, T> callback) where T : EventArgs
        {
            eventsMan.Unsubscribe(this, callback);
        }

        /// <summary>
        /// Add component to entity
        /// </summary>
        /// <param name="component">Component to add</param>
        public void Add(IEntityComponent component)
        {
            Debug.Assert(component != null, "Adding null component to entity is forbidden.");
            Debug.Assert(!components.Any(item => item.GetType() == component.GetType()), "Adding two components of same type to one entity is forbidden.");
            components.Add(component);
        }

        /// <summary>
        /// Remove component from entity
        /// </summary>
        /// <param name="component"></param>
        /// <returns>True if component remove successfuly, false otherwise</returns>
        public bool Remove(IEntityComponent component)
        {
            return components.Remove(component);
        }

        public override string ToString()
        {
            return $"Entity({Id})";
        }

        #endregion Public Methods
    }
}