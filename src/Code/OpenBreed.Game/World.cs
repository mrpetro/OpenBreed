using OpenBreed.Game.Entities;
using OpenBreed.Game.Entities.Builders;
using OpenBreed.Game.Entities.Components;
using OpenBreed.Game.Rendering;
using OpenBreed.Game.States;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Game
{
    public class World
    {
        public GameState GameState { get; }


        public List<WorldEntity> Entities { get; } = new List<WorldEntity>();

        private void GenerateMap()
        {

            var tileTex = GameState.TextureMan.Load(@"Content\TileAtlasTest32bit.bmp");


            var tileAtlas = new TileAtlas(tileTex, 16, 4, 4);


            int width = 64;
            int height = 64;

            var blockBuilder = new WorldBlockBuilder(GameState);
            blockBuilder.SetWorld(this);
            blockBuilder.SetTileAtlas(tileAtlas);

            var rnd = new Random();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    blockBuilder.SetIndices(x, y);
                    blockBuilder.SetTileId(rnd.Next() % 16);
                    Entities.Add((WorldBlock)blockBuilder.Build());
                }
            }
        }


        public void Initialize()
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i].Initialize();
            }
        }

        public World(GameState gameState)
        {
            GameState = gameState;

            GenerateMap();

        }
    }
}
