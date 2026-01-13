using OpenBreed.Audio.Interface.Managers;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Audio.Components;
using OpenBreed.Wecs.Audio.Systems.Events;
using OpenBreed.Wecs.Core.Systems;
using OpenBreed.Wecs.Core.Systems.Categories;

namespace OpenBreed.Wecs.Audio.Systems
{
    [RequireEntityWith(typeof(SoundPlayerComponent))]
    [SystemCategory(CommonCategories.Audio)]
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