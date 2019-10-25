﻿using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Entities;

namespace OpenBreed.Core.Common.Systems
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
        /// Handle given message
        /// </summary>
        /// <param name="sender">Object is sending the message</param>
        /// <param name="message">message</param>
        /// <returns>True if message was handled, false otherwise</returns>
        bool RecieveMsg(object sender, IMsg message);

        #endregion Public Methods
    }
}