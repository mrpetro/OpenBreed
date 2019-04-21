using System;
using OpenTK;
using OpenTK.Graphics;

namespace OpenBreed.Game.Rendering
{
    public class TileAtlas
    {
        #region Private Fields

        private readonly Vector2[] tileCoords;

        #endregion Private Fields

        #region Public Constructors

        public TileAtlas(Texture texture, int tileSize, int tileRows, int tileColumns)
        {
            this.Texture = texture;

            TileSize = tileSize;

            tileCoords = new Vector2[tileRows * tileColumns];

            BuildCoords(tileRows, tileColumns);
        }

        #endregion Public Constructors

        #region Public Properties

        public Texture Texture { get; }
        public int TileSize { get; }

        #endregion Public Properties

        #region Public Methods

        public Vector2 GetCoords(int tileId)
        {
            return tileCoords[tileId];
        }

        #endregion Public Methods

        #region Private Methods

        private void BuildCoords(int tileRows, int tileColumns)
        {
            for (int y = 0; y < tileRows; y++)
            {
                for (int x = 0; x < tileColumns; x++)
                {
                    var coord = new Vector2(x, y);
                    coord = Vector2.Multiply(coord, TileSize);
                    coord = Vector2.Divide( coord, new Vector2(Texture.Width, Texture.Height));
                    tileCoords[x + y * tileRows] = coord;
                }
            }
        }



        internal Vertex[] GetVertices(int tileId)
        {
            var uvSize = new Vector2(TileSize, TileSize);
            uvSize = Vector2.Divide(uvSize, new Vector2(Texture.Width, Texture.Height));

            var uvLD = tileCoords[tileId];
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