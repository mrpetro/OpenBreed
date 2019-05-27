//using OpenTK;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using OpenTK.Graphics.OpenGL;
//using OpenTK.Graphics;

//namespace OpenBreed.Core.Systems.Rendering.Helpers
//{
//    public class FontAtlas
//    {
//        #region Public Fields

//        public const string Characters = @"qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789µ§½!""#¤%&/()=?^*@£€${[]}\~¨'-_.:,;<>|°©®±¥";

//        public static uint[] indices = {
//                                            0,1,2,
//                                            0,2,3
//                                       };

//        public float CharacterWidthNormalized;

//        #endregion Public Fields

//        #region Private Fields

//        private readonly Dictionary<char, int> Lookup = new Dictionary<char, int>();
//        private int ibo;

//        private List<int> vboList;

//        #endregion Private Fields

//        #region Public Constructors

//        public FontAtlas(ITexture texture, int spriteSize, int spriteColumns, int spriteRows)
//        {
//            this.Texture = texture;

//            FontHeight = spriteSize;

//            vboList = new List<int>();

//            RenderTools.CreateIndicesArray(indices, out ibo);
//            BuildCoords(spriteRows, spriteColumns);
//        }

//        #endregion Public Constructors

//        #region Public Properties

//        public int FontHeight { get; }

//        #endregion Public Properties

//        #region Internal Properties

//        internal ITexture Texture { get; }

//        #endregion Internal Properties

//        #region Public Methods

//        public Bitmap GenerateCharacters(int fontSize, string fontName, out Size charSize)
//        {
//            var characters = new List<Bitmap>();
//            using (var font = new Font(fontName, fontSize))
//            {
//                for (int i = 0; i < Characters.Length; i++)
//                {
//                    var charBmp = GenerateCharacter(font, Characters[i]);
//                    characters.Add(charBmp);
//                }
//                charSize = new Size(characters.Max(x => x.Width), characters.Max(x => x.Height));
//                var charMap = new Bitmap(charSize.Width * characters.Count, charSize.Height);
//                using (var gfx = Graphics.FromImage(charMap))
//                {
//                    gfx.FillRectangle(Brushes.Black, 0, 0, charMap.Width, charMap.Height);
//                    for (int i = 0; i < characters.Count; i++)
//                    {
//                        var c = characters[i];
//                        gfx.DrawImageUnscaled(c, i * charSize.Width, 0);

//                        c.Dispose();
//                    }
//                }
//                return charMap;
//            }
//        }

//        public void Draw(Viewport viewport, int spriteId)
//        {
//            GL.BindTexture(TextureTarget.Texture2D, Texture.Id);
//            RenderTools.Draw(viewport, vboList[spriteId], ibo, 6);
//            GL.BindTexture(TextureTarget.Texture2D, 0);
//        }

//        #endregion Public Methods

//        #region Internal Methods

//        internal Vertex[] CreateVertices(Vector2 fontCoord, float fontWidth)
//        {
//            var uvSize = new Vector2(fontWidth, FontHeight);
//            uvSize = Vector2.Divide(uvSize, new Vector2(Texture.Width, Texture.Height));

//            var uvLD = fontCoord;
//            var uvRT = Vector2.Add(uvLD, uvSize);

//            Vertex[] vertices = {
//                                new Vertex(new Vector2(0,   0),              new Vector2(uvLD.X, uvRT.Y), Color4.White),
//                                new Vertex(new Vector2(fontWidth,  0),        new Vector2(uvRT.X, uvRT.Y), Color4.White),
//                                new Vertex(new Vector2(fontWidth,  FontHeight), new Vector2(uvRT.X, uvLD.Y), Color4.White),
//                                new Vertex(new Vector2(0,   FontHeight),       new Vector2(uvLD.X, uvLD.Y), Color4.White),
//                            };

//            return vertices;
//        }

//        #endregion Internal Methods

//        #region Private Methods

//        private Bitmap GenerateCharacter(Font font, char c)
//        {
//            var size = GetSize(font, c);
//            var bmp = new Bitmap((int)size.Width, (int)size.Height);
//            using (var gfx = Graphics.FromImage(bmp))
//            {
//                gfx.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);
//                gfx.DrawString(c.ToString(), font, Brushes.White, 0, 0);
//            }
//            return bmp;
//        }

//        private SizeF GetSize(Font font, char c)
//        {
//            using (var bmp = new Bitmap(512, 512))
//            {
//                using (var gfx = Graphics.FromImage(bmp))
//                {
//                    return gfx.MeasureString(c.ToString(), font);
//                }
//            }
//        }

//        private void Build()
//        {
//            for (int i = 0; i < Characters.Length; i++)
//            {
//                if (!Lookup.ContainsKey(Characters[i]))
//                    Lookup.Add(Characters[i], i);
//            }
//            CharacterWidthNormalized = 1f / Characters.Length;
//        }

//        private void InitializeIndices()
//        {
//        }

//        private void BuildCoords(int spriteRows, int spriteColumns)
//        {
//            for (int i = 0; i < Characters.Length; i++)
//            {
//                if (!Lookup.ContainsKey(Characters[i]))
//                {
//                    var coord = new Vector2(i, 0);
//                    coord = Vector2.Multiply(coord, SpriteSize);
//                    coord = Vector2.Divide(coord, new Vector2(Texture.Width, Texture.Height));

//                    var vertices = CreateVertices(coord);

//                    int vbo;
//                    RenderTools.CreateVertexArray(vertices, out vbo);
//                    vboList.Add(vbo);
//                }


//            }

//            CharacterWidthNormalized = 1f / Characters.Length;
//        }


//        #endregion Private Methods
//    }
//}