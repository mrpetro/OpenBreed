using OpenBreed.Core;
using OpenBreed.Core.Systems.Animation;
using OpenBreed.Core.Systems.Control.Systems;
using OpenBreed.Core.Systems.Movement.Systems;
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

            //Input
            AddSystem(new KeyboardControlSystem(core));
            AddSystem(new AiControlSystem(core));

            //Action
            AddSystem(new MovementSystem(core));
            AddSystem(Core.Physics.CreatePhysicsSystem(64, 64));
            AddSystem(new AnimSystem<int>(core));

            //Audio
            AddSystem(Core.Sounds.CreateSoundSystem());

            //Video
            AddSystem(Core.Rendering.CreateTileSystem(64, 64, 16, true));
            AddSystem(Core.Rendering.CreateSpriteSystem());
            AddSystem(Core.Rendering.CreateTextSystem());
        }
    }
}
