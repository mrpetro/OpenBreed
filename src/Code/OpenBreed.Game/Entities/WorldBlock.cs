using OpenBreed.Game.Common;
using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Entities.Builders;
using OpenBreed.Game.Physics;
using OpenBreed.Game.Physics.Components;
using OpenBreed.Game.Rendering.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities
{
    public class WorldBlock : WorldEntity
    {
        public int X { get; }
        public int Y { get; }

        public WorldBlock(WorldBlockBuilder builder) : base(builder)
        {
            X = builder.x;
            Y = builder.y;

            var position = new Position(X * 16, Y * 16);
            Components.Add(position);
            Components.Add(new GridBoxBody(16));
            Components.Add(new Tile(builder.tileAtlas, builder.tileId));
        }
    }
}
