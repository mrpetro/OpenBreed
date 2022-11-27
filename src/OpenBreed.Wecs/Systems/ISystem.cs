using OpenBreed.Core;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
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
        /// world which owns this system
        /// </summary>
        IWorld World { get; }

        /// <summary>
        /// Perform cleanup of entites and their components related with this system
        /// </summary>
        void Cleanup();

        bool Matches(IEntity entity);

        bool HasEntity(IEntity entity);

        void RequestAddEntity(IEntity entity);

        void RequestRemoveEntity(IEntity entity);

        /// <summary>
        /// Get types of entity components required by this system
        /// </summary>
        IReadOnlyCollection<Type> RequiredComponentTypes { get; }

        #endregion Public Methods
    }
}