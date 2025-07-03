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

        private readonly Queue<TileAtlas> loadQueue = new Queue<TileAtlas>();
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

        public void LoadRefresh(IRenderContext renderContext)
        {
            while (loadQueue.Count > 0)
            {
                var item = loadQueue.Dequeue();
                item.Load(renderContext);

                logger.LogTrace("Tile atlas '{0}' loaded into render context..", item.Id);
            }
        }

        public void UnloadAll(IRenderContext renderContext)
        {
            throw new NotImplementedException();
        }

        public void Render(IRenderView view, int atlasId, int imageId)
        {
            var atlas = items[atlasId];
            var size = atlas.TileSize;
            var vao = atlas.data[imageId].Vbo;

            if (vao == -1)
            {
                return;
            }

            view.Context.Primitives.DrawSprite(
                view,
                atlas.Texture,
                vao,
                new Vector3(0, 0, 0),
                Vector2.One,
                Color4.White);
        }

        #endregion Public Methods

        #region Internal Methods

        internal int Register(string name, TileAtlas tileAtlas)
        {
            items.Add(tileAtlas);
            names.Add(name, tileAtlas);
            loadQueue.Enqueue(tileAtlas);

            logger.LogTrace("Tile atlas '{0}' created with ID {1}.", name, items.Count - 1);

            return items.Count - 1;
        }

        #endregion Internal Methods
    }
}