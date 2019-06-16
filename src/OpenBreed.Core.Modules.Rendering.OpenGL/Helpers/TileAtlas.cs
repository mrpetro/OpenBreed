using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    internal class TileAtlas : ITileAtlas
    {
        #region Private Fields

        private static uint[] indices = {
                                            0,1,2,
                                            0,2,3
                                       };

        private int ibo;

        private List<int> vboList;

        #endregion Private Fields

        #region Internal Constructors

        internal TileAtlas(int id, ITexture texture, int tileSize, int tileColumns, int tileRows)
        {
            Id = id;
            this.Texture = texture;

            TileSize = tileSize;

            vboList = new List<int>();

            RenderTools.CreateIndicesArray(indices, out ibo);
            BuildCoords(tileRows, tileColumns);
        }

        #endregion Internal Constructors

        #region Public Properties

        public ITexture Texture { get; }
        public float TileSize { get; }
        public int Id { get; }

        #endregion Public Properties

        #region Public Methods

        public void Draw(int tileId)
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture.Id);
            RenderTools.Draw(vboList[tileId], ibo, 6);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        #endregion Public Methods

        #region Internal Methods

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

        #endregion Internal Methods

        #region Private Methods

        private void BuildCoords(int tileRows, int tileColumns)
        {
            for (int y = 0; y < tileRows; y++)
            {
                for (int x = 0; x < tileColumns; x++)
                {
                    var coord = new Vector2(x, y);
                    coord = Vector2.Multiply(coord, TileSize);
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