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
    public class HudWorld : World
    {
        public HudWorld(ICore core) : base(core)
        {

            //Input
            AddSystem(new KeyboardControlSystem(core));
            AddSystem(new AiControlSystem(core));

            //Action
            AddSystem(new MovementSystem(core));
            AddSystem(new AnimSystem<int>(core));

            //Audio
            AddSystem(Core.Sounds.CreateSoundSystem());

            //Video
            AddSystem(Core.Rendering.CreateTileSystem(64, 64, 16, false));
            AddSystem(Core.Rendering.CreateSpriteSystem());
            AddSystem(Core.Rendering.CreateTextSystem());
        }
    }
}
