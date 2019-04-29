using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities.Builders;
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
            Components.Add(transform);
            Components.Add(new Sprite(builder.spriteAtlas, builder.spriteId, transform));
        }
    }
}
