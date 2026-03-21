using OpenBreed.Common.Game;
using OpenBreed.Common.Game.Services;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Sandbox.Entities.Actor;
using OpenBreed.Sandbox.Extensions;
using OpenBreed.Scripting.Interface;
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
        private readonly ActorHelper actorHelper;

        public GameInitSystem(IGameServices services,
            ActorHelper actorHelper)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.actorHelper = actorHelper ?? throw new ArgumentNullException(nameof(actorHelper));
        }

        public void OnEvent(WorldInitializedEventArgs e)
        {
            var world = services.Worlds.GetById(e.WorldId);

            if (world.Name is null || !world.Name.StartsWith(WorldNames.Game))
            {
                return;
            }

            var playerCamera = services.Factory.CreateCamera("Camera.Player", 0, 0, 320, 240);

            playerCamera.Add(new PauseImmuneComponent());

            var gameViewport = services.Entities.GetByTag(EntityNames.GameViewport).First();
            gameViewport.SetViewportCamera(playerCamera.Id);

            var player1Entity = services.Entities.GetByTag("Players/P1").First();

            var johnPlayerEntity = actorHelper.CreatePlayerActor("John", new Vector2(0, 0));

            player1Entity.SetControlledEntity(johnPlayerEntity.Id);

            johnPlayerEntity.AddFollower(playerCamera);

            ExecuteHeroEnter(johnPlayerEntity, world.Name, 0);
        }

        public void ExecuteHeroEnter(IEntity heroEntity, string worldName, int entryId)
        {
            var task = OpenBreed.Core.Task.Create((t) => services.AddToWorld(t, heroEntity, worldName));

            task.Then((t) => services.PlayerCharacterEnter(t, heroEntity, entryId));

            task.Start();
        }
    }
}
