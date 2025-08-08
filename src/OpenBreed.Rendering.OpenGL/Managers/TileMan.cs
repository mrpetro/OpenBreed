using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class TileMan : ITileMan
    {
        #region Private Fields

        private readonly List<TileAtlas> items = new List<TileAtlas>();
        private readonly Dictionary<string, TileAtlas> names = new Dictionary<string, TileAtlas>();
        private readonly ITextureMan textureMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Public Constructors

        public TileMan(ITextureMan textureMan, ILogger logger)
        {
            this.textureMan = textureMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public bool Contains(string atlasName)
        {
            return names.ContainsKey(atlasName);
        }

        public ITileAtlasBuilder CreateAtlas()
        {
            return new TileAtlasBuilder(this, textureMan);
        }

        public ITileAtlas GetById(int id)
        {
            return items[id];
        }

        public ITileAtlas GetByName(string alias)
        {
            TileAtlas result = null;
            names.TryGetValue(alias, out result);
            return result;
        }

        public void UnloadAll(IRenderContext renderContext)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

        #region Internal Methods

        internal TileAtlas InternalGetById(int atlasId)
        {
            return items[atlasId];
        }

        internal int Register(string name, TileAtlas tileAtlas)
        {
            items.Add(tileAtlas);
            names.Add(name, tileAtlas);

            logger.LogTrace("Tile atlas '{0}' created with ID {1}.", name, items.Count - 1);

            return items.Count - 1;
        }

        #endregion Internal Methods
    }
}