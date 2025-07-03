using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK.Mathematics;
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

        #region Internal Fields

        /// <summary>
        /// OpenGL vertex buffer object ID
        /// </summary>
        internal int Vbo = -1;

        #endregion Internal Fields
    }

    internal class TileAtlas : ITileAtlas
    {
        #region Internal Fields

        internal readonly List<TileData> data;

        #endregion Internal Fields

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

        #region Public Methods

        public void Load(IRenderContext renderContext)
        {
            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                item.Vbo = CreateTileVertices(renderContext, item);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private int CreateTileVertices(IRenderContext renderContext, TileData tileData)
        {
            var vertices = CreateVertices(tileData);

            var vertexArrayBuilder = renderContext.Primitives.CreatePosTexCoordArray();
            vertexArrayBuilder.AddVertex(vertices[0].position, vertices[0].texCoord);
            vertexArrayBuilder.AddVertex(vertices[1].position, vertices[1].texCoord);
            vertexArrayBuilder.AddVertex(vertices[2].position, vertices[2].texCoord);
            vertexArrayBuilder.AddVertex(vertices[3].position, vertices[3].texCoord);

            vertexArrayBuilder.AddTriangleIndices(0, 1, 3);
            vertexArrayBuilder.AddTriangleIndices(1, 2, 3);

            return vertexArrayBuilder.CreateTexturedVao();
        }

        private Vertex[] CreateVertices(TileData data)
        {
            var uvCoord = Vector2.Divide(new Vector2(data.U, data.V), new Vector2(Texture.Width, Texture.Height));
            var uvSize = Vector2.Divide(new Vector2(TileSize, TileSize), new Vector2(Texture.Width, Texture.Height));

            var uvLD = new Vector2(uvCoord.X, uvCoord.Y);
            var uvRT = Vector2.Add(uvLD, uvSize);

            Vertex[] vertices = {
                                new Vertex(new Vector2(0,   0),              new Vector2(uvLD.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(TileSize,  0),        new Vector2(uvRT.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(TileSize,  TileSize), new Vector2(uvRT.X, uvLD.Y), Color4.White),
                                new Vertex(new Vector2(0,   TileSize),       new Vector2(uvLD.X, uvLD.Y), Color4.White),
                            };

            return vertices;
        }

        #endregion Private Methods
    }
}