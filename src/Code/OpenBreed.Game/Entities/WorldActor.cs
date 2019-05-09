using OpenBreed.Game.Animation.Components;
using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Common.Components.Shapes;
using OpenBreed.Game.Control.Components;
using OpenBreed.Game.Entities.Builders;
using OpenBreed.Game.Movement.Components;
using OpenBreed.Game.Physics.Components;
using OpenBreed.Game.Rendering.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities
{
    public class WorldActor : WorldEntity
    {
        Vector2 position;

        public WorldActor(WorldActorBuilder builder) : base(builder)
        {
            this.position = builder.position;

            var position = new DynamicPosition(builder.position);
            var direction = new Direction(builder.direction);
            Components.Add(position);
            Components.Add(direction);
            Components.Add(new Sprite(builder.spriteAtlas));
            Components.Add(new SpriteDebug());
            Components.Add(new CreatureMovement());
            Components.Add(new CreatureAnimator());
            Components.Add(new AxisAlignedBox(32, 32));
            Components.Add(new DynamicBody());

            if (builder.controller != null)
                Components.Add(builder.controller);

        }
    }
}
