using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenBreed.Rendering.OpenGL.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Builders
{
    public class OSFontAtlasGenerator
    {
        #region Private Fields

        private const int charactersCount = 256;
        private readonly TextMeasurer textMeasurer;
        private readonly ITextureMan textureMan;
        private readonly FontMan fontMan;
        private readonly ISpriteMan spriteMan;

        #endregion Private Fields

        #region Internal Constructors

        internal OSFontAtlasGenerator(
            FontMan fontMan,
            TextMeasurer textMeasurer,
            ITextureMan textureMan,
            ISpriteMan spriteMan)
        {
            this.fontMan = fontMan;
            this.textMeasurer = textMeasurer;
            this.textureMan = textureMan;
            this.spriteMan = spriteMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public IFontAtlas Generate(string fontName, int fontSize)
        {
            var builder = fontMan.Create();

            builder.SetName(fontName);

            var characterLookup = BuildCoords(fontName, fontSize, out float height, out ITexture texture, out Size maxCharSize);

            builder.SetHeight(height);

            var atlasBuilder = spriteMan.CreateAtlas();
            var spriteAtlasName = $"Fonts/{fontName}/{fontSize}";

            atlasBuilder.SetName(spriteAtlasName);

            atlasBuilder.SetTexture(texture.Id);

            var charIndex = 0;

            foreach (var pair in characterLookup)
            {
                var charCoordinates = new Vector2i(charIndex * maxCharSize.Width, 0);

                var vertices = UvBox.CreateVertices(
                    charCoordinates.X,
                    charCoordinates.Y,
                    (int)(pair.Value.Width - pair.Value.XOffset),
                    (int)height,
                    texture.Width,
                    texture.Height);

                atlasBuilder.AppendCoord(
                    charCoordinates.X,
                    charCoordinates.Y,
                    (int)(pair.Value.Width - pair.Value.XOffset),
                    (int)height);

                charIndex++;
            }

            var spriteAtlas = atlasBuilder.Build();

            builder.SetSpriteAtlas(spriteAtlasName);

            builder.SetAlias($"OS/{fontName}/{fontSize}");

            foreach (var item in characterLookup)
            {
                builder.MapCharacterToSpriteId(item.Key, item.Value.SpriteId, item.Value.Width, item.Value.XOffset);
            }

            return builder.Build();
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
            var characters = new List<Size>();

            for (int i = 0; i < charactersCount; i++)
            {
                var characterSize = textMeasurer.MeasureSize(font, (char)i);
                characters.Add(new Size((int)characterSize.Width, (int)characterSize.Width));
            }

            charSize = new Size(characters.Max(x => x.Width), characters.Max(x => x.Height));

            var charMapWidth = charSize.Width * characters.Count;
            var charMapHeight = charSize.Height;
            //var charMapWidth = MathHelper.NextPowerOfTwo(charSize.Width * characters.Count);
            //var charMapHeight = MathHelper.NextPowerOfTwo(charSize.Height);

            var charMap = new Bitmap(charMapWidth, charMapHeight);
            using (var gfx = Graphics.FromImage(charMap))
            {
                gfx.SmoothingMode = SmoothingMode.HighQuality;
                gfx.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                gfx.TextContrast = 4;

                var backgroundBrush = new SolidBrush(Color.FromArgb(0, Color.Black));

                gfx.FillRectangle(backgroundBrush, 0, 0, charMap.Width, charMap.Height);
                for (int i = 0; i < characters.Count; i++)
                {
                    var ch = (char)i;

                    gfx.DrawString(ch.ToString(), font, Brushes.White, i * charSize.Width, 0);
                }
            }
            return charMap;
        }

        private Dictionary<int, FontCharData> BuildCoords(string fontName, int fontSize, out float height, out ITexture texture, out Size maxCharSize)
        {
            var lookup = new Dictionary<int, FontCharData>();

            using (var font = new Font(fontName, fontSize))
            {
                var bitmap = GenerateCharacters(font, out maxCharSize);

                texture = textureMan.Create($"Textures/Fonts/{fontName}/{fontSize}", bitmap);

                for (int ci = 0; ci < charactersCount; ci++)
                {
                    if (!lookup.ContainsKey(ci))
                    {
                        var ch = (char)ci;

                        var charSize = textMeasurer.MeasureSize(font, ch);
                        var charWidth = textMeasurer.MeasureWidth(font, ch);

                        lookup.Add(ci, new FontCharData(ci, charWidth - charSize.Width, charWidth));
                    }
                }

                height = textMeasurer.MeasureHeight(font);

                return lookup;
            }
        }

        #endregion Private Methods
    }

    internal class FontCharData
    {
        #region Public Constructors

        public FontCharData(int spriteId, float xOffset, float width)
        {
            SpriteId = spriteId;
            XOffset = xOffset;
            Width = width;
        }

        #endregion Public Constructors

        #region Public Properties

        public int SpriteId { get; }
        public float XOffset { get; }
        public float Width { get; }

        #endregion Public Properties
    }
}