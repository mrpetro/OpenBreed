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

        /// <summary>
        /// U coordinate on texture
        /// </summary>
        public int U;

        /// <summary>
        /// V coordinate on texture
        /// </summary>
        public int V;

        /// <summary>
        /// Width on texture
        /// </summary>
        public int Width;

        /// <summary>
        /// Height on texture
        /// </summary>
        public int Height;

        #endregion Public Properties

        #region Public Methods

        public static Vertex[] CreateVertices(int u, int v, int width, int height, int textureWidth, int textureHeight)
        {
            var uvCoord = Vector2.Divide(new Vector2(u, v), new Vector2(textureWidth, textureHeight));
            var uvSize = Vector2.Divide(new Vector2(width, height), new Vector2(textureWidth, textureHeight));

            var uvLD = new Vector2(uvCoord.X, uvCoord.Y);
            var uvRT = Vector2.Add(uvLD, uvSize);

            Vertex[] vertices = {
                                new Vertex(new Vector2(0,   0),              new Vector2(uvLD.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(width,  0),        new Vector2(uvRT.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(width,  height), new Vector2(uvRT.X, uvLD.Y), Color4.White),
                                new Vertex(new Vector2(0,   height),       new Vector2(uvLD.X, uvLD.Y), Color4.White),
                            };

            return vertices;
        }

        public static Vertex[] CreateVertices(UvBox uvBox, int textureWidth, int textureHeight) =>
            CreateVertices(uvBox.U, uvBox.V, uvBox.Width, uvBox.Height, textureWidth, textureHeight);

        #endregion Public Methods
    }
}