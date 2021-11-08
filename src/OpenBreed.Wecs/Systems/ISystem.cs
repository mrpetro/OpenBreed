using OpenBreed.Core.Commands;
using OpenBreed.Core;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenBreed.Wecs.Commands;
using System;
using System.Collections.Generic;

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
        /// ID of world which owns this system
        /// </summary>
        int WorldId { get; }

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

        bool HasEntity(Entity entity);

        void AddEntity(Entity entity);

        void RemoveEntity(Entity entity);

        /// <summary>
        /// Handle given command
        /// </summary>
        /// <param name="sender">Object is sending the command</param>
        /// <param name="cmd">Command to recieve</param>
        /// <returns>True if command was handled, false otherwise</returns>
        bool EnqueueCommand(IEntityCommand command);

        /// <summary>
        /// Get types of entity components required by this system
        /// </summary>
        IReadOnlyCollection<Type> RequiredComponentTypes { get; }

        #endregion Public Methods
    }
}