using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Core.Systems.Categories;
using OpenBreed.Wecs.Core.Systems.Events;
using System.Collections.Generic;

namespace OpenBreed.Wecs.Core.Systems
{
    [RequireEntityWith(typeof(PauserComponent))]
    [SystemCategory(CommonCategories.General)]
    public class PausingSystem : IUpdatableSystem
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

        public void Update(IEnumerable<IEntity> entities, IUpdateContext context)
        {
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