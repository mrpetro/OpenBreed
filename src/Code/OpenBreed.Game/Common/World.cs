using OpenBreed.Game.Common;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Entities.Builders;
using OpenBreed.Game.Physics;
using OpenBreed.Game.Rendering;
using OpenBreed.Game.Rendering.Helpers;
using OpenBreed.Game.States;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Game
{
    public class World
    {
        #region Private Fields

        private readonly List<IWorldEntity> entities = new List<IWorldEntity>();
        private readonly List<IWorldSystem> systems = new List<IWorldSystem>();
        private readonly List<IWorldEntity> toAdd = new List<IWorldEntity>();
        private readonly List<IWorldEntity> toRemove = new List<IWorldEntity>();

        internal void RegisterEntity(WorldEntity entity)
        {
            foreach (var component in entity.Components)
            {
                var foundSystem = systems.FirstOrDefault(item => item.GetType() == component.SystemType);

                if (foundSystem == null)
                    throw new InvalidOperationException($"System {component.SystemType} not registered.");

                foundSystem.AddComponent(component);
            }
        }

        internal void UnregisterEntity(WorldEntity entity)
        {
            foreach (var component in entity.Components)
            {
                var foundSystem = systems.FirstOrDefault(item => item.GetType() == component.SystemType);

                if (foundSystem == null)
                    throw new InvalidOperationException($"System {component.SystemType} not registered.");

                foundSystem.RemoveComponent(component);
            }
        }

        #endregion Private Fields

        #region Public Constructors

        public World(GameState core)
        {
            Core = core;
            Entities = new ReadOnlyCollection<IWorldEntity>(entities);

            RenderSystem = new RenderSystem();
            PhysicsSystem = new PhysicsSystem();
            systems.Add(RenderSystem);
            systems.Add(PhysicsSystem);

            GenerateMap();
        }

        #endregion Public Constructors

        #region Public Properties

        public GameState Core { get; }
        public ReadOnlyCollection<IWorldEntity> Entities { get; }

        public PhysicsSystem PhysicsSystem { get; }
        public RenderSystem RenderSystem { get; }

        #endregion Public Properties

        #region Public Methods

        public void AddEntity(WorldEntity entity)
        {
            toAdd.Add(entity);
        }

        public void Initialize()
        {
            Cleanup();
        }

        public void RemoveEntity(WorldEntity entity)
        {
            toRemove.Add(entity);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void AddSystem(IWorldSystem system)
        {
            systems.Add(system);
        }

        protected virtual void Cleanup()
        {
            if (toRemove.Any())
            {
                //Process entities to remove
                for (int i = 0; i < toRemove.Count; i++)
                {
                    toRemove[i].LeaveWorld();
                    entities.Remove(toRemove[i]);
                }

                toRemove.Clear();
            }

            if (toAdd.Any())
            {
                //Process entities to add
                for (int i = 0; i < toAdd.Count; i++)
                {
                    entities.Add(toAdd[i]);
                    toAdd[i].EnterWorld(this);
                }

                toAdd.Clear();
            }
        }

        protected virtual void RemoveSystem(IWorldSystem system)
        {
            systems.Add(system);
        }

        #endregion Protected Methods

        #region Private Methods

        private void GenerateMap()
        {
            var tileTex = Core.TextureMan.Load(@"Content\TileAtlasTest32bit.bmp");

            var tileAtlas = new TileAtlas(tileTex, 16, 4, 4);

            int width = 64;
            int height = 64;

            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas(tileAtlas);

            var rnd = new Random();

            blockBuilder.SetIndices(0, 0);
            blockBuilder.SetTileId(0);
            AddEntity((WorldBlock)blockBuilder.Build());

            blockBuilder.SetIndices(4, 0);
            blockBuilder.SetTileId(1);
            AddEntity((WorldBlock)blockBuilder.Build());

            blockBuilder.SetIndices(4, 4);
            blockBuilder.SetTileId(2);
            AddEntity((WorldBlock)blockBuilder.Build());

            blockBuilder.SetIndices(0, 4);
            blockBuilder.SetTileId(3);
            AddEntity((WorldBlock)blockBuilder.Build());

            //for (int y = 0; y < height; y++)
            //{
            //    for (int x = 0; x < width; x++)
            //    {
            //        blockBuilder.SetIndices(x, y);
            //        blockBuilder.SetTileId(rnd.Next() % 16);
            //        AddEntity((WorldBlock)blockBuilder.Build());
            //    }
            //}
        }

        public void Update(double dt)
        {
            for (int i = 0; i < systems.Count; i++)
                systems[i].Update(dt);
        }

        #endregion Private Methods
    }
}