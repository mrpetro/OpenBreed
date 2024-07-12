using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;

namespace OpenBreed.Wecs.Worlds
{
    /// <summary>
    /// Interface for matching compatible systems and entities
    /// </summary>
    public interface IEntityToSystemMatcher
    {
        #region Public Methods

        /// <summary>
        /// Checks if given system and entity matches and returns result
        /// </summary>
        /// <param name="system">System that is being matched</param>
        /// <param name="entity">Entity that is being matched</param>
        /// <returns>True if system and entity are matching, false otherwise</returns>
        bool AreMatch(IMatchingSystem system, IEntity entity);

        #endregion Public Methods
    }
}