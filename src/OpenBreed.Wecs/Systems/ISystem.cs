using OpenBreed.Core;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems
{
    /// <summary>
    /// Interface to subsystem
    /// </summary>
    public interface ISubsystem
    {

    }

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

        bool ContainsEntity(IEntity entity);

        void AddEntity(IEntity entity);

        void RemoveEntity(IEntity entity);


        #endregion Public Methods
    }
}