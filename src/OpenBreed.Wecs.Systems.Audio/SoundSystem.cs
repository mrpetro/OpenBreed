using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Components.Audio;
using OpenBreed.Wecs.Systems.Audio.Events;
using OpenBreed.Wecs.Systems.Core;

namespace OpenBreed.Wecs.Systems.Audio
{
    [RequireEntityWith(typeof(SoundPlayerComponent))]
    public class SoundSystem : UpdatableMatchingSystemBase
    {
        #region Private Fields

        private readonly ISoundMan soundMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public SoundSystem(
            IWorldMan worldMan,
            ISoundMan soundMan,
            IEventsMan eventsMan) : base(worldMan)
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