﻿using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Worlds;

namespace OpenBreed.Wecs.Systems.Core
{
    [RequireEntityWith(typeof(PauserComponent))]
    public class PausingSystem : UpdatableMatchingSystemBase<PausingSystem>
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
            this.worldMan = worldMan;
            this.eventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
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

        internal void OnWorldPaused(IEntity entity, int worldId)
        {
            eventsMan.Raise(entity, new WorldPausedEventArgs(entity.Id, worldId));
        }

        internal void OnWorldUnpaused(IEntity entity, int worldId)
        {
            eventsMan.Raise(entity, new WorldUnpausedEventArgs(entity.Id, worldId));
        }


        #endregion Protected Methods
    }
}