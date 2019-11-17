using OpenBreed.Core.Collections;
using OpenBreed.Core.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Core.Managers
{
    /// <summary>
    /// Manager responsible for creating, removing and updating core worlds
    /// </summary>
    public class WorldMan
    {
        #region Private Fields

        private readonly List<World> toAdd = new List<World>();
        private readonly List<World> toRemove = new List<World>();
        private readonly IdMap<World> worlds = new IdMap<World>();

        #endregion Private Fields

        #region Public Constructors

        public WorldMan(ICore core)
        {
            Core = core;
            Items = worlds.Items;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// ReadOnly collection of all loaded worlds
        /// </summary>
        public ReadOnlyCollection<World> Items { get; }

        /// <summary>
        /// Reference to Core object
        /// </summary>
        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns world with given ID. Will throw exception when such world has not been found.
        /// </summary>
        /// <param name="id">If of world to be returned</param>
        /// <returns>World reference</returns>
        public World GetById(int id)
        {
            var world = worlds[id];

            if (worlds.TryGetValue(id, out world))
                return world;
            else
                throw new InvalidOperationException($"World with ID '{id}' not found.");
        }

        /// <summary>
        /// Creates world, It will be initialized and added to Core at nearest manager update
        /// </summary>
        /// <returns>New World</returns>
        public World Create()
        {
            var newWorld = new World(Core);
            toAdd.Add(newWorld);
            return newWorld;
        }

        /// <summary>
        /// Marks given world to be removed from Core, it will be removed at nearest Manager Update
        /// </summary>
        /// <param name="world">World to be removed</param>
        public void Remove(World world)
        {
            if (toRemove.Contains(world))
            {
                Core.Logging.Warning($"World '{world}' already pending removing.");
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
                Items[i].Update(dt);
        }

        /// <summary>
        /// Add or remove any pending worlds
        /// </summary>
        public void Cleanup()
        {
            if (toRemove.Any())
            {
                //Process entities to remove
                for (int i = 0; i < toRemove.Count; i++)
                {
                    toRemove[i].Deinitialize();
                    worlds.RemoveById(toRemove[i].Id);
                }

                toRemove.Clear();
            }

            if (toAdd.Any())
            {
                //Process entities to add
                for (int i = 0; i < toAdd.Count; i++)
                {
                    toAdd[i].Id = worlds.Add(toAdd[i]);
                    toAdd[i].Initialize();
                }

                toAdd.Clear();
            }

            //Do cleanups on remaining worlds
            for (int i = 0; i < worlds.Items.Count; i++)
                worlds.Items[i].Cleanup();
        }

        #endregion Public Methods
    }
}