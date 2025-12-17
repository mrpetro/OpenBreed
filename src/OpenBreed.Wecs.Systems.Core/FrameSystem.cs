using Microsoft.Extensions.Logging;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Core.Events;

namespace OpenBreed.Wecs.Systems.Core
{
    [RequireEntityWith(typeof(FrameComponent))]
    public class FrameSystem : IMatchingSystem, IUpdatableSystem
    {
        #region Private Fields

        private readonly IWorldMan worldMan;
        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public FrameSystem(
            IWorldMan worldMan,
            IEntityMan entityMan,
            IEventsMan eventsMan,
            ILogger logger)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
            this.entityMan = entityMan ?? throw new System.ArgumentNullException(nameof(entityMan));
            this.eventsMan = eventsMan ?? throw new System.ArgumentNullException(nameof(eventsMan));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
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
            var tc = entity.Get<FrameComponent>();

            if (tc.Target != -1)
            {
                if (tc.Current >= tc.Target)
                {
                    RaiseUpdateEvent(entity);
                    tc.Current = 0;
                }
                else
                    tc.Current++;
            }
        }

        private void RaiseUpdateEvent(IEntity entity)
        {
            eventsMan.Raise(new EntityFrameEvent(entity.Id));
        }

        #endregion Private Methods
    }
}