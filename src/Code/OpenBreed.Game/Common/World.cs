using OpenBreed.Game.Animation;
using OpenBreed.Game.Common;
using OpenBreed.Game.Common.Components;
using OpenBreed.Game.Control;
using OpenBreed.Game.Entities;
using OpenBreed.Game.Entities.Builders;
using OpenBreed.Game.Movement;
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
        private readonly List<IWorldEntity> toAdd = new List<IWorldEntity>();
        private readonly List<IWorldEntity> toRemove = new List<IWorldEntity>();
        private readonly List<IWorldSystem> systems = new List<IWorldSystem>();

        #endregion Private Fields

        #region Public Constructors

        public World(GameState core)
        {
            Core = core;
            Entities = new ReadOnlyCollection<IWorldEntity>(entities);

            ControlSystem = new ControlSystem();
            MovementSystem = new MovementSystem();
            PhysicsSystem = new PhysicsSystem(64, 64);
            AnimationSystem = new AnimationSystem();
            RenderSystem = new RenderSystem();

            systems.Add(ControlSystem);
            systems.Add(MovementSystem);
            systems.Add(PhysicsSystem);
            systems.Add(AnimationSystem);
            systems.Add(RenderSystem);

            GenerateMap();
        }

        #endregion Public Constructors

        #region Public Properties

        public GameState Core { get; }

        public ReadOnlyCollection<IWorldEntity> Entities { get; }

        public PhysicsSystem PhysicsSystem { get; }
        public ControlSystem ControlSystem { get; }
        public MovementSystem MovementSystem { get; }
        public AnimationSystem AnimationSystem { get; }
        public RenderSystem RenderSystem { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Method will add given entity to this world.
        /// Entity will not be added immediately but at the end of each world update.
        /// An exception will be thrown if given entity already exists in world
        /// </summary>
        /// <param name="entity">Entity to be added to this world</param>
        public void AddEntity(WorldEntity entity)
        {
            if (entity.CurrentWorld != null)
                throw new InvalidOperationException("Entity can't exist in more than one world.");

            toAdd.Add(entity);
        }

        /// <summary>
        /// Method will remove given entity from this world.
        /// Entity will not be removed immediately but at the end of each world update.
        /// An exception will be thrown if given entity does not exist in this world.
        /// </summary>
        /// <param name="entity">Entity to be removed from this world</param>
        public void RemoveEntity(WorldEntity entity)
        {
            if (entity.CurrentWorld != this)
                throw new InvalidOperationException("Entity doesn't exist in this world");

            toRemove.Add(entity);
        }

        public void Initialize()
        {
            Cleanup();
        }

        public void Update(float dt)
        {
            MovementSystem.Update(dt);

            PhysicsSystem.Update(dt);

            AnimationSystem.Animate(dt);

            Cleanup();
        }

        public void ProcessInputs(double dt)
        {
            ControlSystem.ProcessInputs(dt);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void RegisterEntity(WorldEntity entity)
        {
            //Initialize the entity and add it to entities list
            entity.Initialize(this);
            entities.Add(entity);

            //Add all entity components to world systems
            for (int i = 0; i < entity.Components.Count; i++)
                AddComponent(entity.Components[i]);
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

        protected virtual void RemoveSystem(IWorldSystem system)
        {
            systems.Add(system);
        }

        protected void Cleanup()
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

        private void RemoveComponent(IEntityComponent component)
        {
            var foundSystem = systems.FirstOrDefault(item => item.GetType() == component.SystemType);

            if (foundSystem == null)
                throw new InvalidOperationException($"System {component.SystemType} not registered.");

            foundSystem.RemoveComponent(component);
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
            actorBuilder.SetDirection(new OpenTK.Vector2(1, 0));

            actorBuilder.SetController(new Control.Components.MovementController());
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
        }

        #endregion Private Methods
    }
}