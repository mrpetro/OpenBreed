using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Audio;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Audio.Events;
using OpenBreed.Wecs.Systems.Core;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Systems.Audio
{
    [RequireEntityWith(typeof(SoundPlayerComponent))]
    public class SoundSystem : UpdatableMatchingSystemBase<SoundSystem>
    {
        #region Private Fields

        private readonly ISoundMan soundMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public SoundSystem(
            ISoundMan soundMan,
            IEventsMan eventsMan)
        {
            this.soundMan = soundMan;
            this.eventsMan = eventsMan;

        }

        #endregion Public Constructors

        #region Public Methods

        #endregion Public Methods

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var soundPlayerComponent = entity.TryGet<SoundPlayerComponent>();

            if (soundPlayerComponent is null)
                return;

            var toPlay = soundPlayerComponent.ToPlay;

            for (int i = 0; i < toPlay.Count; i++)
            {
                soundMan.PlaySample(toPlay[i]);
                eventsMan.Raise(new SoundPlayEvent(entity.Id, toPlay[i]));
            }

            toPlay.Clear();
        }

        #endregion Protected Methods
    }
}