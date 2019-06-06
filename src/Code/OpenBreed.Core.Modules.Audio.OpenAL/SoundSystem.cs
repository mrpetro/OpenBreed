using OpenBreed.Core.Modules.Audio.Components;
using OpenBreed.Core.Modules.Audio.Systems;
using OpenBreed.Core.Systems;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Audio
{
    public class SoundSystem : WorldSystem<IAudioComponent>, IAudioSystem
    {
        private List<IAudioComponent> components;

        #region Public Constructors

        public SoundSystem(ICore core) : base(core)
        {
            components = new List<IAudioComponent>();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void AddComponent(IAudioComponent component)
        {
            components.Add(component);
        }

        protected override void RemoveComponent(IAudioComponent component)
        {
            components.Remove(component);
        }

        #endregion Protected Methods
    }
}