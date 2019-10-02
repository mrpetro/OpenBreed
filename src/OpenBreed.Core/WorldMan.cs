using OpenBreed.Core.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OpenBreed.Core
{
    public class WorldMan
    {
        #region Private Fields

        private readonly List<World> items = new List<World>();
        private readonly List<World> toAdd = new List<World>();
        private readonly List<World> toRemove = new List<World>();

        #endregion Private Fields

        #region Public Constructors

        public WorldMan(ICore core)
        {
            Core = core;

            Items = new ReadOnlyCollection<World>(items);
        }

        #endregion Public Constructors

        #region Public Properties

        public ReadOnlyCollection<World> Items { get; }

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public void Remove(World world)
        {
            if (toRemove.Contains(world))
                throw new InvalidOperationException("World already pending removing.");

            if (!items.Contains(world))
                throw new InvalidOperationException("World is not added.");

            items.Remove(world);
        }

        public void Add(World world)
        {
            if (toAdd.Contains(world))
                throw new InvalidOperationException("World already pending adding.");

            if (items.Contains(world))
                throw new InvalidOperationException("World already added.");

            toAdd.Add(world);
        }

        /// <summary>
        /// Updates the world
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
                    items.Remove(toRemove[i]);
                }

                toRemove.Clear();
            }

            if (toAdd.Any())
            {
                //Process entities to add
                for (int i = 0; i < toAdd.Count; i++)
                {
                    items.Add(toAdd[i]);
                    toAdd[i].Initialize();
                }

                toAdd.Clear();
            }

            //Do cleanups on remaining worlds
            for (int i = 0; i < items.Count; i++)
                items[i].Cleanup();
        }

        #endregion Public Methods
    }
}