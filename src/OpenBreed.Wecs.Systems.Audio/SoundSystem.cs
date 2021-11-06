using System;
using System.Collections.Generic;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;

namespace OpenBreed.Wecs.Systems.Audio
{
    public class SoundSystem : SystemBase
    {
        #region Private Fields

        private List<IEntityComponent> components;

        #endregion Private Fields

        #region Public Constructors

        internal SoundSystem()
        {
            components = new List<IEntityComponent>();
        }

        public override bool ContainsEntity(Entity entity)
        {
            throw new NotImplementedException();
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