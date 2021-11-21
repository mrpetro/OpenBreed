using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Wecs.Components;
using OpenBreed.Wecs.Components.Audio;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Audio
{
    public class SoundSystem : SystemBase, IUpdatableSystem
    {
        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly ISoundMan soundMan;

        #endregion Private Fields

        #region Public Constructors

        public SoundSystem(ISoundMan soundMan)
        {
            RequireEntityWith<SoundPlayerComponent>();


            this.soundMan = soundMan;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);
        }

        public void Update(float dt)
        {
            foreach (var entity in entities)
            {
                var soundPlayerComponent = entity.Get<SoundPlayerComponent>();

                try
                {
                    soundMan.PlaySample(soundPlayerComponent.SampleId);
                }
                finally
                {
                    entity.Remove<SoundPlayerComponent>();
                }
            }
        }

        public void UpdatePauseImmuneOnly(float dt)
        {
            //foreach (var item in Entities)
            //{
            //}
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity) => entities.Contains(entity);

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods
    }
}