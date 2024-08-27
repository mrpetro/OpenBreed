using OpenBreed.Core.Interface.Managers;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Core
{
    /// <summary>
    /// Updates entity life time and when it's down to zero, it removes it from the world.
    /// </summary>
    [RequireEntityWith(typeof(LifetimeComponent))]
    public class LifetimeSystem : UpdatableMatchingSystemBase<LifetimeSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly IWorldMan worldMan;

        #endregion Private Fields

        #region Public Constructors

        public LifetimeSystem(
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

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var lc = entity.Get<LifetimeComponent>();

            lc.TimeLeft -= context.Dt;

            if (lc.TimeLeft > 0.0f)
            {
                return;
            }

            lc.TimeLeft = 0.0f;

            worldMan.RequestRemoveEntity(entity);
            RaiseLifetimeEndEvent(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private void RaiseLifetimeEndEvent(IEntity entity)
        {
            eventsMan.Raise(new LifetimeEndEvent(entity.Id));
        }

        #endregion Private Methods
    }
}