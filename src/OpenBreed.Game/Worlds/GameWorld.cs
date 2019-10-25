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

namespace OpenBreed.Game.Worlds
{
    public class GameWorld : World
    {
        public GameWorld(ICore core) : base(core)
        {

            //AI
            // Pathfinding/ AI systems here

            //Input
            AddSystem(new WalkingControlSystem(core));
            AddSystem(new AiControlSystem(core));

            //Action
            AddSystem(new MovementSystem(core));
            AddSystem(Core.Physics.CreatePhysicsSystem(64, 64));
            AddSystem(Core.Animations.CreateAnimationSystem<int>());

            //Other
            AddSystem(Core.CreateGroupSystem());

            //Audio
            AddSystem(Core.Sounds.CreateSoundSystem());

            //Video
            AddSystem(Core.Rendering.CreateTileSystem(64, 64, 1, 16, true));
            AddSystem(Core.Rendering.CreateSpriteSystem());
            AddSystem(Core.Rendering.CreateWireframeSystem());
            AddSystem(Core.Rendering.CreateTextSystem());
        }
    }
}
