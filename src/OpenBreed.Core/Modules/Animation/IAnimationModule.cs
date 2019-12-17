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
        /// Animations manager
        /// </summary>
        IAnimMan Anims { get; }

        #endregion Public Methods
    }
}