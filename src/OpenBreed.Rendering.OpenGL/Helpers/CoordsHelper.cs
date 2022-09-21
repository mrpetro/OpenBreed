using OpenTK.Mathematics;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    public class UvBox
    {
        #region Public Constructors

        public UvBox(int u, int v, int width, int height)
        {
            U = u;
            V = v;
            Width = width;
            Height = height;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Height { get; set; }
        public int U { get; set; }
        public int V { get; set; }
        public int Width { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static Vertex[] CreateVertices(UvBox uvBox, int textureWidth, int textureHeight)
        {
            var uvCoord = Vector2.Divide(new Vector2(uvBox.U, uvBox.V), new Vector2(textureWidth, textureHeight));
            var uvSize = Vector2.Divide(new Vector2(uvBox.Width, uvBox.Height), new Vector2(textureWidth, textureHeight));

            var uvLD = new Vector2(uvCoord.X, uvCoord.Y);
            var uvRT = Vector2.Add(uvLD, uvSize);

            Vertex[] vertices = {
                                new Vertex(new Vector2(0,   0),              new Vector2(uvLD.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(uvBox.Width,  0),        new Vector2(uvRT.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(uvBox.Width,  uvBox.Height), new Vector2(uvRT.X, uvLD.Y), Color4.White),
                                new Vertex(new Vector2(0,   uvBox.Height),       new Vector2(uvLD.X, uvLD.Y), Color4.White),
                            };

            return vertices;
        }

        #endregion Public Methods
    }
}