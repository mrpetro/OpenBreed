using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.Common.Builders;
using OpenBreed.Rendering.Common.Helpers;
using System;
using System.Collections.Generic;

namespace OpenBreed.Rendering.Common.Managers
{
    public class StampMan : IStampMan
    {
        #region Protected Fields

        protected readonly ITileStamp missingTileStamp;

        #endregion Protected Fields

        #region Private Fields

        private readonly List<TileStamp> items = new List<TileStamp>();
        private readonly Dictionary<string, TileStamp> names = new Dictionary<string, TileStamp>();
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public StampMan(ILogger logger)
        {
            this.logger = logger;

            missingTileStamp = Create()
                .SetName("Missing")
                .SetSize(1, 1)
                .AddTile(0, 0, 0, 0)
                .Build();
        }

        #endregion Public Constructors

        #region Public Methods

        public IStampBuilder Create()
        {
            return new StampBuilder(this);
        }

        public ITileStamp GetById(int id)
        {
            return items[id];
        }

        public bool TryGetByName(string name, out ITileStamp tileStamp)
        {
            if(names.TryGetValue(name, out TileStamp found))
            {
                tileStamp = found;
                return true;
            }

            tileStamp = null;
            return false;
        }

        public ITileStamp GetByName(string name)
        {
            if(TryGetByName(name, out ITileStamp result))
            {
                return result;
            }

            logger.LogError("Unable to find Tile stamp with name '{0}'.", name);

            return missingTileStamp;
        }

        public bool Contains(string name) => names.ContainsKey(name);

        public IEnumerable<ITileStamp> FindAll(Predicate<ITileStamp> predicate)
        {
            return items.FindAll(predicate);
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Generate ID for new stamp
        /// </summary>
        /// <returns>ID for new stamp</returns>
        internal int GenerateNewId()
        {
            return items.Count;
        }

        internal void Register(string name, TileStamp newStamp)
        {
            items.Add(newStamp);
            names.Add(name, newStamp);
        }

        #endregion Internal Methods
    }
}