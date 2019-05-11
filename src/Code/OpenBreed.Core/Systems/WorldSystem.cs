using OpenBreed.Core.Systems.Common.Components;

namespace OpenBreed.Core.Systems
{
    public abstract class WorldSystem<T> : IWorldSystem where T : IEntityComponent
    {
        #region Protected Constructors

        protected WorldSystem()
        {
        }

        #endregion Protected Constructors

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

        #endregion Public Methods

        #region Protected Methods

        protected abstract void AddComponent(T component);

        protected abstract void RemoveComponent(T component);

        #endregion Protected Methods
    }
}