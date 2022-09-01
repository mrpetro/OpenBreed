using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components.Audio;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Audio.Events;
using OpenBreed.Wecs.Systems.Core;
using OpenBreed.Wecs.Worlds;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Audio
{
    public class SoundSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly ISoundMan soundMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public SoundSystem(ISoundMan soundMan, IEventsMan eventsMan)
        {
            this.soundMan = soundMan;
            this.eventsMan = eventsMan;
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

            var toPlay = soundPlayerComponent.ToPlay;

            //try
            //{
            for (int i = 0; i < toPlay.Count; i++)
            {
                //soundMan.PlaySample(toPlay[i]);
                eventsMan.Raise(null, new SoundPlayEvent(entity.Id, toPlay[i]));
            }

            toPlay.Clear();

            //}
            //finally
            //{
            //    entity.Remove<SoundPlayerComponent>();
            //}
        }

        #endregion Protected Methods
    }
}