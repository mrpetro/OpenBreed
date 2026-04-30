using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Control.Systems.Events;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenBreed.Wecs.Worlds;
using OpenTK.Compute.OpenCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Systems
{
    public class ExplosionOnEnterWorldSystem : IEventSystem<EntityEnteredEvent>
    {
        #region Private Fields

        private readonly IGameServices services;

        #endregion Private Fields

        #region Public Constructors

        public ExplosionOnEnterWorldSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(
            [TargetWorldAsSourceFilter]
            [SourceWorldWithNameFilter(WorldNames.Game)]
            [EntityTriggerActionFilter("EnterWorld", "Perform")]
            EntityEnteredEvent e, IWorld world)
        {
            var entity = services.Entities.GetById(e.EntityId);

            var entityMetadata = entity.GetMetadata();
            var clipName = $"Vanilla/Common/Explosion/{entityMetadata.Flavor}";
            var clipId = services.Clips.GetId(clipName);

            entity.SetSpriteOn();

            services.Triggers.OnEntityAnimFinished(
                entity,
                Erase,
                true);

            entity.PlayAnimation(0, clipId);
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