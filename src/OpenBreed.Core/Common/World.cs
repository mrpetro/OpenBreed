﻿using OpenBreed.Core.Commands;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Events;
using OpenBreed.Core.Extensions;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Managers;
using OpenBreed.Core.Systems;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace OpenBreed.Core.Common
{
    /// <summary>
    /// World class which contains systems and entities
    ///
    /// Enqueues events:
    /// WorldInitializedEvent - when world is initialized
    /// </summary>
    public class World
    {
        #region Public Fields

        public const float MAX_TIME_MULTIPLIER = 10.0f;

        #endregion Public Fields

        #region Private Fields

        private readonly List<Entity> entities = new List<Entity>();
        private readonly List<Entity> toAdd = new List<Entity>();
        private readonly List<Entity> toRemove = new List<Entity>();
        private readonly List<IWorldSystem> systems = new List<IWorldSystem>();

        private float timeMultiplier = 1.0f;

        #endregion Private Fields

        #region Internal Constructors

        internal World(WorldBuilder builder)
        {
            Core = builder.core;
            Name = builder.name;

            Entities = new ReadOnlyCollection<Entity>(entities);

            systems = builder.systems;
            Systems = new ReadOnlyCollection<IWorldSystem>(systems);

            Components = new ComponentsMan();

            Core.Worlds.RegisterWorld(this);

            foreach (var system in systems)
                system.Initialize(this);

            builder.InvokeActions(this);
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Pauses or unpauses this world
        /// </summary>
        public bool Paused { get; private set; }

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

        public ReadOnlyCollection<Entity> Entities { get; }

        public ReadOnlyCollection<IWorldSystem> Systems { get; }

        public ComponentsMan Components { get; }

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

        public override string ToString()
        {
            return $"World:{Name}";
        }

        /// <summary>
        /// Method will remove all entities from this world.
        /// Entities will not be removed immediately but at the end of each world update.
        /// </summary>
        public void RemoveAllEntities()
        {
            for (int i = 0; i < entities.Count; i++)
                RemoveEntity(entities[i]);
        }

        public T GetSystem<T>() where T : IWorldSystem
        {
            return systems.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Method will add given entity to this world.
        /// Entity will not be added immediately but at the end of each world update.
        /// An exception will be thrown if given entity already exists in world
        /// </summary>
        /// <param name="entity">Entity to be added to this world</param>
        public void AddEntity(Entity entity)
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
        public void RemoveEntity(Entity entity)
        {
            if (entity.World != this)
                throw new InvalidOperationException("Entity doesn't exist in this world");

            toRemove.Add(entity);
        }

        public void Pause(bool value)
        {
            if (Paused == value)
                return;

            Paused = value;

            if (Paused)
                Core.Worlds.RaiseEvent(new WorldPausedEventArgs(Id));
            else
                Core.Worlds.RaiseEvent(new WorldUnpausedEventArgs(Id));
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Update(float dt)
        {
            if (Paused)
            {
                foreach (var item in systems.OfType<IUpdatableSystem>())
                {
                    Core.Commands.ExecuteEnqueued();
                    item.UpdatePauseImmuneOnly(dt * TimeMultiplier);
                }
                //systems.OfType<IUpdatableSystem>().ForEach(item => item.UpdatePauseImmuneOnly(dt * TimeMultiplier));
            }

            else
            {
                foreach (var item in systems.OfType<IUpdatableSystem>())
                {
                    Core.Commands.ExecuteEnqueued();
                    item.Update(dt * TimeMultiplier);
                }
                //systems.OfType<IUpdatableSystem>().ForEach(item => item.Update(dt * TimeMultiplier));
            }
        }

        internal void Initialize()
        {
            //InitializeSystems();
            Cleanup();

            Core.Worlds.RaiseEvent(new WorldInitializedEventArgs(Id));
        }

        internal void Deinitialize()
        {
            Core.Worlds.RaiseEvent(new WorldDeinitializedEventArgs(Id));
        }

        internal void Cleanup()
        {
            //Perform deinitialization of removed entities
            toRemove.ForEach(item => DeinitializeEntity(item));

            //Perform cleanup on all world systems
            Systems.ForEach(item => item.Cleanup());

            //Perform initialization of added entities
            toAdd.ForEach(item => InitializeEntity(item));

            toRemove.Clear();
            toAdd.Clear();
        }

        #endregion Internal Methods

        #region Private Methods

        private void DeinitializeEntity(Entity entity)
        {
            ((Entity)entity).World = null;
            entities.Remove(entity);
            RemoveEntityFromSystems(entity);
            OnEntityRemoved(entity);
        }

        private void InitializeEntity(Entity entity)
        {
            entities.Add(entity);
            ((Entity)entity).World = this;
            AddEntityToSystems(entity);
            OnEntityAdded(entity);
        }

        private void OnEntityAdded(Entity entity)
        {
            Core.Worlds.RaiseEvent(new EntityAddedEventArgs(Id, entity.Id));
        }

        private void OnEntityRemoved(Entity entity)
        {
            Core.Worlds.RaiseEvent(new EntityRemovedEventArgs(Id, entity.Id));
        }

        private void InitializeSystems()
        {
            for (int i = 0; i < systems.Count; i++)
                systems[i].Initialize(this);
        }

        private void AddEntityToSystems(Entity entity)
        {
            foreach (var system in systems)
            {
                if (system.Matches(entity))
                    system.AddEntity(entity);
            }
        }

        private void RemoveEntityFromSystems(Entity entity)
        {
            foreach (var system in systems)
            {
                if (system.Matches(entity))
                    system.RemoveEntity(entity);
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