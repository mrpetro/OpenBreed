using OpenBreed.Core;
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
            AddSystem(Core.Sounds.CreateSoundSystem());
            AddSystem(Core.CreateControlSystem());
            AddSystem(Core.CreateMovementSystem());
            AddSystem(Core.Physics.CreatePhysicsSystem(64, 64));
            AddSystem(Core.CreateAnimationSystem());

            AddSystem(Core.Rendering.CreateTileSystem(64, 64, 16));
            AddSystem(Core.Rendering.CreateSpriteSystem());
            AddSystem(Core.Rendering.CreateTextSystem());
            //AddSystem(Core.Rendering.CreateRenderSystem(64, 64, 16));
        }
    }
}
