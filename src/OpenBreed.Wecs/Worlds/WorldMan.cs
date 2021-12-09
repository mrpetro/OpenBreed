using OpenBreed.Common.Logging;
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
    public class WorldMan : IWorldMan
    {
        #region Private Fields

        private readonly HashSet<World> toAdd = new HashSet<World>();
        private readonly HashSet<World> toRemove = new HashSet<World>();
        private readonly IdMap<World> worlds = new IdMap<World>();
        private readonly Dictionary<string, int> namesToIds = new Dictionary<string, int>();
        private readonly IEntityMan entityMan;
        private readonly IEventsMan eventsMan;
        private readonly IScriptMan scriptMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal WorldMan(IEntityMan entityMan, IEventsMan eventsMan, IScriptMan scriptMan, ILogger logger)
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

        #endregion Internal Constructors

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
            if (worlds.TryGetValue(id, out World world))
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
            if (!namesToIds.TryGetValue(name, out worldId))
                return null;

            return worlds[worldId];
        }

        public void RaiseEvent<T>(T eventArgs) where T : EventArgs
        {
            eventsMan.Raise(this, eventArgs);
        }

        /// <summary>
        /// Marks given world to be removed from Core, it will be removed at nearest Manager Update
        /// </summary>
        /// <param name="world">World to be removed</param>
        public void Remove(World world)
        {
            if (toRemove.Contains(world))
            {
                logger.Warning($"World '{world}' already pending removing.");
                return;
            }

            toRemove.Add(world);
        }

        /// <summary>
        /// Updates all worlds
        /// </summary>
        /// <param name="dt">Delta time</param>
        public void Update(float dt)
        {
            AddPendingWorlds();

            foreach (var world in worlds.Items)
                world.Update(dt);

            RemovePendingWorlds();
        }

        public void RegisterWorld(World newWorld)
        {
            newWorld.Id = worlds.Add(newWorld);
            namesToIds.Add(newWorld.Name, newWorld.Id);

            newWorld.InitializeSystems();
            toAdd.Add(newWorld);
        }

        #endregion Public Methods

        #region Private Methods

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

        private void EntityMan_ComponentRemoved(Entity entity, Type componentType)
        {
            if (entity.WorldId == World.NO_WORLD)
                return;

            var world = GetById(entity.WorldId);

            world.CheckRemoveFromSystems(entity, componentType);
        }

        private void EntityMan_ComponentAdded(Entity entity, Type componentType)
        {
            if (entity.WorldId == World.NO_WORLD)
                return;

            var world = GetById(entity.WorldId);

            world.CheckAddToSystems(entity, componentType);
        }

        private void RemoveWorld(World world)
        {
            RaiseEvent(new WorldDeinitializedEventArgs(world.Id));
            worlds.RemoveById(world.Id);
        }

        private void RemovePendingWorlds()
        {
            if (toRemove.Any())
            {
                foreach (var world in toRemove)
                    RemoveWorld(world);

                toRemove.Clear();
            }
        }

        /// <summary>
        /// Initialize or remove any pending worlds
        /// </summary>
        private void AddPendingWorlds()
        {
            if (toAdd.Any())
            {
                foreach (var world in toAdd)
                    AddWorld(world);

                toAdd.Clear();
            }
        }

        private void AddWorld(World world)
        {
            RaiseEvent(new WorldInitializedEventArgs(world.Id));
            scriptMan.TryInvokeFunction("WorldLoaded", world.Id);
        }

        #endregion Private Methods
    }
}