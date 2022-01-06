using OpenBreed.Common.Logging;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class TileMan : ITileMan
    {
        #region Private Fields

        private static uint[] indicesArray = {
                                            0,1,2,
                                            0,2,3
                                       };

        private readonly int ibo;
        private readonly List<TileAtlas> items = new List<TileAtlas>();
        private readonly Dictionary<string, TileAtlas> names = new Dictionary<string, TileAtlas>();
        private readonly ITextureMan textureMan;
        private readonly ILogger logger;

        #endregion Private Fields

        #region Internal Constructors

        public TileMan(ITextureMan textureMan, ILogger logger)
        {
            this.textureMan = textureMan;
            this.logger = logger;

            //RenderTools.CreateIndicesArray(indicesArray, out ibo);
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal RenderingMan Module { get; }

        #endregion Internal Properties

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

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        public void Render(int atlasId, int imageId)
        {
            var atlas = items[atlasId];

            GL.BindTexture(TextureTarget.Texture2D, atlas.Texture.InternalId);
            RenderTools.Draw(atlas.data[imageId].Vbo, ibo, 6);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        #endregion Public Methods

        #region Internal Methods

        internal int Register(string name, TileAtlas tileAtlas)
        {
            items.Add(tileAtlas);
            names.Add(name, tileAtlas);

            logger.Verbose($"Tile atlas '{name}' created with ID {items.Count - 1}.");

            return items.Count - 1;
        }

        internal int CreateTileVertices(TileData spriteData, int tileSize, int width, int height)
        {
            var vertices = CreateVertices(spriteData, tileSize, width, height);
            int vbo;
            RenderTools.CreateVertexArray(vertices, out vbo);
            return vbo;
        }

        #endregion Internal Methods

        #region Private Methods

        private Vertex[] CreateVertices(TileData data, int tileSize, int textureWidth, int textureHeight)
        {
            var uvCoord = Vector2.Divide(new Vector2(data.U, data.V), new Vector2(textureWidth, textureHeight));
            var uvSize = Vector2.Divide(new Vector2(tileSize, tileSize), new Vector2(textureWidth, textureHeight));

            var uvLD = new Vector2(uvCoord.X, uvCoord.Y);
            var uvRT = Vector2.Add(uvLD, uvSize);

            Vertex[] vertices = {
                                new Vertex(new Vector2(0,   0),              new Vector2(uvLD.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(tileSize,  0),        new Vector2(uvRT.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(tileSize,  tileSize), new Vector2(uvRT.X, uvLD.Y), Color4.White),
                                new Vertex(new Vector2(0,   tileSize),       new Vector2(uvLD.X, uvLD.Y), Color4.White),
                            };

            return vertices;
        }

        #endregion Private Methods
    }
}