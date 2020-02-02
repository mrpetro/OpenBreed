using OpenBreed.Core.Modules.Audio.Components;
using OpenBreed.Core.Modules.Audio.Systems;
using OpenBreed.Core.Modules.Animation.Systems;
using System;
using System.Collections.Generic;
using OpenBreed.Core.Common.Systems;
using OpenTK.Audio.OpenAL;
using OpenTK;
using OpenBreed.Core.Modules.Audio.Builders;
using OpenBreed.Core.Systems;

namespace OpenBreed.Core.Modules.Audio.Systems
{
    public class SoundSystem : WorldSystem, IAudioSystem
    {
        #region Private Fields

        private List<IAudioComponent> components;

        #endregion Private Fields

        #region Public Constructors

        internal SoundSystem(SoundSystemBuilder builder) : base(builder.core)
        {
            components = new List<IAudioComponent>();
        }

        #endregion Public Constructors

    }
}