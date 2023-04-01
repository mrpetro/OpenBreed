using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Events;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Systems.Core
{
    [RequireEntityWith(typeof(ExpungeComponent))]
    public class ExpungeSystem : UpdatableSystemBase<ExpungeSystem>
    {
        #region Private Fields

        private readonly IWorldMan worldMan;
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public ExpungeSystem(
            IWorld world,
            IWorldMan worldMan,
            IEventsMan eventsMan)
        {
            this.worldMan = worldMan;
            this.eventsMan = eventsMan;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IWorldContext context)
        {
            var expungeComponent = entity.TryGet<ExpungeComponent>();

            if (expungeComponent is null)
                return;

            try
            {
                eventsMan.Raise(null, new ExpungeEvent(entity.Id));

                worldMan.RequestRemoveEntity(entity);
            }
            finally
            {
                entity.Remove<ExpungeComponent>();
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
