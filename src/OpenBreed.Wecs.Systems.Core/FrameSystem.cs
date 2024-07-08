using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Core.Events;
using System;

namespace OpenBreed.Wecs.Systems.Core
{
    [RequireEntityWith(typeof(FrameComponent))]
    public class FrameSystem : UpdatableMatchingSystemBase<FrameSystem>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal FrameSystem(
            IEntityMan entityMan,
            IEventsMan eventsMan,
            ILogger logger)
        {
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
            this.logger = logger;

        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var tc = entity.Get<FrameComponent>();

            if(tc.Target != -1)
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

        #endregion Protected Methods

        #region Private Methods

        private void RaiseUpdateEvent(IEntity entity)
        {
            eventsMan.Raise(entity, new EntityFrameEvent(entity.Id));
        }

        #endregion Private Methods
    }
}