using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Modules.Audio.Systems;
using OpenBreed.Core.Modules.Rendering.Systems;

namespace OpenBreed.Core.Modules.Audio.Builders
{
    public class SoundSystemBuilder : IWorldSystemBuilder<SoundSystem>
    {
        #region Internal Fields

        internal ICore core;

        #endregion Internal Fields

        #region Public Constructors

        public SoundSystemBuilder(ICore core)
        {
            this.core = core;
        }

        #endregion Public Constructors

        #region Public Methods

        public SoundSystem Build()
        {
            return new SoundSystem(this);
        }

        #endregion Public Methods
    }
}