using OpenBreed.Core.Modules;
using OpenBreed.Core.Modules.Audio.Systems;
using OpenBreed.Core.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core.Modules.Audio
{
    public class OpenALModule : IAudioModule
    {
        #region Public Constructors

        public OpenALModule(ICore core)
        {
            Core = core ?? throw new ArgumentNullException(nameof(core));
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        /// <summary>
        /// Creates sound system and return it
        /// </summary>
        /// <returns>Sound system interface</returns>
        public IAudioSystem CreateSoundSystem()
        {
            return new SoundSystem(Core);
        }
    }
}
