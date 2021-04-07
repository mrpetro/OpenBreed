using OpenBreed.Core.Commands;
using OpenBreed.Core;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Commands;

namespace OpenBreed.Wecs.Systems
{
    /// <summary>
    /// Interface to system which is part of some world
    /// </summary>
    public interface ISystem
    {
        #region Public Methods

        /// <summary>
        /// Id of the phase in which system will be updated
        /// </summary>
        int PhaseId { get; }

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

        bool Matches(Entity entity);

        void AddEntity(Entity entity);

        void RemoveEntity(Entity entity);

        bool HandleCommand(ICommand cmd);

        /// <summary>
        /// Handle given command
        /// </summary>
        /// <param name="sender">Object is sending the command</param>
        /// <param name="cmd">Command to recieve</param>
        /// <returns>True if command was handled, false otherwise</returns>
        bool EnqueueCommand(IEntityCommand command);

        #endregion Public Methods
    }
}