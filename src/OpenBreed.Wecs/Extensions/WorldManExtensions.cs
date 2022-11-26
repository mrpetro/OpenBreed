using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Extensions
{
    /// <summary>
    /// Extension methods for IWorldMan interface
    /// </summary>
    public static class WorldManExtensions
    {
        #region Public Methods

        /// <summary>
        /// Gets World object for specified entity
        /// </summary>
        /// <param name="worldMan">This world manager</param>
        /// <param name="entity">Entity to get world from</param>
        /// <returns>World object</returns>
        public static IWorld GetWorld(this IWorldMan worldMan, IEntity entity)
        {
            return worldMan.GetById(entity.WorldId);
        }

        #endregion Public Methods
    }
}