﻿using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    public class SpriteAtlas : ISpriteAtlas
    {
        #region Public Fields

        public static uint[] indices = {
                                            0,1,2,
                                            0,2,3
                                       };

        #endregion Public Fields

        #region Private Fields

        private int ibo;

        private List<int> vboList;

        #endregion Private Fields

        #region Public Constructors

        public SpriteAtlas(int id, ITexture texture, float spriteWidth, float spriteHeight, int spriteColumns, int spriteRows)
        {
            Id = id;
            Texture = texture;
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;

            vboList = new List<int>();

            RenderTools.CreateIndicesArray(indices, out ibo);
            BuildCoords(spriteRows, spriteColumns);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Id of this sprite atlas
        /// </summary>
        public int Id { get; }

        public float SpriteWidth { get; }

        public float SpriteHeight { get; }

        #endregion Public Properties

        #region Internal Properties

        internal ITexture Texture { get; }

        #endregion Internal Properties

        #region Public Methods

        public void Draw(int spriteId)
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture.Id);
            RenderTools.Draw(vboList[spriteId], ibo, 6);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        #endregion Public Methods

        #region Internal Methods

        internal Vertex[] CreateVertices(Vector2 spriteCoord)
        {
            var uvSize = new Vector2(SpriteWidth, SpriteHeight);
            uvSize = Vector2.Divide(uvSize, new Vector2(Texture.Width, Texture.Height));

            var uvLD = spriteCoord;
            var uvRT = Vector2.Add(uvLD, uvSize);

            Vertex[] vertices = {
                                new Vertex(new Vector2(0,   0),              new Vector2(uvLD.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(SpriteWidth,  0),        new Vector2(uvRT.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(SpriteWidth,  SpriteHeight), new Vector2(uvRT.X, uvLD.Y), Color4.White),
                                new Vertex(new Vector2(0,   SpriteHeight),       new Vector2(uvLD.X, uvLD.Y), Color4.White),
                            };

            return vertices;
        }

        #endregion Internal Methods

        #region Private Methods

        private void InitializeIndices()
        {
        }

        private void BuildCoords(int spriteRows, int spriteColumns)
        {
            for (int y = 0; y < spriteRows; y++)
            {
                for (int x = 0; x < spriteColumns; x++)
                {
                    var coord = new Vector2(x, y);
                    coord = Vector2.Multiply(coord, new Vector2(SpriteWidth, SpriteHeight));
                    coord = Vector2.Divide(coord, new Vector2(Texture.Width, Texture.Height));

                    var vertices = CreateVertices(coord);

                    int vbo;
                    RenderTools.CreateVertexArray(vertices, out vbo);
                    vboList.Add(vbo);
                }
            }
        }

        #endregion Private Methods
    }
}