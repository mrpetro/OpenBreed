using OpenBreed.Audio.Interface;
using OpenBreed.Audio.Interface.Managers;

namespace OpenBreed.Core.Modules.Audio
{
    public class OpenALModule : BaseCoreModule, IAudioModule
    {
        #region Public Constructors

        public OpenALModule(ICore core) : base(core)
        {
            Sounds = core.GetManager<ISoundMan>();
        }

        #endregion Public Constructors

        #region Public Properties

        public ISoundMan Sounds { get; }

        #endregion Public Properties
    }
}