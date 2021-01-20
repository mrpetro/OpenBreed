using System;
using System.Collections.Generic;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Components;
using OpenBreed.Systems.Audio.Builders;
using OpenBreed.Systems.Core;

namespace OpenBreed.Systems.Audio
{
    public class SoundSystem : WorldSystem
    {
        #region Private Fields

        private List<IEntityComponent> components;

        #endregion Private Fields

        #region Public Constructors

        internal SoundSystem(SoundSystemBuilder builder) : base(builder.core)
        {
            components = new List<IEntityComponent>();
        }

        protected override void OnAddEntity(Entity entity)
        {
            throw new NotImplementedException();
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            throw new NotImplementedException();
        }

        #endregion Public Constructors

    }
}