using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Builders
{
    internal class FontCharData
    {
        public FontCharData(int index, float xOffset, float width)
        {
            Index = index;
            XOffset = xOffset;
            Width = width;
        }

        public int Index { get; }
        public float XOffset { get; }
        public float Width { get; }
    }

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

        internal readonly Dictionary<int, FontCharData> Lookup = new Dictionary<int, FontCharData>();
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

        internal FontFromOSAtlasGenerator(FontMan fontMan, TextMeasurer textMeasurer, ITextureMan textureMan)
        {
            this.fontMan = fontMan;
            this.textMeasurer = textMeasurer;
            this.textureMan = textureMan;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal int[] Characters { get; private set; }

        internal int FontSize { get; private set; }

        internal string FontName { get; private set; }

        internal Size MaximumCharacterSize { get; private set; }

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
                gfx.SmoothingMode = SmoothingMode.HighQuality;
                gfx.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                gfx.TextContrast = 4;

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

                MaximumCharacterSize = maxCharSize;

                Texture = textureMan.Create($"Textures/Fonts/{fontName}/{fontSize}", bitmap);

                for (int ci = 0; ci < Characters.Length; ci++)
                {
                    if (!Lookup.ContainsKey(Characters[ci]))
                    {
                        var charSize = textMeasurer.MeasureSize(font, (char)Characters[ci]);
                        var charWidth = textMeasurer.MeasureWidth(font, (char)Characters[ci]);

                        Lookup.Add(Characters[ci], new FontCharData(ci, charWidth - charSize.Width,  charWidth));
                    }
                }

                return textMeasurer.MeasureHeight(font);
            }
        }

        #endregion Private Methods
    }
}