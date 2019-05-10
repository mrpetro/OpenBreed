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
    /// <summary>
    /// World class which contains systems and entities
    /// </summary>
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
            RenderSystem = new RenderSystem(64, 64);

            systems.Add(ControlSystem);
            systems.Add(MovementSystem);
            systems.Add(PhysicsSystem);
            systems.Add(AnimationSystem);
            systems.Add(RenderSystem);
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
            InitializeSystems();
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

        private void InitializeSystems()
        {
            for (int i = 0; i < systems.Count; i++)
                systems[i].Initialize(this);
        }

        private void DeinitializeSystems()
        {
            for (int i = 0; i < systems.Count; i++)
                systems[i].Deinitialize(this);
        }

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

        #endregion Private Methods
    }
}