using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Core
{
    public class PausingSystem : UpdatableSystemBase
    {
        #region Private Fields

        private readonly IWorldMan worldMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public PausingSystem(IWorldMan worldMan, IEventsMan eventsMan)
        {
            this.worldMan = worldMan;
            this.eventsMan = eventsMan;

            RequireEntityWith<PauserComponent>();
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(Entity entity, IWorldContext context)
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
                    OnWorldPaused(entity, WorldId);
                else
                    OnWorldUnpaused(entity, WorldId);
            }
            finally
            {
                entity.Remove<PauserComponent>();
            }
        }

        internal void OnWorldPaused(Entity entity, int worldId)
        {
            eventsMan.Raise(entity, new WorldPausedEventArgs(entity.Id, worldId));
        }

        internal void OnWorldUnpaused(Entity entity, int worldId)
        {
            eventsMan.Raise(entity, new WorldUnpausedEventArgs(entity.Id, worldId));
        }


        #endregion Protected Methods
    }
}