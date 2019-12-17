using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Audio.Components;
using OpenBreed.Core.Modules.Audio.Helpers;
using OpenBreed.Core.Modules.Audio.Systems;

namespace OpenBreed.Core.Modules.Audio
{
    /// <summary>
    /// Core module interface specialized in sounds and music related work
    /// </summary>
    public interface IAudioModule : ICoreModule
    {
        #region Public Properties

        ISoundMan Sounds { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create local sound emiter that emits sound from specific position in owning world coordinates
        /// </summary>
        /// <returns>Local sound emiter</returns>
        ISoundEmiter CreateLocalSoundEmiter();

        /// <summary>
        /// Create global sound emiter that emits sound on entire world
        /// </summary>
        /// <returns>Global sound emiter</returns>
        ISoundEmiter CreateGlobalSoundEmiter();

        #endregion Public Methods
    }
}