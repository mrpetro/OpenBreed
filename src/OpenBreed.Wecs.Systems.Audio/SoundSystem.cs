using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Wecs.Components.Audio;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Audio
{
    public class SoundSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly ISoundMan soundMan;

        #endregion Private Fields

        #region Public Constructors

        public SoundSystem(ISoundMan soundMan)
        {
            this.soundMan = soundMan;

            RequireEntityWith<SoundPlayerComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntity(Entity entity, IWorldContext context)
        {
            var soundPlayerComponent = entity.TryGet<SoundPlayerComponent>();

            if (soundPlayerComponent is null)
                return;

            try
            {
                soundMan.PlaySample(soundPlayerComponent.SampleId);
            }
            finally
            {
                entity.Remove<SoundPlayerComponent>();
            }
        }

        #endregion Protected Methods
    }
}