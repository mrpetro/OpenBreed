using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Worlds
{
    /// <summary>
    /// World interface
    ///
    public interface IWorld
    {
        #region Public Properties

        /// <summary>
        /// Time "speed" control value, can't be negative but can be 0 (Basicaly stops time).
        /// </summary>
        float DtMultiplier { get; set; }

        /// <summary>
        /// All world entitites
        /// </summary>
        IEnumerable<IEntity> Entities { get; }

        /// <summary>
        /// Id of this world
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Name of this world
        /// </summary>
        string Name { get; }

        /// <summary>
        /// All world systems
        /// </summary>
        ISystem[] Systems { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Remove given entity from this world
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        void RemoveEntity(IEntity entity);

        /// <summary>
        /// Add given entity to this world
        /// </summary>
        /// <param name="entity">Entity to add</param>
        void AddEntity(IEntity entity);

        /// <summary>
        /// Get specific system from this world
        /// </summary>
        /// <typeparam name="T">Type of system to get</typeparam>
        /// <returns>World system of specific type</returns>
        T GetSystem<T>() where T : IMatchingSystem;

        #endregion Public Methods
    }
}