using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using OpenBreed.Common.Logging;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Graphics;
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
        private readonly IPrimitiveRenderer primitiveRenderer;

        #endregion Private Fields

        #region Internal Constructors

        public TileMan(ITextureMan textureMan, ILogger logger, IPrimitiveRenderer primitiveRenderer)
        {
            this.textureMan = textureMan;
            this.logger = logger;
            this.primitiveRenderer = primitiveRenderer;
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
            var size = atlas.TileSize;
            var vao = atlas.data[imageId].Vbo;
            primitiveRenderer.DrawSprite(
                atlas.Texture,
                vao,
                new Vector3(0,0,0),
                Vector2.One,
                Color4.White);
        }

        #endregion Public Methods

        #region Internal Methods

        internal int Register(string name, TileAtlas tileAtlas)
        {
            items.Add(tileAtlas);
            names.Add(name, tileAtlas);

            logger.LogTrace("Tile atlas '{0}' created with ID {1}.", name, items.Count - 1);

            return items.Count - 1;
        }

        internal int CreateTileVertices(TileData tileData, int tileSize, int width, int height)
        {
            var vertices = CreateVertices(tileData, tileSize, width, height);

            var vertexArrayBuilder = primitiveRenderer.CreatePosTexCoordArray();
            vertexArrayBuilder.AddVertex(vertices[0].position, vertices[0].texCoord);
            vertexArrayBuilder.AddVertex(vertices[1].position, vertices[1].texCoord);
            vertexArrayBuilder.AddVertex(vertices[2].position, vertices[2].texCoord);
            vertexArrayBuilder.AddVertex(vertices[3].position, vertices[3].texCoord);

            vertexArrayBuilder.AddTriangleIndices(0, 1, 3);
            vertexArrayBuilder.AddTriangleIndices(1, 2, 3);

            return vertexArrayBuilder.CreateTexturedVao();
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