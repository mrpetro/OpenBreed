using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Core
{
    [RequireEntityWith(typeof(LifetimeComponent))]
    public class LifetimeSystem : UpdatableSystemBase<LifetimeSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public LifetimeSystem(
            IWorld world,
            IWorldMan worldMan,
            IEntityMan entityMan,
            IEventsMan eventsMan)
        {
            this.worldMan = worldMan;
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var lc = entity.Get<LifetimeComponent>();

            lc.TimeLeft -= context.Dt;

            if (lc.TimeLeft > 0.0f)
            {
                return;
            }

            lc.TimeLeft = 0.0f;

            RaiseLifetimeEndEvent(entity);

            entity.Expunge();
        }

        #endregion Protected Methods

        #region Private Methods

        private void RaiseLifetimeEndEvent(IEntity entity)
        {
            eventsMan.Raise(entity, new LifetimeEndEvent(entity.Id));
        }

        #endregion Private Methods
    }
}