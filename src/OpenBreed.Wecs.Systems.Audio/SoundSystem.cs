using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Entities;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Audio
{
    public class SoundSystem : SystemBase
    {
        #region Private Fields

        private List<IEntityComponent> components;

        #endregion Private Fields

        #region Internal Constructors

        internal SoundSystem()
        {
            components = new List<IEntityComponent>();
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity)
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

        #endregion Protected Methods
    }
}