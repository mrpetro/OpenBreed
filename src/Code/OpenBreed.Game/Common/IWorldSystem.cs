using OpenBreed.Game.Common.Components;

namespace OpenBreed.Game.Common
{
    /// <summary>
    /// Interface to system which is part of some world
    /// </summary>
    public interface IWorldSystem
    {
        #region Public Methods

        /// <summary>
        /// Initialize the system when world is created
        /// </summary>
        /// <param name="world">World that this system is initialized on</param>
        void Initialize(World world);

        /// <summary>
        /// Deinitialize the system when world is destroyed
        /// </summary>
        /// <param name="world">World that this system is part of</param>
        void Deinitialize(World world);

        /// <summary>
        /// Add the component to this system when entity is added to it's world
        /// </summary>
        /// <param name="component">Component to add</param>
        void AddComponent(IEntityComponent component);

        /// <summary>
        /// Remove the component from this system when entity is being removed from it's world
        /// </summary>
        /// <param name="component">Component to remove</param>
        void RemoveComponent(IEntityComponent component);

        /// <summary>
        /// Update this system with given time step
        /// </summary>
        /// <param name="dt">Time step</param>
        void Update(float dt);

        #endregion Public Methods
    }
}