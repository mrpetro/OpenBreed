using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Tools.Collections;
using OpenBreed.Core.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
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

        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly IdMap<World> IdsToWorldsLookup = new IdMap<World>();
        private readonly ILogger logger;
        private readonly Dictionary<string, int> namesToIdsLookup = new Dictionary<string, int>();
        private readonly IScriptMan scriptMan;
        private readonly HashSet<World> toDeinitialize = new HashSet<World>();
        private readonly HashSet<World> toInitialize = new HashSet<World>();
        private readonly List<World> worlds = new List<World>();

        #endregion Private Fields

        #region Public Constructors

        public WorldMan(IEntityMan entityMan, IEventsMan eventsMan, IScriptMan scriptMan, ILogger logger)
        {
            this.entityMan = entityMan;
            this.eventsMan = eventsMan;
            this.scriptMan = scriptMan;
            this.logger = logger;

            entityMan.ComponentAdded += EntityMan_ComponentAdded;
            entityMan.ComponentRemoved += EntityMan_ComponentRemoved;
            entityMan.EnterWorldRequested += EntityMan_EnterWorldRequested;
            entityMan.LeaveWorldRequested += EntityMan_LeaveWorldRequested;
        }

        #endregion Public Constructors

        #region Public Events

        public event EntitiyEntered EntitiyEntered;

        public event EntitiyLeft EntitiyLeft;

        #endregion Public Events

        #region Public Methods

        public WorldBuilder Create()
        {
            return new WorldBuilder(this, logger);
        }

        /// <summary>
        /// Returns world with given ID. Will throw exception when such world has not been found.
        /// </summary>
        /// <param name="id">If of world to be returned</param>
        /// <returns>World reference</returns>
        public World GetById(int id)
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
        public World GetByName(string name)
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

            newWorld.InitializeSystems();
            toInitialize.Add(newWorld);
        }

        /// <summary>
        /// Marks given world to be removed from Core, it will be removed at nearest Manager Update
        /// </summary>
        /// <param name="world">World to be removed</param>
        public void Remove(World world)
        {
            if (toDeinitialize.Contains(world))
            {
                logger.Warning($"World '{world}' already pending removing.");
                return;
            }

            toDeinitialize.Add(world);
        }

        /// <summary>
        /// Updates all worlds
        /// </summary>
        /// <param name="dt">Delta time</param>
        public void Update(float dt)
        {
            InitializePendingWorlds();

            foreach (var world in worlds)
                world.Update(dt);

            DeinitializePendingWorlds();
        }

        #endregion Public Methods

        //internal void OnWorldPaused(int worldId)
        //{
        //    RaiseEvent(new WorldPausedEventArgs(worldId));
        //}

        //internal void OnWorldUnpaused(int worldId)
        //{
        //    RaiseEvent(new WorldUnpausedEventArgs(worldId));
        //}

        #region Internal Methods

        internal void OnEntityAdded(Entity entity, int worldId)
        {
            eventsMan.Raise(entity, new EntityEnteredEventArgs(entity.Id, worldId));
        }

        internal void OnEntityRemoved(Entity entity, int worldId)
        {
            eventsMan.Raise(entity, new EntityLeftEventArgs(entity.Id, worldId));
        }

        #endregion Internal Methods

        #region Private Methods

        private void DeinitializePendingWorlds()
        {
            if (toDeinitialize.Any())
            {
                foreach (var world in toDeinitialize)
                    DeinitializeWorld(world);

                toDeinitialize.Clear();
            }
        }

        private void DeinitializeWorld(World world)
        {
            worlds.Remove(world);

            eventsMan.Raise(this, new WorldDeinitializedEventArgs(world.Id));
        }

        private void EntityMan_ComponentAdded(Entity entity, Type componentType)
        {
            if (entity.WorldId == World.NO_WORLD)
                return;

            var world = GetById(entity.WorldId);

            world.CheckAddToSystems(entity, componentType);
        }

        private void EntityMan_ComponentRemoved(Entity entity, Type componentType)
        {
            if (entity.WorldId == World.NO_WORLD)
                return;

            var world = GetById(entity.WorldId);

            world.CheckRemoveFromSystems(entity, componentType);
        }

        private void EntityMan_EnterWorldRequested(Entity entity, int worldId)
        {
            var world = GetById(worldId);
            world.RequestAddEntity(entity);
        }

        private void EntityMan_LeaveWorldRequested(Entity entity)
        {
            var world = GetById(entity.WorldId);
            world.RequestRemoveEntity(entity);
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

        private void InitializeWorld(World world)
        {
            worlds.Add(world);

            eventsMan.Raise(this, new WorldInitializedEventArgs(world.Id));
        }

        #endregion Private Methods
    }
}