using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Sandbox.Worlds;
using OpenBreed.Wecs.Abstractions.Attributes;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Events;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Game.Wecs.Systems.Hud
{
    [RequireEntityWithTag("FpsCounter")]
    public class DebugHudSystem : IUpdatableSystem
    {
        private readonly IGameServices services;

        public DebugHudSystem(IGameServices services)
        {
            this.services = services;
        }

        public void UpdateEntity(IEntity entity, IUpdateContext context)
        {
            var fps = services.Renders.Fps;

            var text = $"FPS: {fps:0.00}";

            entity.SetText(0, text);
        }


        public void Update(IEnumerable<IEntity> entities, IUpdateContext context)
        {
            foreach (var entity in entities)
            {
                UpdateEntity(entity , context);

            }
        }
    }
}
