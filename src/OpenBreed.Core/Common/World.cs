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
using OpenBreed.Core.Extensions;

namespace OpenBreed.Core
{
    public delegate void SystemEventDelegate(ISystemEvent systemEvent);


    /// <summary>
    /// World class which contains systems and entities
    /// </summary>
    public class World
    {

        #region Public Fields

        public const float MAX_TILE_MULTIPLIER = 10.0f;

        #endregion Public Fields

        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly List<IEntity> toAdd = new List<IEntity>();
        private readonly List<IEntity> toRemove = new List<IEntity>();
        private readonly List<IWorldSystem> systems = new List<IWorldSystem>();

        private float timeMultiplier = 1.0f;

        #endregion Private Fields

        #region Public Constructors

        protected World(ICore core)
        {
            Core = core;
            Entities = new ReadOnlyCollection<IEntity>(entities);
            Systems = new ReadOnlyCollection<IWorldSystem>(systems);
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

        public ReadOnlyCollection<IEntity> Entities { get; }
        public ReadOnlyCollection<IWorldSystem> Systems { get; }

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

        public void PostMsg(IEntity sender, IEntityMsg entityMsg)
        {
            foreach (var system in Systems)
            {
                if (system.HandleMsg(sender, entityMsg))
                    break;
            }
        }

        //public void SubscribeEvent(string eventType, 

        //private readonly Dictionary<string, List<IEntity>> eventListeners = new Dictionary<string, List<IEntity>>();

        //public void PostEvent(IWorldSystem sender, ISystemEvent systemEvent)
        //{
        //    foreach (var item in eventListeners)
        //    {
        //        List<IEntity> listeners = null;
        //        if (!eventListeners.TryGetValue(systemEvent.Type, out listeners))
        //            return;

        //        foreach (var listener in listeners)
        //            listener.
        //    }
        //}

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
            Cleanup();

            systems.OfType<IUpdatableSystem>().ForEach(item => item.Update(dt * TimeMultiplier));
        }

        #endregion Public Methods

        #region Internal Methods

        internal void RegisterEntity(Entity entity)
        {
            //Initialize the entity and add it to entities list
            entity.Initialize(this);
            entities.Add(entity);

            AddToSystems(entity);
        }

        internal void UnregisterEntity(Entity entity)
        {
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
            systems.Remove(system);
        }

        protected void Cleanup()
        {
            if (toRemove.Any())
            {
                //Process entities to remove
                for (int i = 0; i < toRemove.Count; i++)
                    UnregisterEntity((Entity)toRemove[i]);

                toRemove.Clear();
            }

            if (toAdd.Any())
            {
                //Process entities to add
                for (int i = 0; i < toAdd.Count; i++)
                    RegisterEntity((Entity)toAdd[i]);

                toAdd.Clear();
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void AddToSystems(Entity entity)
        {
            foreach (var system in systems)
            {
                if (system.Matches(entity))
                    system.AddEntity(entity);
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