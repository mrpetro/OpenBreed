﻿using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Worlds
{
    /// <summary>
    /// World class which contains systems and entities
    ///
    /// Enqueues events:
    /// WorldInitializedEvent - when world is initialized
    /// </summary>
    internal sealed class World : IWorld
    {
        #region Public Fields

        public const float MAX_TIME_MULTIPLIER = 10.0f;

        #endregion Public Fields

        #region Private Fields

        private readonly Dictionary<Type, object> modules = new Dictionary<Type, object>();
        private readonly HashSet<IEntity> entities = new HashSet<IEntity>();
        private readonly HashSet<IEntity> toAdd = new HashSet<IEntity>();
        private readonly HashSet<IEntity> toRemove = new HashSet<IEntity>();
        private readonly WorldMan worldMan;
        private readonly WorldContext context;
        private float timeMultiplier = 1.0f;

        #endregion Private Fields

        #region Internal Constructors

        internal World(WorldBuilder builder)
        {
            Name = builder.name;
            modules = builder.modules;
            worldMan = builder.worldMan;
            context = new WorldContext(this);
            Systems = builder.CreateSystems(this).ToArray();
        }

        #endregion Internal Constructors

        #region Public Properties

        public ISystem[] Systems { get; }

        /// <summary>
        /// Time "speed" control value, can't be negative but can be 0 (Basicaly stops time).
        /// </summary>
        public float DtMultiplier
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

        public IEnumerable<IEntity> Entities => entities;

        /// <summary>
        /// Id of this world
        /// </summary>
        public int Id { get; internal set; }

        /// <summary>
        /// Name of this world
        /// </summary>
        public string Name { get; }

        #endregion Public Properties

        #region Public Methods

        public override string ToString()
        {
            return $"World:{Name}";
        }

        public T GetSystem<T>() where T : ISystem
        {
            return Systems.OfType<T>().FirstOrDefault();
        }

        public TModule GetModule<TModule>()
        {
            if (modules.TryGetValue(typeof(TModule), out object module))
                return (TModule)module;
            else
                throw new InvalidOperationException($"Module of type '{typeof(TModule)}' not found.");
        }

        //public void Pause() => SetPause(true);

        //public void Unpause() => SetPause(false);

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Method will request to add given entity to this world.
        /// Entity will not be added immediately but at the end of each world update.
        /// An exception will be thrown if given entity already exists in world
        /// </summary>
        /// <param name="entity">Entity to be added to this world</param>
        internal void RequestAddEntity(IEntity entity)
        {
            if (entity.WorldId != WecsConsts.NO_WORLD_ID)
                throw new InvalidOperationException("Entity can't exist in more than one world.");

            toAdd.Add(entity);
        }

        /// <summary>
        /// Method will request to remove given entity from this world.
        /// Entity will not be removed immediately but at the end of each world update.
        /// An exception will be thrown if given entity does not exist in this world.
        /// </summary>
        /// <param name="entity">Entity to be removed from this world</param>
        internal void RequestRemoveEntity(IEntity entity)
        {
            if (entity.WorldId != Id)
                throw new InvalidOperationException("Entity doesn't exist in this world");

            toRemove.Add(entity);
        }

        internal void CheckRemoveFromSystems(IEntity entity, Type componentType)
        {
            foreach (var system in Systems)
            {
                if (!system.HasEntity(entity))
                    continue;

                if (!system.Matches(entity))
                    system.RequestRemoveEntity(entity);
            }
        }

        internal void CheckAddToSystems(IEntity entity, Type componentType)
        {
            foreach (var system in Systems)
            {
                if (system.HasEntity(entity))
                    continue;

                if (system.Matches(entity))
                    system.RequestAddEntity(entity);
            }
        }

        internal void RemovePendingEntities()
        {
            //Perform deinitialization of removed entities
            toRemove.ForEach(item => RemoveEntity(item));
            toRemove.Clear();

            //Perform cleanup on all world systems
            Systems.ForEach(item => item.Cleanup());
        }

        internal void Update(float dt)
        {
            AddPendingEntities();

            context.DtMultiplier = DtMultiplier;
            context.UpdateDeltaTime(dt);

            foreach (var item in Systems.OfType<IUpdatableSystem>())
                item.Update(context);

            RemovePendingEntities();
        }

        #endregion Internal Methods

        #region Private Methods

        private void AddPendingEntities()
        {
            //Perform initialization of added entities
            toAdd.ForEach(item => AddEntity(item));
            toAdd.Clear();
        }

        //private void SetPause(bool paused)
        //{
        //    if (Paused == paused)
        //        return;

        //    Paused = paused;

            //if (Paused)
            //    worldMan.OnWorldPaused(Id);
            //else
            //    worldMan.OnWorldUnpaused(Id);
        //}

        private void RemoveEntity(IEntity entity)
        {
            RemoveFromAllSystems(entity);
            entities.Remove(entity);
            ((Entity)entity).WorldId = WecsConsts.NO_WORLD_ID;
            worldMan.OnEntityRemoved(entity, Id);
        }

        private void AddEntity(IEntity entity)
        {
            AddEntityToSystems(entity);
            entities.Add(entity);
            ((Entity)entity).WorldId = Id;
            worldMan.OnEntityAdded(entity, Id);
        }

        private void AddEntityToSystems(IEntity entity)
        {
            foreach (var system in Systems)
            {
                if (system.Matches(entity))
                    system.RequestAddEntity(entity);
            }
        }

        private void RemoveFromAllSystems(IEntity entity)
        {
            foreach (var system in Systems)
            {
                if (system.Matches(entity))
                    system.RequestRemoveEntity(entity);
            }
        }

        #endregion Private Methods

        //private void InitializeSystems()
        //{
        //    for (int i = 0; i < systems.Count; i++)
        //        systems[i].Initialize(this);
        //}

        //private void DeinitializeSystems()
        //{
        //    for (int i = 0; i < systems.Count; i++)
        //        systems[i].Deinitialize();
        //}
    }
}