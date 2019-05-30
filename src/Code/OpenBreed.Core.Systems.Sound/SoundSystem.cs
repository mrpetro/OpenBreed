using OpenBreed.Core.Systems.Sound.Components;
using System;

namespace OpenBreed.Core.Systems.Sound
{
    public class SoundSystem : WorldSystem<ISoundComponent>, ISoundSystem
    {
        #region Public Constructors

        public SoundSystem(ICore core) : base(core)
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void AddComponent(ISoundComponent component)
        {
            throw new NotImplementedException();
        }

        protected override void RemoveComponent(ISoundComponent component)
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods
    }
}