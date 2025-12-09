using OpenBreed.Common.Game.Services;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenBreed.Wecs.Worlds;
using OpenTK.Compute.OpenCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Components.Common.Extensions;
using OpenBreed.Wecs.Systems.Rendering.Extensions;
using OpenBreed.Wecs.Systems.Animation.Extensions;
using OpenBreed.Wecs.Systems.Animation.Events;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Extensions;
using OpenBreed.Wecs.Systems.Core.Extensions;

namespace OpenBreed.Sandbox.Systems
{
    public class ActorOnEnterTriggerSystem : IEntityOnTriggerActionSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public ActorOnEnterTriggerSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Properties

        public string TriggerName => "EnterWorld";

        public string ActionName => "ShowMission";

        #endregion Public Properties

        #region Public Methods

        public void OnTrigger(IEntity triggeringEntity, IEntity triggerEntity)
        {
            var gameWorld = services.Worlds.GetWorld(triggeringEntity);
            var missionEntity = services.Entities.GetMission(gameWorld.Id);

            services.EntityTriggers.TryOnTrigger("HeroEnter", triggerEntity, missionEntity);

            //missionEntity.TryInvoke(Scripting, Logging, "OnShow", actorEntity);
        }

        #endregion Public Methods
    }
}