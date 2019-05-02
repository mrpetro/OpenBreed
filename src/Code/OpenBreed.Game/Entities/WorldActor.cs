using OpenBreed.Game.Animation.Components;
using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Control.Components;
using OpenBreed.Game.Entities.Builders;
using OpenBreed.Game.Movement.Components;
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
            position = builder.position;

            var transform = new Transformation(builder.position);
            var direction = new Direction(builder.direction);
            Components.Add(transform);
            Components.Add(direction);
            Components.Add(new Sprite(builder.spriteAtlas));
            Components.Add(new CreatureMovement());
            Components.Add(new CreatureAnimator());

            if (builder.controller != null)
                Components.Add(builder.controller);

        }
    }
}
