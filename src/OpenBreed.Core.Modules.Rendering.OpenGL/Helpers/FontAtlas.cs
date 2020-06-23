using OpenBreed.Core.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    internal class FontAtlas : IFont
    {
        //public const string Characters = @" qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789µ§½!""#¤%&/()=?^*@£€${[]}\~¨'-_.:,;<>|°©®±¥ł";

        #region Public Fields

        public static uint[] indices = {
                                            0,1,2,
                                            0,2,3
                                       };

        #endregion Public Fields

        #region Private Fields

        private readonly Dictionary<int, (int, float)> Lookup = new Dictionary<int, (int, float)>();
        private int ibo;
        private List<int> vboList;

        #endregion Private Fields

        #region Internal Constructors

        internal FontAtlas(FontAtlasBuilder builder)
        {
            Characters = new int[256];
            for (int i = 0; i < 256; i++)
                Characters[i] = i;

            Id = builder.GetNewId();
            vboList = new List<int>();

            RenderTools.CreateIndicesArray(indices, out ibo);

            Height = BuildCoords(builder.FontMan.Module.Textures, builder.FontName, builder.FontSize);
        }

        #endregion Internal Constructors

        #region Public Properties

        public int[] Characters { get; }

        /// <summary>
        /// Id of this sprite atlas
        /// </summary>
        public int Id { get; }

        public float Height { get; }

        #endregion Public Properties

        #region Internal Properties

        internal ITexture Texture { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public float GetWidth(char character)
        {
            return Lookup[character].Item2;
        }

        public float GetWidth(string text)
        {
            var totalWidth = 0.0f;
            for (int i = 0; i < text.Length; i++)
                totalWidth += Lookup[text[i]].Item2;
            return totalWidth;
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

        public void Draw(char character)
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture.InternalId);
            RenderTools.Draw(vboList[Lookup[character].Item1], ibo, 6);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Draw(string text)
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture.InternalId);

            var offset = 0.0f;

            for (int i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                var key = Lookup[ch].Item1;

                GL.Translate(offset, 0.0f, 0.0f);
                RenderTools.Draw(vboList[key], ibo, 6);
                offset = Lookup[ch].Item2 * 0.75f;
            }

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Draw(int spriteId)
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture.InternalId);
            RenderTools.Draw(vboList[spriteId], ibo, 6);
            GL.BindTexture(TextureTarget.Texture2D, 0);
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

        #endregion Internal Methods

        #region Private Methods

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

        private void InitializeIndices()
        {
        }

        private float BuildCoords(ITextureMan textures, string fontName, int fontSize)
        {
            using (var font = new Font(fontName, fontSize))
            {
                var bitmap = GenerateCharacters(font, out Size maxCharSize);

                Texture = textures.Create($"Textures/Fonts/{fontName}/{fontSize}", bitmap);

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

        #endregion Private Methods
    }
}