using OpenBreed.Core.Modules.Audio.Systems;
using OpenBreed.Core.Systems;

namespace OpenBreed.Core.Modules.Audio
{
    /// <summary>
    /// Core module interface specialized in sounds and music related work
    /// </summary>
    public interface IAudioModule : ICoreModule
    {
        #region Public Methods

        /// <summary>
        /// Creates sound system and return it
        /// </summary>
        /// <returns>Sound system interface</returns>
        IAudioSystem CreateSoundSystem();

        #endregion Public Methods
    }
}