using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Builders
{
    public class FontFromOSAtlasGenerator
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
        internal float Height;
        internal ITexture Texture;
        internal List<int> vboList;

        #endregion Internal Fields

        #region Private Fields

        private readonly TextMeasurer textMeasurer;
        private readonly ITextureMan textureMan;
        private readonly FontMan fontMan;

        #endregion Private Fields

        #region Internal Constructors

        internal FontFromOSAtlasGenerator(FontMan fontMan, TextMeasurer textMeasurer, ITextureMan textureMan, IPrimitiveRenderer primitiveRenderer)
        {
            this.fontMan = fontMan;
            this.textMeasurer = textMeasurer;
            this.textureMan = textureMan;
            PrimitiveRenderer = primitiveRenderer;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal int[] Characters { get; private set; }

        internal int FontSize { get; private set; }

        internal string FontName { get; private set; }

        internal int Id { get; private set; }

        internal IPrimitiveRenderer PrimitiveRenderer { get; }

        #endregion Internal Properties

        #region Public Methods

        public IFont Build()
        {
            Id = GetNewId();
            vboList = new List<int>();

            Characters = new int[256];
            for (int i = 0; i < 256; i++)
                Characters[i] = i;

            Height = BuildCoords(FontName, FontSize);

            return new FontFromOSAtlas(this);
        }

        public void SetName(string fontName)
        {
            FontName = fontName;
        }

        public void SetSize(int fontSize)
        {
            FontSize = fontSize;
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
            return fontMan.GenerateNewId();
        }

        #endregion Internal Methods

        #region Private Methods

        private Bitmap GenerateCharacters(Font font, out Size charSize)
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
                var backgroundBrush = new SolidBrush(Color.FromArgb(0, Color.Black));

                gfx.FillRectangle(backgroundBrush, 0, 0, charMap.Width, charMap.Height);
                for (int i = 0; i < characters.Count; i++)
                {
                    var c = characters[i];
                    gfx.DrawImageUnscaled(c, i * charSize.Width, 0);

                    c.Dispose();
                }
            }
            return charMap;
        }

        private Bitmap GenerateCharacter(Font font, int intCh)
        {
            var backgroundBrush = new SolidBrush(Color.FromArgb(0, Color.Black));

            var ch = (char)intCh;
            var size = textMeasurer.MeasureSize(font, ch);
            var bmp = new Bitmap((int)size.Width, (int)size.Height);
            using (var gfx = Graphics.FromImage(bmp))
            {
                gfx.FillRectangle(backgroundBrush, 0, 0, bmp.Width, bmp.Height);
                gfx.DrawString(ch.ToString(), font, Brushes.White, 0, 0); ;
            }
            return bmp;
        }

        private float BuildCoords(string fontName, int fontSize)
        {
            using (var font = new Font(fontName, fontSize))
            {
                var bitmap = GenerateCharacters(font, out Size maxCharSize);

                //bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

                Texture = textureMan.Create($"Textures/Fonts/{fontName}/{fontSize}", bitmap);

                for (int ci = 0; ci < Characters.Length; ci++)
                {
                    if (!Lookup.ContainsKey(Characters[ci]))
                    {
                        var charSize = textMeasurer.MeasureSize(font, (char)Characters[ci]);
                        var charWidth = textMeasurer.MeasureWidth(font, (char)Characters[ci]);

                        var texCoord = new Vector2(ci * maxCharSize.Width, 0);
                        var vertices = CreateVertices(texCoord, charSize.Width, charSize.Height);

                        var vertexArrayBuilder = PrimitiveRenderer.CreatePosTexCoordArray();
                        vertexArrayBuilder.AddVertex(vertices[0].position, vertices[0].texCoord);
                        vertexArrayBuilder.AddVertex(vertices[1].position, vertices[1].texCoord);
                        vertexArrayBuilder.AddVertex(vertices[2].position, vertices[2].texCoord);
                        vertexArrayBuilder.AddVertex(vertices[3].position, vertices[3].texCoord);

                        vertexArrayBuilder.AddTriangleIndices(0, 1, 3);
                        vertexArrayBuilder.AddTriangleIndices(1, 2, 3);

                        var vao = vertexArrayBuilder.CreateTexturedVao();

                        vboList.Add(vao);

                        Lookup.Add(Characters[ci], (ci, charWidth));
                    }
                }

                return textMeasurer.MeasureHeight(font);
            }
        }

        #endregion Private Methods
    }
}