using OpenBreed.Core.Systems;

namespace OpenBreed.Core.Modules
{
    /// <summary>
    /// Core module interface specialized in sounds and music related work
    /// </summary>
    public interface ISoundModule : ICoreModule
    {
        #region Public Methods

        /// <summary>
        /// Creates sound system and return it
        /// </summary>
        /// <returns>Sound system interface</returns>
        ISoundSystem CreateSoundSystem();

        #endregion Public Methods
    }
}