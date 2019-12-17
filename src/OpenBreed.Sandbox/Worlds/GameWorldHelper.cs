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
        public static void AddSystems(Program core, WorldBuilder builder)
        {
            int width = builder.width;
            int height = builder.height;

            //AI
            // Pathfinding/ AI systems here

            //Input
            builder.AddSystem(core.CreateWalkingControlSystem().Build());
            builder.AddSystem(core.CreateAiControlSystem().Build());

            //Action
            builder.AddSystem(core.CreateMovementSystem().Build());
            builder.AddSystem(core.CreatePhysicsSystem().SetGridSize(width, height).Build());
            builder.AddSystem(core.CreateAnimationSystem().Build());

            //Audio
            builder.AddSystem(core.CreateSoundSystem().Build());

            //Video
            builder.AddSystem(core.CreateTileSystem().SetGridSize(width, height)
                                                       .SetLayersNo(1)
                                                       .SetTileSize(16)
                                                       .SetGridVisible(true)
                                                       .Build());
            builder.AddSystem(core.CreateSpriteSystem().Build());
            builder.AddSystem(core.CreateWireframeSystem().Build());
            builder.AddSystem(core.CreateTextSystem().Build());
        }

        public static World CreateGameWorld(Program core, string worldName)
        {
            var builder = core.Worlds.Create().SetName(worldName);
            AddSystems(core, builder);

            return builder.Build();
        }
    }
}
