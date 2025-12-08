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

namespace OpenBreed.Sandbox.Systems
{
    public class ExplosionOnEnterWorldSystem : IEntityOnTriggerActionSystem
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public ExplosionOnEnterWorldSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public string TriggerName => "EnterWorld";

        public string ActionName => "Perform";

        #endregion Public Constructors

        #region Public Methods

        public void OnTrigger(IEntity triggeringEntity, IEntity triggerEntity)
        {
            var entityMetadata = triggerEntity.GetMetadata();
            var clipName = $"Vanilla/Common/Explosion/{entityMetadata.Flavor}";
            var clipId = services.Clips.GetId(clipName);

            triggerEntity.SetSpriteOn();

            services.Triggers.OnEntityAnimFinished(
                triggerEntity,
                Erase,
                true);

            triggerEntity.PlayAnimation(0, clipId);
        }

        #endregion Public Methods

        #region Private Methods

        private void Erase(IEntity entity, AnimFinishedEvent e)
        {
            services.Worlds.RequestRemoveEntity(entity);
            services.Entities.RequestErase(entity);
        }

        #endregion Private Methods
    }
}