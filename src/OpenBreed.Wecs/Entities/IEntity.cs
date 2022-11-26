using OpenBreed.Wecs.Components;
using System.Collections.Generic;
using System;

namespace OpenBreed.Wecs.Entities
{
    /// <summary>
    /// Entity interface
    /// </summary>
    public interface IEntity
    {
        #region Public Properties

        /// <summary>
        /// Identification number of this entity
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Id of world which this entity is part of
        /// </summary>
        int WorldId { get; }

        /// <summary>
        /// Read-olny list of components for this entity
        /// </summary>
        ICollection<IEntityComponent> ComponentValues { get; }

        /// <summary>
        /// Gets the collection of component types
        /// </summary>
        ICollection<Type> ComponentTypes { get; }

        /// <summary>
        /// Entity tag information, useful for finding
        /// </summary>
        public string Tag { get; }

        /// <summary>
        /// State of entity (candidate to be remove from this interface)
        /// </summary>
        object State { get; set; }

        void RaiseEvent<T>(T eventArgs) where T : EventArgs;

        /// <summary>
        /// Gets component of specific type if it exists
        /// </summary>
        /// <typeparam name="TComponent">Type of component to get</typeparam>
        /// <returns>Entity component if exists, null if not</returns>
        TComponent TryGet<TComponent>();

        /// <summary>
        /// Checks if entity contains component of specific type
        /// </summary>
        /// <typeparam name="TComponent">Type of component to check</typeparam>
        /// <returns>true if entity contains the component, false if not</returns>
        bool Contains<TComponent>();

        /// <summary>
        /// Set component of specific type
        /// </summary>
        /// <typeparam name="TComponent">Type of component to set</typeparam>
        /// <param name="component">Component instance</param>
        void Set<TComponent>(TComponent component) where TComponent : class, IEntityComponent;

        /// <summary>
        /// Gets component of specific type
        /// </summary>
        /// <typeparam name="TComponent">Type of component to get</typeparam>
        /// <returns>Entity component if exists, throws exception if not</returns>
        public TComponent Get<TComponent>();

        /// <summary>
        /// Add component of given type
        /// </summary>
        /// <typeparam name="TComponent">Type of component to add</typeparam>
        /// <param name="component">Component instance</param>
        void Add<TComponent>(TComponent component) where TComponent : class, IEntityComponent;

        /// <summary>
        /// Remove component of specific type
        /// </summary>
        /// <typeparam name="TComponent">Type of component to remove</typeparam>
        /// <returns></returns>
        bool Remove<TComponent>();

        /// <summary>
        /// Destroy entity - Leave world if at any, then remove from entities list
        /// </summary>
        void Destroy();

        /// <summary>
        /// Enter world given in argument
        /// </summary>
        /// <param name="worldId">World Id to which entity will enter</param>
        void EnterWorld(int worldId);

        /// <summary>
        /// Leave current world
        /// </summary>
        void LeaveWorld();

        #endregion Public Properties
    }
}