using Microsoft.Extensions.Logging;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class TileData
    {
        #region Public Fields

        /// <summary>
        /// U coordinate of tile on texture
        /// </summary>
        public int U;

        /// <summary>
        /// V coordinate of tile on texture
        /// </summary>
        public int V;

        #endregion Public Fields
    }

    internal class TileAtlas : ITileAtlas
    {
        #region Internal Fields

        internal readonly List<TileData> data;

        #endregion Internal Fields

        #region Private Fields

        private readonly RenderContextItem<List<int>> contextData = new RenderContextItem<List<int>>();

        #endregion Private Fields

        #region Public Constructors

        public TileAtlas(TileAtlasBuilder builder)
        {
            Texture = builder.Texture;
            TileSize = builder.TileSize;
            data = builder.GetTileData();
            Id = builder.Register(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public ITexture Texture { get; }
        public float TileSize { get; }
        public int Id { get; }

        #endregion Public Properties

        #region Internal Methods

        internal int GetTileVao(IRenderContext context, int tileId)
        {
            if (tileId == -1)
            {
                return -1;
            }

            var atlasVaos = contextData.GetOrAdd(context, Load);
            return atlasVaos[tileId];
        }

        #endregion Internal Methods

        #region Private Methods

        private List<int> Load(IRenderContext renderContext)
        {
            var vboList = new List<int>();

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                var itemVbo = CreateTileVertices(renderContext, item);

                vboList.Add(itemVbo);
            }

            //logger.LogTrace("Tile atlas '{0}' loaded into render context..", item.Id);

            return vboList;
        }

        private int CreateTileVertices(IRenderContext renderContext, TileData tileData)
        {
            var vertices = UvBox.CreateVertices(
            tileData.U,
            tileData.V,
            (int)TileSize,
            (int)TileSize,
            Texture.Width,
            Texture.Height);

            var vertexArrayBuilder = renderContext.Primitives.CreatePosTexCoordArray();
            vertexArrayBuilder.AddVertex(vertices[0].position, vertices[0].texCoord);
            vertexArrayBuilder.AddVertex(vertices[1].position, vertices[1].texCoord);
            vertexArrayBuilder.AddVertex(vertices[2].position, vertices[2].texCoord);
            vertexArrayBuilder.AddVertex(vertices[3].position, vertices[3].texCoord);

            vertexArrayBuilder.AddTriangleIndices(0, 1, 3);
            vertexArrayBuilder.AddTriangleIndices(1, 2, 3);

            return vertexArrayBuilder.CreateTexturedVao();
        }

        #endregion Private Methods
    }
}