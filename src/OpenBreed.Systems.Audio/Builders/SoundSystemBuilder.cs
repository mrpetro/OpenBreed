using OpenBreed.Core;
using OpenBreed.Core.Systems;

namespace OpenBreed.Systems.Audio.Builders
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