using OpenBreed.Game.Common;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Entities.Builders;
using OpenBreed.Game.Rendering;
using OpenBreed.Game.Rendering.Helpers;
using OpenBreed.Game.States;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Game
{
    public class World
    {
        #region Private Fields

        private readonly List<IWorldSystem> systems = new List<IWorldSystem>();
        private readonly List<IWorldSystem> toAdd = new List<IWorldSystem>();
        private readonly List<IWorldSystem> toRemove = new List<IWorldSystem>();

        #endregion Private Fields

        #region Public Constructors

        public World(GameState gameState)
        {
            GameState = gameState;

            GenerateMap();
        }

        #endregion Public Constructors

        #region Public Properties

        public List<WorldEntity> Entities { get; } = new List<WorldEntity>();

        public GameState GameState { get; }

        public List<IWorldSystem> Systems { get; }

        #endregion Public Properties

        #region Public Methods

        public virtual void AddSystem(IWorldSystem system)
        {
            toAdd.Add(system);
        }

        public void Initialize()
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i].Initialize();
            }
        }

        public virtual void RemoveSystem(IWorldSystem system)
        {
            toRemove.Add(system);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Cleanup()
        {
            if (toRemove.Any())
            {
                //Process worlds to remove
                for (int i = 0; i < toRemove.Count; i++)
                {
                    toRemove[i].Deinitialize(this);
                    systems.Remove(toRemove[i]);
                }

                toRemove.Clear();
            }

            if (toAdd.Any())
            {
                //Process worlds to add
                for (int i = 0; i < toAdd.Count; i++)
                {
                    systems.Add(toAdd[i]);
                    toAdd[i].Initialize(this);
                }

                toAdd.Clear();
            }
        }

        #endregion Protected Methods

        #region Private Methods

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

        #endregion Private Methods
    }
}