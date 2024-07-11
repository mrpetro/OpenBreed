using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Tools.Collections;
using OpenBreed.Core.Managers;
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

        private readonly Dictionary<int, HashSet<IEntity>> entitiesToAdd = new Dictionary<int, HashSet<IEntity>>();
        private readonly Dictionary<int, HashSet<IEntity>> entitiesToRemove = new Dictionary<int, HashSet<IEntity>>();
        private readonly HashSet<IEntity> entitiesToUpdate = new HashSet<IEntity>();
        private readonly IEntityToSystemMatcher entityToSystemMatcher;
        private readonly IEventsMan eventsMan;
        private readonly IdMap<World> IdsToWorldsLookup = new IdMap<World>();
        private readonly ILogger logger;
        private readonly Dictionary<string, int> namesToIdsLookup = new Dictionary<string, int>();
        private readonly ISystemFactory systemFactory;
        private readonly HashSet<World> toDeinitialize = new HashSet<World>();
        private readonly HashSet<World> toInitialize = new HashSet<World>();
        private readonly List<World> worlds = new List<World>();

        #endregion Private Fields

        #region Public Constructors

        public WorldMan(
            IEventsMan eventsMan,
            ISystemFactory systemFactory,
            IEntityToSystemMatcher entityToSystemMatcher,
            ILogger logger)
        {
            this.eventsMan = eventsMan;
            this.systemFactory = systemFactory;
            this.entityToSystemMatcher = entityToSystemMatcher;
            this.logger = logger;
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
            if (name is null)
                return null;

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
                logger.LogWarning("World '{0}' already pending removing.", world);
                return;
            }

            toDeinitialize.Add((World)world);
        }

        public void RequestAddEntity(IEntity entity, int worldId)
        {
            //If entity is already is same world then do nothing
            if (entity.WorldId == worldId)
                return;

            //If entity exists in another world then request remove it
            if (entity.WorldId != WecsConsts.NO_WORLD_ID)
                RequestRemoveEntity(entity);

            if (!entitiesToAdd.TryGetValue(worldId, out HashSet<IEntity> entities))
            {
                entities = new HashSet<IEntity>();
                entitiesToAdd.Add(worldId, entities);
            }

            entities.Add(entity);
        }

        public void RequestRemoveEntity(IEntity entity)
        {
            //If entity is already in limbo then do nothing
            if (entity.WorldId == WecsConsts.NO_WORLD_ID)
                return;

            if (!entitiesToRemove.TryGetValue(entity.WorldId, out HashSet<IEntity> entities))
            {
                entities = new HashSet<IEntity>();
                entitiesToRemove.Add(entity.WorldId, entities);
            }

            if(entities.Add(entity))
                eventsMan.Raise(null, new EntityLeavingEvent(entity.Id, entity.WorldId));
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

        private void AddPendingEntities(int worldId, HashSet<IEntity> entities)
        {
            var world = GetById(worldId);

            foreach (var entity in entities)
            {
                world.AddEntity(entity);
                OnEntityAdded(entity, worldId);
            }
        }

        private void CheckUpdateAddToSystems(IWorld world, IEntity entity)
        {
            foreach (var system in world.Systems.OfType<IMatchingSystem>())
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
            eventsMan.Raise(entity, new EntityEnteredEvent(entity.Id, worldId));
        }

        private void OnEntityRemoved(IEntity entity, int worldId)
        {
            eventsMan.Raise(entity, new EntityLeftEvent(entity.Id, worldId));
        }

        private void RemovePendingEntities()
        {
            foreach (var keyValuePair in entitiesToRemove)
                RemovePendingEntities(keyValuePair.Key, keyValuePair.Value);

            entitiesToRemove.Clear();
        }

        private void RemovePendingEntities(int worldId, HashSet<IEntity> entities)
        {
            var world = GetById(worldId);

            foreach (var entity in entities)
            {
                world.RemoveEntity(entity);
                OnEntityRemoved(entity, worldId);
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