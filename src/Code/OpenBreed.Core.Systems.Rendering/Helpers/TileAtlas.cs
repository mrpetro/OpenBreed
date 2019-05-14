using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace OpenBreed.Core.Systems.Rendering.Helpers
{
    public class TileAtlas
    {
        private static uint[] indices = {
                                            0,1,2,
                                            0,2,3
                                       };

        private int ibo;

        private List<int> vboList;


        #region Private Fields

        #endregion Private Fields

        #region Public Constructors

        public TileAtlas(ITexture texture, int tileSize, int tileColumns, int tileRows)
        {
            this.Texture = texture;

            TileSize = tileSize;

            vboList = new List<int>();

            RenderTools.CreateIndicesArray(indices, out ibo);
            BuildCoords(tileRows, tileColumns);
        }

        #endregion Public Constructors

        #region Public Properties

        public ITexture Texture { get; }
        public int TileSize { get; }

        #endregion Public Properties

        #region Public Methods

        #endregion Public Methods

        #region Private Methods

        public void Draw(Viewport viewport, int tileId)
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture.Id);
            RenderTools.Draw(viewport, vboList[tileId], ibo, 6);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void BuildCoords(int tileRows, int tileColumns)
        {
            for (int y = 0; y < tileRows; y++)
            {
                for (int x = 0; x < tileColumns; x++)
                {
                    var coord = new Vector2(x, y);
                    coord = Vector2.Multiply(coord, TileSize);
                    coord = Vector2.Divide( coord, new Vector2(Texture.Width, Texture.Height));

                    var vertices = CreateVertices(coord);

                    int vbo;
                    RenderTools.CreateVertexArray(vertices, out vbo);
                    vboList.Add(vbo);
                }
            }
        }


        internal Vertex[] CreateVertices(Vector2 coord)
        {
            var uvSize = new Vector2(TileSize, TileSize);
            uvSize = Vector2.Divide(uvSize, new Vector2(Texture.Width, Texture.Height));

            var uvLD = coord;
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