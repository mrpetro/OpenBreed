using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Core.Modules.Animation;
using OpenBreed.Core.Modules.Animation.Systems.Control.Systems;
using OpenBreed.Core.Modules.Physics.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Worlds
{
    public static class GameWorldHelper
    {
        public static World SetupSystems(ICore core, World gameWorld, int width, int height)
        {
            //AI
            // Pathfinding/ AI systems here

            //Input
            gameWorld.AddSystem(new WalkingControlSystem(core));
            gameWorld.AddSystem(new AiControlSystem(core));

            //Action
            gameWorld.AddSystem(new MovementSystem(core));
            gameWorld.AddSystem(core.Physics.CreatePhysicsSystem(width, height));
            gameWorld.AddSystem(core.Animations.CreateAnimationSystem<int>());

            //Other
            gameWorld.AddSystem(core.CreateGroupSystem());

            //Audio
            gameWorld.AddSystem(core.Sounds.CreateSoundSystem());

            //Video
            gameWorld.AddSystem(core.Rendering.CreateTileSystem(width, height, 1, 16, true));
            gameWorld.AddSystem(core.Rendering.CreateSpriteSystem());
            gameWorld.AddSystem(core.Rendering.CreateWireframeSystem());
            gameWorld.AddSystem(core.Rendering.CreateTextSystem());

            return gameWorld;
        }

        public static World CreateGameWorld(ICore core, string worldName)
        {
            var gameWorld = core.Worlds.Create(worldName);

            //AI
            // Pathfinding/ AI systems here

            //Input
            gameWorld.AddSystem(new WalkingControlSystem(core));
            gameWorld.AddSystem(new AiControlSystem(core));

            //Action
            gameWorld.AddSystem(new MovementSystem(core));
            gameWorld.AddSystem(core.Physics.CreatePhysicsSystem(64, 64));
            gameWorld.AddSystem(core.Animations.CreateAnimationSystem<int>());

            //Other
            gameWorld.AddSystem(core.CreateGroupSystem());

            //Audio
            gameWorld.AddSystem(core.Sounds.CreateSoundSystem());

            //Video
            gameWorld.AddSystem(core.Rendering.CreateTileSystem(64, 64, 1, 16, true));
            gameWorld.AddSystem(core.Rendering.CreateSpriteSystem());
            gameWorld.AddSystem(core.Rendering.CreateWireframeSystem());
            gameWorld.AddSystem(core.Rendering.CreateTextSystem());

            return gameWorld;
        }
    }
}
