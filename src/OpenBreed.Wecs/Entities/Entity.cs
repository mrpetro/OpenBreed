using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OpenBreed.Wecs.Entities
{
    /// <summary>
    /// Entity interface implementation
    /// </summary>
    internal class Entity : IEntity
    {
        #region Private Fields

        private readonly EntityMan entityMan;
        private readonly Dictionary<Type, IEntityComponent> components = new Dictionary<Type, IEntityComponent>();

        #endregion Private Fields

        #region Internal Constructors

        internal Entity(
            EntityMan entityMan,
            string tag,
            List<IEntityComponent> initialComponents)
        {
            this.entityMan = entityMan;
            Tag = tag;

            if (initialComponents is null)
                components = new Dictionary<Type, IEntityComponent>();
            else
                components = initialComponents.ToDictionary(item => item.GetType());

            ComponentValues = components.Values;
            ComponentTypes = components.Keys;
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
        /// Entity tag information, useful for finding
        /// </summary>
        public string Tag { get; }

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
                return default;
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

        public void Set<TComponent>(TComponent component) where TComponent : class, IEntityComponent
        {
            Debug.Assert(component != null, "Adding null component to entity is forbidden.");

            var count = components.Count;
            components[component.GetType()] = component;

            if (components.Count != count)
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

        public override string ToString()
        {
            if(Tag is null)
                return $"{Id}";
            else
                return $"{Id} ({Tag})";
        }

        #endregion Public Methods
    }
}