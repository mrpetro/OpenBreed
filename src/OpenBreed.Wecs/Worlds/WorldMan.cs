﻿using OpenBreed.Common.Logging;
using OpenBreed.Common.Tools.Collections;
using OpenBreed.Core;
using OpenBreed.Core.Managers;
using OpenBreed.Scripting.Interface;
using OpenBreed.Wecs.Commands;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Events;
using OpenBreed.Wecs.Systems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Wecs.Worlds
{
    /// <summary>
    /// Manager responsible for creating, removing and updating core worlds
    /// </summary>
    public class WorldMan : IWorldMan
    {
        #region Private Fields

        private readonly List<World> toInitialize = new List<World>();
        private readonly List<World> toRemove = new List<World>();
        private readonly IdMap<World> worlds = new IdMap<World>();
        private readonly Dictionary<string, int> namesToIds = new Dictionary<string, int>();
        private readonly IEntityMan entityMan;
        private readonly ICommandsMan commandsMan;
        private readonly IEventsMan eventsMan;
        private readonly IScriptMan scriptMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal WorldMan(IEntityMan entityMan, ICommandsMan commandsMan, IEventsMan eventsMan, IScriptMan scriptMan, ILogger logger)
        {
            this.entityMan = entityMan;
            this.commandsMan = commandsMan;
            this.eventsMan = eventsMan;
            this.scriptMan = scriptMan;
            this.logger = logger;
            Items = worlds.Items;

            commandsMan.Register<PauseWorldCommand>(HandlePauseWorld);
            commandsMan.Register<RemoveEntityCommand>(HandleRemoveEntity);
            commandsMan.Register<AddEntityCommand>(HandleAddEntity);
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// ReadOnly collection of all loaded worlds
        /// </summary>
        public ReadOnlyCollection<World> Items { get; }

        #endregion Public Properties

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
            for (int i = 0; i < Items.Count; i++)
                UpdateWorld(Items[i], dt);
        }

        private void DeinitializeWorld(World world)
        {
            RaiseEvent(new WorldDeinitializedEventArgs(world.Id));
        }

        /// <summary>
        /// Initialize or remove any pending worlds
        /// </summary>
        public void Cleanup()
        {
            if (toRemove.Any())
            {
                //Process entities to remove
                for (int i = 0; i < toRemove.Count; i++)
                {
                    DeinitializeWorld(toRemove[i]);
                    worlds.RemoveById(toRemove[i].Id);
                }

                toRemove.Clear();
            }

            if (toInitialize.Any())
            {
                //Process entities for initialization
                for (int i = 0; i < toInitialize.Count; i++)
                    InitializeWorld(toInitialize[i]);

                toInitialize.Clear();
            }

            //Do cleanups on remaining worlds
            for (int i = 0; i < worlds.Items.Count; i++)
                worlds.Items[i].Cleanup(this);
        }

        private void InitializeWorld(World world)
        {
            world.Initialize(this);
            scriptMan.TryInvokeFunction("WorldLoaded", world.Id);
        }

        public void RegisterWorld(World newWorld)
        {
            newWorld.InitializeSystems();
            newWorld.Id = worlds.Add(newWorld);
            namesToIds.Add(newWorld.Name, newWorld.Id);
            toInitialize.Add(newWorld);
        }

        #endregion Public Methods

        #region Private Methods

        private void UpdateWorld(World world, float dt)
        {
            if (world.Paused)
            {
                foreach (var item in world.Systems.OfType<IUpdatableSystem>())
                {
                    commandsMan.ExecuteEnqueued();
                    item.UpdatePauseImmuneOnly(dt * world.TimeMultiplier);
                }
                //systems.OfType<IUpdatableSystem>().ForEach(item => item.UpdatePauseImmuneOnly(dt * TimeMultiplier));
            }
            else
            {
                foreach (var item in world.Systems.OfType<IUpdatableSystem>())
                {
                    commandsMan.ExecuteEnqueued();
                    item.Update(dt * world.TimeMultiplier);
                }
                //systems.OfType<IUpdatableSystem>().ForEach(item => item.Update(dt * TimeMultiplier));
            }
        }

        private bool HandleRemoveEntity(RemoveEntityCommand cmd)
        {
            var world = GetById(cmd.WorldId);
            var entity = entityMan.GetById(cmd.EntityId);
            world.RemoveEntity(entity);
            return true;
        }

        private bool HandleAddEntity(AddEntityCommand cmd)
        {
            var world = GetById(cmd.WorldId);
            var entity = entityMan.GetById(cmd.EntityId);
            world.AddEntity(entity);
            return true;
        }

        private bool HandlePauseWorld(PauseWorldCommand cmd)
        {
            var world = GetById(cmd.WorldId);

            if (cmd.Pause == world.Paused)
                return true;

            world.Paused = cmd.Pause;

            if (world.Paused)
                RaiseEvent(new WorldPausedEventArgs(world.Id));
            else
                RaiseEvent(new WorldUnpausedEventArgs(world.Id));

            return true;
        }

        #endregion Private Methods
    }
}