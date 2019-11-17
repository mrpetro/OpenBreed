using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Animation.Helpers;
using OpenBreed.Core.Modules.Animation.Systems;

namespace OpenBreed.Core.Modules.Animation
{
    /// <summary>
    /// Core module interface specialized in animation related work
    /// </summary>
    public interface IAnimationModule : ICoreModule
    {
        #region Public Methods

        /// <summary>
        /// Creates animation system and return it
        /// </summary>
        /// <returns>Animation system interface</returns>
        IAnimationSystem CreateAnimationSystem<T>();

        /// <summary>
        /// Animations manager
        /// </summary>
        IAnimMan Anims { get; }

        #endregion Public Methods
    }
}