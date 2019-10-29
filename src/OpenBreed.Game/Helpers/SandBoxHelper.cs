using OpenBreed.Core;
using OpenBreed.Core.Common;
using OpenBreed.Game.Entities.Builders;
using OpenBreed.Game.Worlds;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game.Helpers
{
    public static class SandBoxHelper
    {
        public static void SetupMap(World world)
        {
            var blockBuilder = new WorldBlockBuilder(world.Core);
            blockBuilder.SetTileAtlas("Atlases/Tiles/16/Test");

            blockBuilder.HasBody = false;
            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    blockBuilder.SetPosition(new Vector2(x * 16, y * 16));
                    blockBuilder.SetTileId(12);
                    world.AddEntity(blockBuilder.Build());
                }
            }

            blockBuilder.HasBody = true;

            for (int x = 0; x < 64; x++)
            {
                blockBuilder.SetPosition(new Vector2(x * 16, 0));
                blockBuilder.SetTileId(9);
                world.AddEntity(blockBuilder.Build());

                blockBuilder.SetPosition(new Vector2(x * 16, 62 * 16));
                blockBuilder.SetTileId(9);
                world.AddEntity(blockBuilder.Build());
            }

            for (int y = 0; y < 64; y++)
            {
                blockBuilder.SetPosition(new Vector2(0, y * 16));
                blockBuilder.SetTileId(9);
                world.AddEntity(blockBuilder.Build());

                blockBuilder.SetPosition(new Vector2(62 * 16, y * 16));
                blockBuilder.SetTileId(9);
                world.AddEntity(blockBuilder.Build());
            }
        }
    }
}
