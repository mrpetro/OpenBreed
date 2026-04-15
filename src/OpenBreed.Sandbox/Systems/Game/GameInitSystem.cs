using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Scripting.Abstractions;
using OpenBreed.Wecs.Abstractions.Events;
using OpenBreed.Wecs.Abstractions.Primitives;
using OpenBreed.Wecs.Abstractions.Services;
using OpenBreed.Wecs.Abstractions.Systems;
using OpenBreed.Wecs.Control.Systems.Extensions;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBreed.Wecs.Core.Components.Extensions;
using OpenBreed.Common.Game.Wecs.Extensions;

namespace OpenBreed.Sandbox.Systems.Game
{
    internal class GameInitSystem :
        IEventSystem<WorldInitializedEventArgs>
    {
        private readonly IGameServices services;

        public GameInitSystem(IGameServices services)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public void OnEvent(WorldInitializedEventArgs e)
        {
            var world = services.Worlds.GetById(e.WorldId);

            if (world.Name is null || !world.Name.StartsWith(WorldNames.Game))
            {
                return;
            }

            var johnPlayerEntity = services.Entities.GetByTag("John").FirstOrDefault();

            services.ExecuteHeroEnter(johnPlayerEntity, world.Name, 0);
        }
    }
}
