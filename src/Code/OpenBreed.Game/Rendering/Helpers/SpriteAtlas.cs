using System;
using OpenTK;
using OpenTK.Graphics;

namespace OpenBreed.Game.Rendering.Helpers
{
    public class SpriteAtlas
    {
        #region Private Fields

        private readonly Vector2[] spriteCoords;

        #endregion Private Fields

        #region Public Constructors

        public SpriteAtlas(Texture texture, int spriteSize, int spriteColumns, int spriteRows)
        {
            this.Texture = texture;

            SpriteSize = spriteSize;

            spriteCoords = new Vector2[spriteRows * spriteColumns];

            BuildCoords(spriteRows, spriteColumns);
        }

        #endregion Public Constructors

        #region Public Properties

        public Texture Texture { get; }
        public int SpriteSize { get; }

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
                    spriteCoords[x + y * spriteRows] = coord;
                }
            }
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