using OpenBreed.Core.Systems.Common.Components;

namespace OpenBreed.Core.Systems
{
    /// <summary>
    /// Interface to system which is part of some world
    /// </summary>
    public interface IWorldSystemEx
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
        /// Update this system with given time step
        /// </summary>
        /// <param name="dt">Time step</param>
        void Update(float dt);

        #endregion Public Methods
    }
}