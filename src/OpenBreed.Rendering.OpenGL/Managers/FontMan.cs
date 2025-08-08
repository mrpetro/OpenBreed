using Microsoft.Extensions.Logging;
using OpenBreed.Common.Tools;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class FontMan : IFontMan
    {
        #region Private Fields

        private readonly ITextureMan textureMan;
        private readonly ISpriteMan spriteMan;
        private readonly ILogger logger;
        private readonly List<IFontAtlas> items = new List<IFontAtlas>();
        private readonly Dictionary<string, IFontAtlas> aliases = new Dictionary<string, IFontAtlas>();
        private readonly TextMeasurer textMeasurer = new TextMeasurer();

        #endregion Private Fields

        #region Public Constructors

        public FontMan(
            ITextureMan textureMan,
            ISpriteMan spriteMan,
            ILogger logger)
        {
            this.textureMan = textureMan;
            this.spriteMan = spriteMan;
            this.logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public IFontAtlas GetById(int id)
        {
            return items[id];
        }

        public IFontAtlasBuilder Create()
        {
            return new FontAtlasBuilder(this, spriteMan);
        }

        public IFontAtlas GetGfxFont(string fontName)
        {
            var alias = $"Gfx/{fontName}";

            IFontAtlas result;
            if (aliases.TryGetValue(alias, out result))
                return result;
            else
                return null;
        }

        public IFontAtlas GetOSFont(string fontName, int fontSize)
        {
            fontName = fontName.Trim().ToLower();

            var alias = $"OS/{fontName}/{fontSize}";

            if (aliases.TryGetValue(alias, out IFontAtlas result))
            {
                return result;
            }

            var fontGenerator = new OSFontAtlasGenerator(this, textMeasurer, textureMan, spriteMan);
            var font = fontGenerator.Generate(fontName, fontSize);
            return font;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Register(string alias, IFontAtlas font)
        {
            items.Add(font);
            aliases.Add(alias, font);
        }

        internal int GenerateNewId()
        {
            return items.Count;
        }

        #endregion Internal Methods
    }
}