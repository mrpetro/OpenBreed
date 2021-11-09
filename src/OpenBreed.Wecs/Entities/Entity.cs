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
        #region Private Fields

        private readonly EntityMan entityMan;
        private readonly Dictionary<Type, IEntityComponent> components = new Dictionary<Type, IEntityComponent>();

        #endregion Private Fields

        #region Internal Constructors

        internal Entity(EntityMan entityMan, List<IEntityComponent> initialComponents)
        {
            this.entityMan = entityMan;

            if (initialComponents is null)
                components = new Dictionary<Type, IEntityComponent>();
            else
                components = initialComponents.ToDictionary(item => item.GetType());

            ComponentValues = components.Values;
            ComponentTypes = components.Keys;
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Read-olny list of components for this entity
        /// </summary>
        public ICollection<IEntityComponent> ComponentValues { get; }

        /// <summary>
        /// Gets the collection of component types
        /// </summary>
        public ICollection<Type> ComponentTypes { get; }

        /// <summary>
        /// Property for user purpose data
        /// </summary>
        public object Tag { get; set; }


        public object State { get; set; }

        /// <summary>
        /// Id of world which this entity is part of 
        /// </summary>
        public int WorldId { get; internal set; } = -1;

        /// <summary>
        /// Identification number of this entity
        /// </summary>
        public int Id { get; internal set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets component of specific type if it exists
        /// </summary>
        /// <typeparam name="TComponent">Type of component to get</typeparam>
        /// <returns>Entity component if exists, null if not</returns>
        public TComponent TryGet<TComponent>()
        {
            if (components.TryGetValue(typeof(TComponent), out IEntityComponent component))
                return (TComponent)component;
            else
                return default(TComponent);
        }

        /// <summary>
        /// Checks if entity contains component of specific type
        /// </summary>
        /// <typeparam name="TComponent">Type of component to check</typeparam>
        /// <returns>true if entity contains the component, false if not</returns>
        public bool Contains<TComponent>()
        {
            return components.ContainsKey(typeof(TComponent));
        }

        /// <summary>
        /// Gets component of specific type
        /// </summary>
        /// <typeparam name="TComponent">Type of component to get </typeparam>
        /// <returns>Entity component if exists, throws exception if not</returns>
        public TComponent Get<TComponent>()
        {
            Debug.Assert(components.ContainsKey(typeof(TComponent)), $"Expected to get component '{typeof(TComponent).Name}'.");

            return (TComponent)components[typeof(TComponent)];
        }

        /// <summary>
        /// Enqueue an event of specific type and arguments
        /// </summary>
        /// <param name="eventArgs">Arguments of event</param>
        public void RaiseEvent<T>(T eventArgs) where T : EventArgs
        {
            entityMan.Raise(this, eventArgs);
        }

        /// <summary>
        /// Subscribe to particular event
        /// </summary>
        /// <param name="callback">event callback</param>
        public void Subscribe<T>(Action<object, T> callback) where T : EventArgs
        {
            entityMan.Subscribe(this, callback);
        }

        /// <summary>
        /// Unsubscribe from particular event
        /// </summary>
        /// <param name="callback">event callback to unsubscribe</param>
        public void Unsubscribe<T>(Action<object, T> callback) where T : EventArgs
        {
            entityMan.Unsubscribe(this, callback);
        }

        public void Set<TComponent>(TComponent component) where TComponent : class, IEntityComponent
        {
            Debug.Assert(component != null, "Adding null component to entity is forbidden.");

            var count = components.Count;
            components[component.GetType()] = component;

            if(components.Count != count)
                entityMan.OnComponentAdded(this, typeof(TComponent));
        }

        /// <summary>
        /// Add component to entity
        /// </summary>
        /// <param name="component">Component to add</param>
        public void Add<TComponent>(TComponent component) where TComponent : class, IEntityComponent
        {
            Debug.Assert(component != null, "Adding null component to entity is forbidden.");
            Debug.Assert(!components.ContainsKey(typeof(TComponent)), "Adding two components of same type to one entity is forbidden.");
            components.Add(typeof(TComponent), component);
            entityMan.OnComponentAdded(this, typeof(TComponent));
        }

        /// <summary>
        /// Remove component of specific type from entity
        /// </summary>
        /// <typeparam name="TComponent">Type of component to remove</typeparam>
        /// <returns>True if component remove successfuly, false otherwise</returns>
        public bool Remove<TComponent>()
        {
            var removed = components.Remove(typeof(TComponent));
            entityMan.OnComponentRemoved(this, typeof(TComponent));
            return removed;
        }

        /// <summary>
        /// Remove component from entity
        /// </summary>
        /// <param name="component"></param>
        /// <returns>True if component remove successfuly, false otherwise</returns>
        public bool Remove(IEntityComponent component)
        {
            return components.Remove(component.GetType());
        }

        public override string ToString()
        {
            return $"Entity({Id})";
        }

        public void LeaveWorld()
        {
            //If entity is already in limbo then do nothing
            if (WorldId == World.NO_WORLD)
                return;

            entityMan.RequestLeaveWorld(this);
        }

        public void EnterWorld(int worldId)
        {
            //If entity is already is same world then do nothing
            if (WorldId == worldId)
                return;

            //If entity is not in limbo then it's in different world.
            if (WorldId != World.NO_WORLD)
                throw new InvalidOperationException("Entity already in different world.");

            entityMan.RequestEnterWorld(this, worldId);
        }

        #endregion Public Methods
    }
}