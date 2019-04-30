﻿using OpenBreed.Game.Common;
using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Control;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Entities.Builders;
using OpenBreed.Game.Physics;
using OpenBreed.Game.Rendering;
using OpenBreed.Game.Rendering.Helpers;
using OpenBreed.Game.States;
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

        #endregion Private Fields

        #region Public Constructors

        public World(GameState core)
        {
            Core = core;
            Entities = new ReadOnlyCollection<IWorldEntity>(entities);

            RenderSystem = new RenderSystem();
            PhysicsSystem = new PhysicsSystem();
            ControlSystem = new ControlSystem();
            systems.Add(RenderSystem);
            systems.Add(PhysicsSystem);
            systems.Add(ControlSystem);

            GenerateMap();
        }

        #endregion Public Constructors

        #region Public Properties

        public GameState Core { get; }

        public ReadOnlyCollection<IWorldEntity> Entities { get; }

        public PhysicsSystem PhysicsSystem { get; }
        public ControlSystem ControlSystem { get; }
        public RenderSystem RenderSystem { get; }

        #endregion Public Properties

        #region Public Methods

        public void AddEntity(WorldEntity entity)
        {
            if (entity.CurrentWorld != null)
                throw new InvalidOperationException("Entity can't exist in more than one world.");

            toAdd.Add(entity);
        }

        public void Initialize()
        {
            Cleanup();
        }

        public void RemoveEntity(WorldEntity entity)
        {
            if (entity.CurrentWorld == null)
                throw new InvalidOperationException("Entity doesn't exist in any world.");

            toRemove.Add(entity);
        }

        public void Update(double dt)
        {
            for (int i = 0; i < systems.Count; i++)
                systems[i].Update(dt);
        }

        public void ProcessInputs(double dt)
        {
            ControlSystem.ProcessInputs(dt);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void RegisterEntity(WorldEntity entity)
        {
            //Add all entity components to world systems
            for (int i = 0; i < entity.Components.Count; i++)
                AddComponent(entity.Components[i]);

            //Initialize the entity and add it to entities list
            entity.Initialize(this);
            entities.Add(entity);
        }

        internal void UnregisterEntity(WorldEntity entity)
        {
            //Remove all entity components from world systems
            for (int i = 0; i < entity.Components.Count; i++)
                RemoveComponent(entity.Components[i]);

            //Deinitialize the entity and remove it from entities list
            entity.Deinitialize();
            entities.Remove(entity);
        }

        #endregion Internal Methods

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
                    UnregisterEntity((WorldEntity)toRemove[i]);

                toRemove.Clear();
            }

            if (toAdd.Any())
            {
                //Process entities to add
                for (int i = 0; i < toAdd.Count; i++)
                    RegisterEntity((WorldEntity)toAdd[i]);

                toAdd.Clear();
            }
        }

        protected virtual void RemoveSystem(IWorldSystem system)
        {
            systems.Add(system);
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddComponent(IEntityComponent component)
        {
            if (component.SystemType == null)
                return;

            var foundSystem = systems.FirstOrDefault(item => item.GetType() == component.SystemType);

            if (foundSystem == null)
                throw new InvalidOperationException($"System {component.SystemType} not registered.");

            foundSystem.AddComponent(component);
        }

        private void GenerateMap()
        {
            var tileTex = Core.TextureMan.Load(@"Content\TileAtlasTest32bit.bmp");

            var spriteTex = Core.TextureMan.Load(@"Content\ArrowSpriteSet.png");

            var tileAtlas = new TileAtlas(tileTex, 16, 4, 4);
            var spriteAtlas = new SpriteAtlas(spriteTex, 32, 8, 1);

            int width = 64;
            int height = 64;

            var blockBuilder = new WorldBlockBuilder(Core);
            blockBuilder.SetTileAtlas(tileAtlas);

            var actorBuilder = new WorldActorBuilder(Core);
            actorBuilder.SetSpriteAtlas(spriteAtlas);
            actorBuilder.SetPosition(new OpenTK.Vector2(20, 20));
            actorBuilder.SetSpriteId(7);
            AddEntity((WorldActor)actorBuilder.Build());

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

        private void RemoveComponent(IEntityComponent component)
        {
            var foundSystem = systems.FirstOrDefault(item => item.GetType() == component.SystemType);

            if (foundSystem == null)
                throw new InvalidOperationException($"System {component.SystemType} not registered.");

            foundSystem.RemoveComponent(component);
        }

        #endregion Private Methods
    }
}