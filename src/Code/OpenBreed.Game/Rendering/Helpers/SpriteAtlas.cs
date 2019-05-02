using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenBreed.Game.Rendering.Helpers
{
    public class SpriteAtlas
    {
        public static uint[] indices = {
                                            0,1,2,
                                            0,2,3
                                       };

        #region Private Fields

        private int ibo;

        private readonly Vector2[] spriteCoords;

        #endregion Private Fields

        #region Public Constructors

        private void InitializeIndices()
        {
        }

        public SpriteAtlas(Texture texture, int spriteSize, int spriteColumns, int spriteRows)
        {
            this.Texture = texture;

            SpriteSize = spriteSize;

            spriteCoords = new Vector2[spriteRows * spriteColumns];
            vboList = new List<int>();

            BuildCoords(spriteRows, spriteColumns);
        }

        #endregion Public Constructors

        #region Public Properties

        public Texture Texture { get; }
        public int SpriteSize { get; }

        private List<int> vboList;

        #endregion Public Properties

        #region Public Methods

        public Vector2 GetCoords(int spriteId)
        {
            return spriteCoords[spriteId];
        }

        #endregion Public Methods

        #region Private Methods

        private void BuildCoords(int spriteRows, int spriteColumns)
        {
            for (int y = 0; y < spriteRows; y++)
            {
                for (int x = 0; x < spriteColumns; x++)
                {
                    var coord = new Vector2(x, y);
                    coord = Vector2.Multiply(coord, SpriteSize);
                    coord = Vector2.Divide( coord, new Vector2(Texture.Width, Texture.Height));

                    var spriteId = x + y * spriteRows;

                    spriteCoords[spriteId] = coord;

                    AddSprite(spriteId);
                }
            }
        }

        public void Draw(Viewport viewport, int spriteId)
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture.Id);
            RenderTools.Draw(viewport, vboList[spriteId], ibo, 6);
        }

        public void AddSprite(int spriteId)
        {
            var vertices = GetVertices(spriteId);

            int vbo;

            RenderTools.Create(vertices, indices, out vbo, out ibo);

            vboList.Add(vbo);
        }

        internal Vertex[] GetVertices(int tileId)
        {
            var uvSize = new Vector2(SpriteSize, SpriteSize);
            uvSize = Vector2.Divide(uvSize, new Vector2(Texture.Width, Texture.Height));

            var uvLD = spriteCoords[tileId];
            var uvRT = Vector2.Add(uvLD, uvSize);

            Vertex[] vertices = {
                                new Vertex(new Vector2(0,   0),              new Vector2(uvLD.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(SpriteSize,  0),        new Vector2(uvRT.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(SpriteSize,  SpriteSize), new Vector2(uvRT.X, uvLD.Y), Color4.White),
                                new Vertex(new Vector2(0,   SpriteSize),       new Vector2(uvLD.X, uvLD.Y), Color4.White),
                            };

            return vertices;
        }

        #endregion Private Methods
    }
}