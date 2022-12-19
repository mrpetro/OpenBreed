using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Tools.Collections;
using OpenBreed.Core.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Worlds
{
    /// <summary>
    /// Manager responsible for creating, removing and updating core worlds
    /// </summary>
    internal class WorldMan : IWorldMan
    {
        #region Private Fields

        private readonly Dictionary<IWorld, HashSet<IEntity>> entitiesToAdd = new Dictionary<IWorld, HashSet<IEntity>>();
        private readonly Dictionary<IWorld, HashSet<IEntity>> entitiesToRemove = new Dictionary<IWorld, HashSet<IEntity>>();
        private readonly HashSet<IEntity> entitiesToUpdate = new HashSet<IEntity>();
        private readonly IEntityMan entityMan;
        private readonly IEntityToSystemMatcher entityToSystemMatcher;
        private readonly IEventsMan eventsMan;
        private readonly IdMap<World> IdsToWorldsLookup = new IdMap<World>();
        private readonly ILogger logger;
        private readonly Dictionary<string, int> namesToIdsLookup = new Dictionary<string, int>();

        //private readonly HashSet<IEntity> entitiesToUpdate = new HashSet<IEntity>();
        private readonly ISystemFactory systemFactory;

        private readonly HashSet<World> toDeinitialize = new HashSet<World>();
        private readonly HashSet<World> toInitialize = new HashSet<World>();
        private readonly List<World> worlds = new List<World>();

        #endregion Private Fields

        #region Public Constructors

        public WorldMan(
            IEntityMan entityMan,
            IEventsMan eventsMan,
            ISystemFactory systemFactory,
            IEntityToSystemMatcher entityToSystemMatcher,
            ILogger logger)
        {
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
            this.systemFactory = systemFactory;
            this.entityToSystemMatcher = entityToSystemMatcher;
            this.logger = logger;

            entityMan.ComponentAdded += EntityMan_ComponentAdded;
            entityMan.ComponentRemoved += EntityMan_ComponentRemoved;
            entityMan.EnterWorldRequested += EntityMan_EnterWorldRequested;
            entityMan.LeaveWorldRequested += EntityMan_LeaveWorldRequested;
        }

        #endregion Public Constructors

        #region Public Methods

        public IWorldBuilder Create()
        {
            return new WorldBuilder(this, entityToSystemMatcher, logger, systemFactory);
        }

        /// <summary>
        /// Returns world with given ID. Will throw exception when such world has not been found.
        /// </summary>
        /// <param name="id">If of world to be returned</param>
        /// <returns>World reference</returns>
        public IWorld GetById(int id)
        {
            if (IdsToWorldsLookup.TryGetValue(id, out World world))
                return world;
            else
                throw new InvalidOperationException($"World with ID '{id}' not found.");
        }

        /// <summary>
        /// Gets world by it's name
        /// </summary>
        /// <param name="name">Name of world to find</param>
        /// <returns>World object if found, null otherwise</returns>
        public IWorld GetByName(string name)
        {
            int worldId;
            if (!namesToIdsLookup.TryGetValue(name, out worldId))
                return null;

            return IdsToWorldsLookup[worldId];
        }

        public void RegisterWorld(World newWorld)
        {
            newWorld.Id = IdsToWorldsLookup.Add(newWorld);
            namesToIdsLookup.Add(newWorld.Name, newWorld.Id);
            toInitialize.Add(newWorld);
        }

        /// <summary>
        /// Marks given world to be removed from Core, it will be removed at nearest Manager Update
        /// </summary>
        /// <param name="world">World to be removed</param>
        public void Remove(IWorld world)
        {
            if (toDeinitialize.Contains(world))
            {
                logger.Warning($"World '{world}' already pending removing.");
                return;
            }

            toDeinitialize.Add((World)world);
        }

        /// <summary>
        /// Updates all worlds
        /// </summary>
        /// <param name="dt">Delta time</param>
        public void Update(float dt)
        {
            InitializePendingWorlds();

            RemovePendingEntities();

            foreach (var world in worlds)
                world.Update(dt);

            UpdateSystems();

            AddPendingEntities();

            DeinitializePendingWorlds();
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Method will request to add given entity to this world.
        /// Entity will not be added immediately but at the end of each world update.
        /// An exception will be thrown if given entity already exists in world
        /// </summary>
        /// <param name="entity">Entity to be added to this world</param>
        internal void RequestAddEntity(IEntity entity, IWorld world)
        {
            if (entity.WorldId != WecsConsts.NO_WORLD_ID)
                throw new InvalidOperationException("Entity can't exist in more than one world.");

            if (!entitiesToAdd.TryGetValue(world, out HashSet<IEntity> entities))
            {
                entities = new HashSet<IEntity>();
                entitiesToAdd.Add(world, entities);
            }

            entities.Add(entity);
        }

        /// <summary>
        /// Method will request to remove given entity from this world.
        /// Entity will not be removed immediately but at the end of each world update.
        /// An exception will be thrown if given entity does not exist in this world.
        /// </summary>
        /// <param name="entity">Entity to be removed from this world</param>
        internal void RequestRemoveEntity(IEntity entity, IWorld world)
        {
            if (entity.WorldId != world.Id)
                throw new InvalidOperationException("Entity doesn't exist in this world");

            if (!entitiesToRemove.TryGetValue(world, out HashSet<IEntity> entities))
            {
                entities = new HashSet<IEntity>();
                entitiesToRemove.Add(world, entities);
            }

            entities.Add(entity);
        }

        internal void RequestUpdateEntity(IEntity entity)
        {
            entitiesToUpdate.Add(entity);
        }

        #endregion Internal Methods

        #region Private Methods

        private void AddPendingEntities()
        {
            foreach (var keyValuePair in entitiesToAdd)
                AddPendingEntities(keyValuePair.Key, keyValuePair.Value);

            entitiesToAdd.Clear();
        }

        private void AddPendingEntities(IWorld world, HashSet<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                world.AddEntity(entity);
                OnEntityAdded(entity, world.Id);
            }
        }

        private void CheckUpdateAddToSystems(IWorld world, IEntity entity)
        {
            foreach (var system in world.Systems)
            {
                var areMatching = entityToSystemMatcher.AreMatch(system, entity);

                if (system.ContainsEntity(entity))
                {
                    if (!areMatching)
                    {
                        system.RemoveEntity(entity);
                        continue;
                    }
                }
                else
                {
                    if (areMatching)
                    {
                        system.AddEntity(entity);
                        continue;
                    }
                }
            }
        }

        private void DeinitializePendingWorlds()
        {
            if (toDeinitialize.Any())
            {
                foreach (var world in toDeinitialize)
                    DeinitializeWorld(world);

                toDeinitialize.Clear();
            }
        }

        private void DeinitializeWorld(IWorld world)
        {
            worlds.Remove((World)world);

            eventsMan.Raise(this, new WorldDeinitializedEventArgs(world.Id));
        }

        private void EntityMan_ComponentAdded(IEntity entity, Type componentType)
        {
            RequestUpdateEntity(entity);
        }

        private void EntityMan_ComponentRemoved(IEntity entity, Type componentType)
        {
            RequestUpdateEntity(entity);
        }

        private void EntityMan_EnterWorldRequested(IEntity entity, int worldId)
        {
            var world = (World)GetById(worldId);
            RequestAddEntity(entity, world);
        }

        private void EntityMan_LeaveWorldRequested(IEntity entity)
        {
            var world = (World)GetById(entity.WorldId);
            RequestRemoveEntity(entity, world);
        }

        /// <summary>
        /// Initialize or remove any pending worlds
        /// </summary>
        private void InitializePendingWorlds()
        {
            if (toInitialize.Any())
            {
                foreach (var world in toInitialize)
                    InitializeWorld(world);

                toInitialize.Clear();
            }
        }

        private void InitializeWorld(IWorld world)
        {
            worlds.Add((World)world);

            eventsMan.Raise(this, new WorldInitializedEventArgs(world.Id));
        }

        private void OnEntityAdded(IEntity entity, int worldId)
        {
            eventsMan.Raise(entity, new EntityEnteredEventArgs(entity.Id, worldId));
        }

        private void OnEntityRemoved(IEntity entity, int worldId)
        {
            eventsMan.Raise(entity, new EntityLeftEventArgs(entity.Id, worldId));
        }

        private void RemovePendingEntities()
        {
            foreach (var keyValuePair in entitiesToRemove)
                RemovePendingEntities(keyValuePair.Key, keyValuePair.Value);

            entitiesToRemove.Clear();
        }

        private void RemovePendingEntities(IWorld world, HashSet<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                world.RemoveEntity(entity);
                OnEntityRemoved(entity, world.Id);
            }
        }

        private void UpdateSystems()
        {
            foreach (var entity in entitiesToUpdate)
            {
                if (entity.WorldId == WecsConsts.NO_WORLD_ID)
                    continue;

                var world = (World)GetById(entity.WorldId);

                CheckUpdateAddToSystems(world, entity);
            }

            entitiesToUpdate.Clear();
        }

        #endregion Private Methods
    }
}