using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Sandbox.Extensions;
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
    public class FpsCounterSystem : IEventSystem<ViewportResizedEvent>, IUpdatableSystem
    {
        private readonly IGameServices services;

        public FpsCounterSystem(IGameServices services)
        {
            this.services = services;
        }

        public void Update(ViewportResizedEvent e)
        {
            var entity = services.Entities.GetById(e.EntityId);
            var hudViewport = services.Entities.GetHudViewport();

            if (entity != hudViewport)
            {
                return;
            }

            var fpsCounter = services.Entities.GetFpsCounter();
            fpsCounter.SetPosition(-e.Width / 2.0f, -e.Height / 2.0f);
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
