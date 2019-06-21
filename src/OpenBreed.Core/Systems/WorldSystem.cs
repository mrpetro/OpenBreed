using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Common.Components;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Systems
{
    public abstract class WorldSystem<T> : IWorldSystem where T : IEntityComponent
    {
        #region Private Fields

        private readonly List<Type> requiredComponents = new List<Type>();

        #endregion Private Fields

        #region Protected Constructors

        protected WorldSystem(ICore core)
        {
            Core = core;
        }

        #endregion Protected Constructors

        #region Public Properties

        /// <summary>
        /// Reference to Core
        /// </summary>
        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initialize the system when world is created
        /// </summary>
        /// <param name="world">World that this system is initialized on</param>
        public virtual void Initialize(World world)
        {
        }

        /// <summary>
        /// Deinitialize the system when world is destroyed
        /// </summary>
        /// <param name="world">World that this system is part of</param>
        public virtual void Deinitialize(World world)
        {
        }

        /// <summary>
        /// Add the component to this system when entity is added to it's world
        /// </summary>
        /// <param name="component">Component to add</param>
        public void AddComponent(IEntityComponent component)
        {
            AddComponent((T)component);
        }

        /// <summary>
        /// Remove the component from this system when entity is being removed from it's world
        /// </summary>
        /// <param name="component">Component to remove</param>
        public void RemoveComponent(IEntityComponent component)
        {
            RemoveComponent((T)component);
        }

        /// <summary>
        /// Update this system with given time step
        /// </summary>
        /// <param name="dt">Time step</param>
        public virtual void Update(float dt)
        {
        }

        public bool Matches(Entity entity)
        {
            return false;
        }

        public void AddEntity(Entity entity)
        {
        }

        #endregion Public Methods

        #region Protected Methods

        protected void Require<C>()
        {
            var type = typeof(C);
            if (!requiredComponents.Contains(type))
                requiredComponents.Add(type);
        }

        protected abstract void AddComponent(T component);

        protected abstract void RemoveComponent(T component);

        public bool Matches(IEntity entity)
        {
            return false;
        }

        public void AddEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveEntity(IEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods
    }
}