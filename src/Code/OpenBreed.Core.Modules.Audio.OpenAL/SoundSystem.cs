using OpenBreed.Core.Modules.Audio.Components;
using OpenBreed.Core.Modules.Audio.Systems;
using OpenBreed.Core.Systems;
using System;

namespace OpenBreed.Core.Modules.Audio
{
    public class SoundSystem : WorldSystem<IAudioComponent>, IAudioSystem
    {
        #region Public Constructors

        public SoundSystem(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void AddComponent(IAudioComponent component)
        {
            throw new NotImplementedException();
        }

        protected override void RemoveComponent(IAudioComponent component)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods
    }
}