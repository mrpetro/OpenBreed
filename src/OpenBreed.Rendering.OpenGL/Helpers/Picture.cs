using Microsoft.Extensions.Logging;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Managers;
using System;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    internal class Picture : IPicture
    {
        #region Private Fields

        private readonly RenderContextItem<int> contextData = new RenderContextItem<int>();

        #endregion Private Fields

        #region Public Constructors

        public Picture(PictureBuilder builder)
        {
            Texture = builder.Texture;
            Id = builder.Register(this);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Id of this picture
        /// </summary>
        public int Id { get; }

        #endregion Public Properties

        #region Internal Properties

        internal ITexture Texture { get; }

        #endregion Internal Properties

        #region Internal Methods

        internal int GetVbo(IRenderContext context)
        {
            var vao = contextData.GetOrAdd(context, Load);
            return vao;
        }

        #endregion Internal Methods

        #region Private Methods

        private int Load(IRenderContext renderContext)
        {
            //logger.LogTrace("Picture '{0}' loaded into render context..", item.Id);

            return CreatePictureVertices(renderContext);
        }

        private int CreatePictureVertices(IRenderContext renderContext)
        {
            var uvBox = new UvBox(0, 0, Texture.Width, Texture.Height);
            var vertices = UvBox.CreateVertices(uvBox, Texture.Width, Texture.Height);

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