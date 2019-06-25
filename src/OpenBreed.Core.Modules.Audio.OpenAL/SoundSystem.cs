using OpenBreed.Core.Modules.Audio.Components;
using OpenBreed.Core.Modules.Audio.Systems;
using OpenBreed.Core.Systems;
using System;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Audio
{
    public class SoundSystem : WorldSystem, IAudioSystem
    {
        #region Private Fields

        private List<IAudioComponent> components;

        #endregion Private Fields

        #region Public Constructors

        public SoundSystem(ICore core) : base(core)
        {
            components = new List<IAudioComponent>();
        }

        #endregion Public Constructors

    }
}