using OpenBreed.Game.Rendering;
using OpenBreed.Game.Rendering.Helpers;
using OpenBreed.Game.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Entities.Builders
{
    public class WorldBlockBuilder : WorldEntityBuilder
    {
        internal int x;
        internal int y;
        internal TileAtlas tileAtlas;
        internal int tileId;

        public WorldBlockBuilder(GameState core) : base(core)
        {
        }

        public void SetIndices(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void SetTileAtlas(TileAtlas tileAtlas)
        {
            this.tileAtlas = tileAtlas;
        }

        public void SetTileId(int tileId)
        {
            this.tileId = tileId;
        }

        public override IEntity Build()
        {
            return new WorldBlock(this);
        }
    }
}
