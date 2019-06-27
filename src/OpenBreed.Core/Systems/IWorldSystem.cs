using OpenBreed.Core.Entities;
using OpenBreed.Core.Systems.Common.Components;

namespace OpenBreed.Core.Systems
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

        bool Matches(IEntity entity);

        void AddEntity(IEntity entity);

        void RemoveEntity(IEntity entity);

        /// <summary>
        /// Handle given entity message
        /// </summary>
        /// <param name="sender">entity which is sending the message</param>
        /// <param name="message">message</param>
        /// <returns>True if message was handled, false otherwise</returns>
        bool HandleMsg(IEntity sender, IEntityMsg message);

        #endregion Public Methods
    }
}