using System;
using System.Collections.Generic;
using OpenBreed.Components.Common;
using OpenBreed.Ecsw.Components;
using OpenBreed.Ecsw.Entities;
using OpenBreed.Ecsw.Systems;
using OpenBreed.Systems.Audio.Builders;

namespace OpenBreed.Systems.Audio
{
    public class SoundSystem : SystemBase
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