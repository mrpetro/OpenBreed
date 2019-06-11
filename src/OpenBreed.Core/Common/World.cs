using OpenBreed.Core.Entities;
using OpenBreed.Core.Modules.Audio.Systems;
using OpenBreed.Core.Modules.Physics;
using OpenBreed.Core.Modules.Rendering;
using OpenBreed.Core.Modules.Rendering.Systems;
using OpenBreed.Core.Systems;
using OpenBreed.Core.Systems.Common.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Core
{
    /// <summary>
    /// World class which contains systems and entities
    /// </summary>
    public class World
    {
        #region Public Fields

        public const float MAX_TILE_MULTIPLIER = 10.0f;

        #endregion Public Fields

        #region Private Fields

        private readonly List<IWorldEntity> entities = new List<IWorldEntity>();
        private readonly List<IWorldEntity> toAdd = new List<IWorldEntity>();
        private readonly List<IWorldEntity> toRemove = new List<IWorldEntity>();
        private readonly List<IWorldSystem> systems = new List<IWorldSystem>();

        private float timeMultiplier = 1.0f;

        #endregion Private Fields

        #region Public Constructors

        public World(ICore core)
        {
            Core = core;
            Entities = new ReadOnlyCollection<IWorldEntity>(entities);

            SoundSystem = Core.Sounds.CreateSoundSystem();
            ControlSystem = Core.CreateControlSystem();
            MovementSystem = Core.CreateMovementSystem();
            PhysicsSystem = Core.Physics.CreatePhysicsSystem(64, 64);
            AnimationSystem = Core.CreateAnimationSystem();
            RenderSystem = Core.Rendering.CreateRenderSystem(64,64);

            //systems.Add(SoundSystem);

            systems.Add(ControlSystem);
            systems.Add(MovementSystem);
            systems.Add(PhysicsSystem);
            systems.Add(AnimationSystem);
            systems.Add(RenderSystem);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Time "speed" control value, can't be negative but can be 0 (Basicaly stops time).
        /// </summary>
        public float TimeMultiplier
        {
            get
            {
                return timeMultiplier;
            }

            set
            {
                timeMultiplier = MathHelper.Clamp(value, 0, MAX_TILE_MULTIPLIER);
            }
        }

        public ICore Core { get; }

        public ReadOnlyCollection<IWorldEntity> Entities { get; }

        public IAudioSystem SoundSystem { get; }
        public IPhysicsSystem PhysicsSystem { get; }
        public IControlSystem ControlSystem { get; }
        public IMovementSystem MovementSystem { get; }
        public IAnimationSystem AnimationSystem { get; }
        public IRenderSystem RenderSystem { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Method will add given entity to this world.
        /// Entity will not be added immediately but at the end of each world update.
        /// An exception will be thrown if given entity already exists in world
        /// </summary>
        /// <param name="entity">Entity to be added to this world</param>
        public void AddEntity(IWorldEntity entity)
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

        public void Deinitialize()
        {
        }

        public void Update(float dt)
        {
            ControlSystem.Update(dt * TimeMultiplier);

            MovementSystem.Update(dt * TimeMultiplier);

            PhysicsSystem.Update(dt * TimeMultiplier);

            AnimationSystem.Update(dt * TimeMultiplier);

            Cleanup();
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