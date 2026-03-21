using Microsoft.Extensions.Logging;
using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Common.Game.Wecs.Events;
using OpenBreed.Common.Game.Wecs.Extensions;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Extensions;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Audio.Systems.Extensions;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Wecs.Core.Systems.Events;
using OpenBreed.Wecs.Core.Systems.Extensions;
using System;

namespace OpenBreed.Sandbox.Systems.Actor
{
    public class ActorResurectSystem : IEventSystem<DestroyedEvent>
    {
        private readonly IGameServices services;
        private readonly IEntityClass actorClass;

        public ActorResurectSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));

            actorClass = services.Classes.GetByName("Actor");
        }

        public void OnEvent(DestroyedEvent e)
        {
            var entity = services.Entities.GetById(e.EntityId);

            if (!entity.Is(actorClass))
            {
                return;
            }

            var pos = entity.GetPosition();
            entity.StartEmit("ABTA\\Templates\\Common\\Projectiles\\Explosion")
                .SetOption("flavor", "Small")
                .SetOption("startX", pos.X)
                .SetOption("startY", pos.Y)
                .Finish();

            var soundId = services.Sounds.GetByName("Vanilla/Common/Hero/Dying");
            entity.EmitSound(soundId);

            entity.SetResurrectable(entity.WorldId);

            services.Logger.LogInformation("Player Died!");

            var limboWorld = services.Worlds.GetByName(WorldNames.Limbo);

            entity.State = "Dead";

            var task = Core.Task.Create((t) => services.AddToWorld(t, entity, WorldNames.Limbo));

            task.Then((t) => services.Wait(t, entity, 3000))
                .Then((t) => services.Resurrect(t, entity));

            task.Start();
        }
    }
}