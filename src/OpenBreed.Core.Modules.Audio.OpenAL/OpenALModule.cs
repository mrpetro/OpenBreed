using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Audio.Components;
using OpenBreed.Core.Modules.Audio.Helpers;
using OpenBreed.Core.Modules.Audio.Systems;
using System;

namespace OpenBreed.Core.Modules.Audio
{
    public class OpenALModule : BaseCoreModule, IAudioModule
    {
        #region Private Fields

        private SoundMan soundMan = new SoundMan();

        #endregion Private Fields

        #region Public Constructors

        public OpenALModule(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Public Properties


        public ISoundMan Sounds { get { return soundMan; } }

        #endregion Public Properties

        #region Public Methods

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