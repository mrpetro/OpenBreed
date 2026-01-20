using OpenBreed.Common.Game.Services;
using OpenBreed.Wecs.Worlds;
using OpenTK.Compute.OpenCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenBreed.Wecs.Animation.Systems.Extensions;
using OpenBreed.Wecs.Animation.Systems.Events;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Systems;

namespace OpenBreed.Sandbox.Systems
{
    public class ExplosionOnEnterWorldSystem : IOnAddEntityActionSystem
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

        public void OnAddEntity(IWorld world, IEntity entity)
        {
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