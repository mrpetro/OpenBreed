using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Systems
{
    /// <summary>
    /// Interface to system which is part of some world
    /// </summary>
    public interface IWorldSystem
    {
        #region Public Methods

        /// <summary>
        /// World which owns this system
        /// </summary>
        World World { get; }

        /// <summary>
        /// Initialize the system when world is created
        /// </summary>
        /// <param name="world">World that this system is initialized on</param>
        void Initialize(World world);

        /// <summary>
        /// Perform cleanup of entites and their components related with this system
        /// </summary>
        void Cleanup();

        /// <summary>
        /// Deinitialize the system when world is destroyed
        /// </summary>
        void Deinitialize();

        bool Matches(IEntity entity);

        void AddEntity(IEntity entity);

        void RemoveEntity(IEntity entity);

        /// <summary>
        /// Handle given command
        /// </summary>
        /// <param name="sender">Object is sending the command</param>
        /// <param name="cmd">Command to recieve</param>
        /// <returns>True if command was handled, false otherwise</returns>
        bool ExecuteCommand(ICommand cmd);

        #endregion Public Methods
    }
}