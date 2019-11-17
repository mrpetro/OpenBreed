using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Audio.Components;
using OpenBreed.Core.Modules.Audio.Helpers;
using OpenBreed.Core.Modules.Audio.Systems;
using System;

namespace OpenBreed.Core.Modules.Audio
{
    public class OpenALModule : IAudioModule
    {
        #region Private Fields

        private SoundMan soundMan = new SoundMan();

        #endregion Private Fields

        #region Public Constructors

        public OpenALModule(ICore core)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        public ISoundMan Sounds { get { return soundMan; } }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates sound system and return it
        /// </summary>
        /// <returns>Sound system interface</returns>
        public IAudioSystem CreateSoundSystem()
        {
            return new SoundSystem(Core);
        }

        /// <summary>
        /// Create local sound emiter that emits sound from specific position in owning world coordinates
        /// </summary>
        /// <returns>Local sound emiter</returns>
        public ISoundEmiter CreateLocalSoundEmiter()
        {
            return new LocalSoundEmiter();
        }

        /// <summary>
        /// Create global sound emiter that emits sound on entire world
        /// </summary>
        /// <returns>Global sound emiter</returns>
        public ISoundEmiter CreateGlobalSoundEmiter()
        {
            return new GlobalSoundEmiter();
        }

        #endregion Public Methods
    }
}