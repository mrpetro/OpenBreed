using OpenBreed.Core.Common.Helpers;
using OpenBreed.Core.Common.Systems;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Core.States;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Core.Common
{
    /// <summary>
    /// World class which contains systems and entities
    /// </summary>
    public class World
    {
        #region Public Fields

        public const float MAX_TIME_MULTIPLIER = 10.0f;

        #endregion Public Fields

        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly List<IEntity> toAdd = new List<IEntity>();
        private readonly List<IEntity> toRemove = new List<IEntity>();
        private readonly List<IWorldSystem> systems = new List<IWorldSystem>();

        private float timeMultiplier = 1.0f;

        #endregion Private Fields

        #region Internal Constructors

        internal World(ICore core, string name)
        {
            Core = core;
            Name = name;

            Entities = new ReadOnlyCollection<IEntity>(entities);
            Systems = new ReadOnlyCollection<IWorldSystem>(systems);
            Components = new ComponentsMan();

            MessageBus = new WorldMessageBus(this);
            MessageBus.RegisterHandler(StateChangeMsg.TYPE, new StateChangeMsgHandler(this));
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Pauses or unpauses this world
        /// </summary>
        public bool Paused { get; set; }

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
                timeMultiplier = MathHelper.Clamp(value, 0, MAX_TIME_MULTIPLIER);
            }
        }

        public ICore Core { get; }
        public ReadOnlyCollection<IEntity> Entities { get; }
        public ReadOnlyCollection<IWorldSystem> Systems { get; }
        public ComponentsMan Components { get; }
        public WorldMessageBus MessageBus { get; }

        /// <summary>
        /// Id of this world
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// Name of this world
        /// </summary>
        public string Name { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Method will add given entity to this world.
        /// Entity will not be added immediately but at the end of each world update.
        /// An exception will be thrown if given entity already exists in world
        /// </summary>
        /// <param name="entity">Entity to be added to this world</param>
        public void AddEntity(IEntity entity)
        {
            if (entity.World != null)
                throw new InvalidOperationException("Entity can't exist in more than one world.");

            toAdd.Add(entity);
        }

        /// <summary>
        /// Method will remove given entity from this world.
        /// Entity will not be removed immediately but at the end of each world update.
        /// An exception will be thrown if given entity does not exist in this world.
        /// </summary>
        /// <param name="entity">Entity to be removed from this world</param>
        public void RemoveEntity(IEntity entity)
        {
            if (entity.World != this)
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
            if (Paused)
                systems.OfType<IUpdatableSystem>().ForEach(item => item.UpdatePauseImmuneOnly(dt * TimeMultiplier));
            else
                systems.OfType<IUpdatableSystem>().ForEach(item => item.Update(dt * TimeMultiplier));
        }

        public void RemoveAllEntities()
        {
            for (int i = 0; i < entities.Count; i++)
                RemoveEntity(entities[i]);
        }

        public virtual void AddSystem(IWorldSystem system)
        {
            systems.Add(system);
        }

        public virtual void RemoveSystem(IWorldSystem system)
        {
            systems.Remove(system);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void RegisterEntity(Entity entity)
        {
            AddToSystems(entity);

            //Initialize the entity and add it to entities list
            entities.Add(entity);
        }

        internal void UnregisterEntity(Entity entity)
        {
            //Deinitialize the entity and remove it from entities list
            entities.Remove(entity);

            RemoveFromSystems(entity);
        }

        internal void Cleanup()
        {
            //Perform deinitialization of removed entities
            toRemove.ForEach(item => ((Entity)item).Deinitialize());

            //Process entities to remove
            for (int i = 0; i < toRemove.Count; i++)
                UnregisterEntity((Entity)toRemove[i]);

            //Process entities to add
            for (int i = 0; i < toAdd.Count; i++)
                RegisterEntity((Entity)toAdd[i]);

            //Perform cleanup on all world systems
            Systems.ForEach(item => item.Cleanup());

            //Perform initialization of added entities
            toAdd.ForEach(item => ((Entity)item).Initialize(this));

            toRemove.Clear();
            toAdd.Clear();
        }

        #endregion Internal Methods

        #region Private Methods

        private void AddToSystems(Entity entity)
        {
            foreach (var system in systems)
            {
                if (system.Matches(entity))
                    system.AddEntity(entity);
            }
        }

        private void RemoveFromSystems(Entity entity)
        {
            foreach (var system in systems)
            {
                if (system.Matches(entity))
                    system.RemoveEntity(entity);
            }
        }

        private void InitializeSystems()
        {
            for (int i = 0; i < systems.Count; i++)
                systems[i].Initialize(this);
        }

        private void DeinitializeSystems()
        {
            for (int i = 0; i < systems.Count; i++)
                systems[i].Deinitialize();
        }

        #endregion Private Methods
    }
}