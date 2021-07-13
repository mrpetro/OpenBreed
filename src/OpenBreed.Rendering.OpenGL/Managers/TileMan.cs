using System;
using System.Collections.Generic;
using System.Drawing;
using OpenBreed.Common.Logging;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class TileMan : ITileMan
    {
        #region Private Fields

        private readonly List<ITileAtlas> items = new List<ITileAtlas>();
        private readonly Dictionary<string, ITileAtlas> aliases = new Dictionary<string, ITileAtlas>();
        private readonly ITextureMan textureMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        internal TileMan(ITextureMan textureMan, ILogger logger)
        {
            this.textureMan = textureMan;
            this.logger = logger;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal RenderingMan Module { get; }

        #endregion Internal Properties

        #region Public Methods

        public ITileAtlas Create(string alias, int textureId, int tileSize, Point[] points)
        {
            ITileAtlas result;
            if (aliases.TryGetValue(alias, out result))
                return result;

            var texture = textureMan.GetById(textureId);
            result = new TileAtlas(items.Count, texture, tileSize, points);
            items.Add(result);
            aliases.Add(alias, result);

            logger.Verbose($"Tile Atlas {result.Id} ({alias}) created.");

            return result;
        }

        public ITileAtlas Create(string alias, int textureId, int tileSize, int tileColumns, int tileRows)
        {
            ITileAtlas result;
            if (aliases.TryGetValue(alias, out result))
                return result;

            var texture = textureMan.GetById(textureId);
            result = new TileAtlas(items.Count, texture, tileSize, tileColumns, tileRows);
            items.Add(result);
            aliases.Add(alias, result);

            logger.Verbose($"Tile Atlas {result.Id} ({alias}) created.");

            return result;
        }

        public ITileAtlas GetById(int id)
        {
            return items[id];
        }

        public ITileAtlas GetByAlias(string alias)
        {
            ITileAtlas result = null;
            aliases.TryGetValue(alias, out result);
            return result;
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}