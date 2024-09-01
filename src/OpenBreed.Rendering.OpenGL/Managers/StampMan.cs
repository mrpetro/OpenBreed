using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Helpers;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Managers
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

            this.missingTileStamp = Create()
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