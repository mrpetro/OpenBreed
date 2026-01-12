using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Systems.Core.Categories;
using OpenBreed.Wecs.Systems.Core.Events;

namespace OpenBreed.Wecs.Systems.Core
{
    [RequireEntityWith(typeof(PauserComponent))]
    [SystemCategory(CommonCategories.General)]
    public class PausingSystem : IMatchingSystem, IUpdatableSystem
    {
        #region Private Fields

        private readonly IWorldMan worldMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public PausingSystem(
            IWorldMan worldMan,
            IEventsMan eventsMan)
        {
            this.worldMan = worldMan ?? throw new System.ArgumentNullException(nameof(worldMan));
            this.eventsMan = eventsMan ?? throw new System.ArgumentNullException(nameof(eventsMan));
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

        #region Internal Methods

        internal void OnWorldPaused(IEntity entity, int worldId)
        {
            eventsMan.Raise(new WorldPausedEventArgs(entity.Id, worldId));
        }

        internal void OnWorldUnpaused(IEntity entity, int worldId)
        {
            eventsMan.Raise(new WorldUnpausedEventArgs(entity.Id, worldId));
        }

        #endregion Internal Methods

        #region Private Methods

        private void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var pauserComponent = entity.TryGet<PauserComponent>();

            if (pauserComponent is null)
                return;

            try
            {
                if (pauserComponent.Pause == context.Paused)
                    return;

                context.Paused = pauserComponent.Pause;

                if (pauserComponent.Pause)
                    OnWorldPaused(entity, context.WorldId);
                else
                    OnWorldUnpaused(entity, context.WorldId);
            }
            finally
            {
                entity.Remove<PauserComponent>();
            }
        }

        #endregion Private Methods
    }
}