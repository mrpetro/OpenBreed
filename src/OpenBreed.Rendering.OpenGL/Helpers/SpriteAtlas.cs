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

        internal RenderContextItem<List<int>> ContextData { get; } = new RenderContextItem<List<int>>();
        internal ITexture Texture { get; }

        #endregion Internal Properties

        #region Public Methods

        public Vector2 GetSpriteSize(int spriteId)
        {
            return data[spriteId].Size;
        }

        public void Load(IRenderContext renderContext)
        {
            var vboList = new List<int>();

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
                var itemVbo = CreateSpriteVertices(renderContext, item);

                vboList.Add(itemVbo);
            }

            ContextData.Add(renderContext, vboList);
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

        internal int CreateSpriteVertices(IRenderContext renderContext, SpriteData spriteData)
        {
            var vertices = UvBox.CreateVertices(
                spriteData.U,
                spriteData.V,
                spriteData.Width,
                spriteData.Height,
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

        #endregion Internal Methods
    }
}