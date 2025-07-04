using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class SpriteData
    {
        #region Public Fields

        /// <summary>
        /// U coordinate of sprite on texture
        /// </summary>
        public int U;

        /// <summary>
        /// V coordinate of sprite on texture
        /// </summary>
        public int V;

        /// <summary>
        /// Width of sprite on texture
        /// </summary>
        public int Width;

        /// <summary>
        /// Height of sprite on texture
        /// </summary>
        public int Height;

        #endregion Public Fields

        #region Internal Fields

        /// <summary>
        /// OpenGL vertex buffer object ID
        /// </summary>
        internal int Vbo = -1;

        #endregion Internal Fields

        #region Public Properties

        /// <summary>
        /// Size of sprite
        /// </summary>
        public Vector2 Size => new Vector2(Width, Height);

        #endregion Public Properties
    }

    internal class SpriteAtlas : ISpriteAtlas
    {
        #region Internal Fields

        internal readonly List<SpriteData> data;

        #endregion Internal Fields

        #region Internal Constructors

        internal SpriteAtlas(SpriteAtlasBuilder builder)
        {
            Texture = builder.Texture;
            data = builder.GetSpriteData();
            Id = builder.Register(this);
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Id of this sprite atlas
        /// </summary>
        public int Id { get; }

        #endregion Public Properties

        #region Internal Properties

        internal ITexture Texture { get; }

        #endregion Internal Properties

        #region Public Methods

        public Vector2 GetSpriteSize(int spriteId)
        {
            return data[spriteId].Size;
        }

        public void Load(IRenderContext renderContext)
        {
            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                item.Vbo = CreateSpriteVertices(renderContext, item);
            }
        }

        public bool IsValid(int imageId)
        {
            if (imageId < 0)
                return false;

            if (imageId >= data.Count)
                return false;

            return true;
        }

        #endregion Public Methods

        #region Internal Methods

        private Vertex[] CreateVertices(SpriteData data)
        {
            var uvCoord = Vector2.Divide(new Vector2(data.U, data.V), new Vector2(Texture.Width, Texture.Height));
            var uvSize = Vector2.Divide(new Vector2(data.Width, data.Height), new Vector2(Texture.Width, Texture.Height));

            var uvLD = new Vector2(uvCoord.X, uvCoord.Y);
            var uvRT = Vector2.Add(uvLD, uvSize);

            Vertex[] vertices = {
                                new Vertex(new Vector2(0,   0),              new Vector2(uvLD.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(data.Width,  0),        new Vector2(uvRT.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(data.Width,  data.Height), new Vector2(uvRT.X, uvLD.Y), Color4.White),
                                new Vertex(new Vector2(0,   data.Height),       new Vector2(uvLD.X, uvLD.Y), Color4.White),
                            };

            return vertices;
        }

        internal int CreateSpriteVertices(IRenderContext renderContext, SpriteData spriteData)
        {
            var vertices = CreateVertices(spriteData);

            var vertexArrayBuilder = renderContext.Primitives.CreatePosTexCoordArray();
            vertexArrayBuilder.AddVertex(vertices[0].position, vertices[0].texCoord);
            vertexArrayBuilder.AddVertex(vertices[1].position, vertices[1].texCoord);
            vertexArrayBuilder.AddVertex(vertices[2].position, vertices[2].texCoord);
            vertexArrayBuilder.AddVertex(vertices[3].position, vertices[3].texCoord);

            vertexArrayBuilder.AddTriangleIndices(0, 1, 3);
            vertexArrayBuilder.AddTriangleIndices(1, 2, 3);

            return vertexArrayBuilder.CreateTexturedVao();
        }

        #endregion Internal Methods
    }
}