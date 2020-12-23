using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Builders
{
    public class FontAtlasBuilder
    {
        #region Public Fields

        public static uint[] indices = {
                                            0,1,2,
                                            0,2,3
                                       };

        #endregion Public Fields

        #region Internal Fields

        internal readonly List<Vector2> coords = new List<Vector2>();

        internal readonly Dictionary<int, (int, float)> Lookup = new Dictionary<int, (int, float)>();
        internal FontMan FontMan;
        internal float Height;
        internal ITexture Texture;

        internal int ibo;
        internal List<int> vboList;

        #endregion Internal Fields

        #region Private Fields

        private readonly ITextureMan textureMan;

        #endregion Private Fields

        #region Internal Constructors

        internal FontAtlasBuilder(FontMan fontMan, ITextureMan textureMan)
        {
            FontMan = fontMan;
            this.textureMan = textureMan;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal int[] Characters { get; private set; }

        internal int FontSize { get; private set; }

        internal string FontName { get; private set; }

        internal int Id { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public IFont Build()
        {
            Id = GetNewId();
            vboList = new List<int>();

            Characters = new int[256];
            for (int i = 0; i < 256; i++)
                Characters[i] = i;

            RenderTools.CreateIndicesArray(indices, out ibo);

            Height = BuildCoords(FontName, FontSize);

            return new FontAtlas(this);
        }

        public Bitmap GenerateCharacters(Font font, out Size charSize)
        {
            var characters = new List<Bitmap>();

            for (int i = 0; i < Characters.Length; i++)
            {
                var charBmp = GenerateCharacter(font, Characters[i]);
                characters.Add(charBmp);
            }
            charSize = new Size(characters.Max(x => x.Width), characters.Max(x => x.Height));

            var charMapWidth = charSize.Width * characters.Count;
            var charMapHeight = charSize.Height;
            //var charMapWidth = MathHelper.NextPowerOfTwo(charSize.Width * characters.Count);
            //var charMapHeight = MathHelper.NextPowerOfTwo(charSize.Height);

            var charMap = new Bitmap(charMapWidth, charMapHeight);
            using (var gfx = Graphics.FromImage(charMap))
            {
                gfx.FillRectangle(Brushes.Black, 0, 0, charMap.Width, charMap.Height);
                for (int i = 0; i < characters.Count; i++)
                {
                    var c = characters[i];
                    gfx.DrawImageUnscaled(c, i * charSize.Width, 0);

                    c.Dispose();
                }
            }
            return charMap;
        }

        #endregion Public Methods

        #region Internal Methods

        internal Vertex[] CreateVertices(Vector2 fontCoord, float fontWidth, float fontHeight)
        {
            var uvSize = new Vector2(fontWidth, fontHeight);
            uvSize = Vector2.Divide(uvSize, new Vector2(Texture.Width, Texture.Height));

            var uvLD = fontCoord;
            uvLD = Vector2.Divide(uvLD, new Vector2(Texture.Width, Texture.Height));
            var uvRT = Vector2.Add(uvLD, uvSize);

            Vertex[] vertices = {
                                new Vertex(new Vector2(0,   0),              new Vector2(uvLD.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(fontWidth,  0),        new Vector2(uvRT.X, uvRT.Y), Color4.White),
                                new Vertex(new Vector2(fontWidth,  fontHeight), new Vector2(uvRT.X, uvLD.Y), Color4.White),
                                new Vertex(new Vector2(0,   fontHeight),       new Vector2(uvLD.X, uvLD.Y), Color4.White),
                            };

            return vertices;
        }

        internal int GetNewId()
        {
            return FontMan.GenerateNewId();
        }

        internal void SetFontName(string fontName)
        {
            FontName = fontName;
        }

        internal void SetFontSize(int fontSize)
        {
            FontSize = fontSize;
        }

        #endregion Internal Methods

        #region Private Methods

        private SizeF MeasureSize(Font font, char c)
        {
            using (var bmp = new Bitmap(512, 512))
            {
                using (var gfx = Graphics.FromImage(bmp))
                {
                    return gfx.MeasureString(c.ToString(), font);
                }
            }
        }

        private Bitmap GenerateCharacter(Font font, int intCh)
        {
            var ch = (char)intCh;
            var size = MeasureSize(font, ch);
            var bmp = new Bitmap((int)size.Width, (int)size.Height);
            using (var gfx = Graphics.FromImage(bmp))
            {
                gfx.FillRectangle(Brushes.Black, 0, 0, bmp.Width, bmp.Height);
                gfx.DrawRectangle(Pens.Blue, 0, 0, bmp.Width, bmp.Height);
                gfx.DrawString(ch.ToString(), font, Brushes.White, 0, 0); ;
            }
            return bmp;
        }

        private float MeasureWidth(Font font, char c)
        {
            using (var bmp = new Bitmap(512, 512))
            {
                using (var gfx = Graphics.FromImage(bmp))
                {
                    var stringFormat = new StringFormat(StringFormat.GenericTypographic);
                    stringFormat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                    return gfx.MeasureString(c.ToString(), font, 0, stringFormat).Width;
                }
            }
        }

        private float BuildCoords(string fontName, int fontSize)
        {
            using (var font = new Font(fontName, fontSize))
            {
                var bitmap = GenerateCharacters(font, out Size maxCharSize);

                Texture = textureMan.Create($"Textures/Fonts/{fontName}/{fontSize}", bitmap);

                for (int i = 0; i < Characters.Length; i++)
                {
                    if (!Lookup.ContainsKey(Characters[i]))
                    {
                        var charSize = MeasureSize(font, (char)Characters[i]);
                        var charWidth = MeasureWidth(font, (char)Characters[i]);

                        var coord = new Vector2(i * maxCharSize.Width, 0);
                        var vertices = CreateVertices(coord, charSize.Width, charSize.Height);

                        int vbo;
                        RenderTools.CreateVertexArray(vertices, out vbo);
                        vboList.Add(vbo);

                        Lookup.Add(Characters[i], (i, charWidth));
                    }
                }

                return MeasureHeight(font);
            }
        }

        private float MeasureHeight(Font font)
        {
            using (var bmp = new Bitmap(512, 512))
            {
                using (var gfx = Graphics.FromImage(bmp))
                {
                    return font.GetHeight(gfx);
                }
            }
        }

        #endregion Private Methods
    }
}