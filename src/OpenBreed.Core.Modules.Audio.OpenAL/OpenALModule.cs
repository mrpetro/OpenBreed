using OpenBreed.Audio.Interface;
using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Audio.OpenAL.Managers;
using OpenBreed.Core.Managers;
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


        #endregion Public Methods
    }
}