using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Core.Events;

namespace OpenBreed.Wecs.Systems.Core
{
    /// <summary>
    /// Updates entity life time and when it's down to zero, it removes it from the world.
    /// </summary>
    [RequireEntityWith(typeof(LifetimeComponent))]
    public class LifetimeSystem : IMatchingSystem, IUpdatableSystem
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

        #region Public Methods

        public void Update(IUpdateContext context)
        {
            var world = worldMan.GetById(context.WorldId);

            var entities = world.GetMatchingEntities(this);

            foreach (var entity in entities)
            {
                UpdateEntity(entity, context);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void UpdateEntity(IEntity entity, IUpdateContext context)
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

        private void RaiseLifetimeEndEvent(IEntity entity)
        {
            eventsMan.Raise(new LifetimeEndEvent(entity.Id));
        }

        #endregion Private Methods
    }
}