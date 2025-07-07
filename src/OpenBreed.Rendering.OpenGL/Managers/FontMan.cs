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

        private readonly Queue<IFont> loadQueue = new Queue<IFont>();
        private readonly ITextureMan textureMan;
        private readonly ISpriteMan spriteMan;
        private readonly ILogger logger;
        private readonly List<IFont> items = new List<IFont>();
        private readonly Dictionary<string, IFont> aliases = new Dictionary<string, IFont>();
        private readonly TextMeasurer textMeasurer = new TextMeasurer();
        private readonly Dictionary<string, FontFromOSAtlas> fonts = new Dictionary<string, FontFromOSAtlas>();

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

        public IFont GetById(int id)
        {
            return items[id];
        }

        public IFontAtlasBuilder Create()
        {
            return new FontFromSpritesAtlasBuilder(this, spriteMan);
        }

        public IFont GetGfxFont(string fontName)
        {
            var alias = $"Gfx/{fontName}";

            IFont result;
            if (aliases.TryGetValue(alias, out result))
                return result;
            else
                return null;
        }

        public void LoadRefresh(IRenderContext renderContext)
        {
            while (loadQueue.Count > 0)
            {
                var item = loadQueue.Dequeue();
                item.Load(renderContext);

                logger.LogTrace("Font '{0}' loaded into render context..", item.Id);
            }
        }

        public IFont GetOSFont(string fontName, int fontSize)
        {
            fontName = fontName.Trim().ToLower();

            var alias = $"OS/{fontName}/{fontSize}";
            IFont result;
            if (aliases.TryGetValue(alias, out result))
                return result;

            var fontGenerator = new FontFromOSAtlasGenerator(this, textMeasurer, textureMan);
            fontGenerator.SetName(fontName);
            fontGenerator.SetSize(fontSize);
            var font = fontGenerator.Build();
            Register(alias, font);
            return font;
        }

        public void UnloadAll(IRenderContext context)
        {
            throw new System.NotImplementedException();
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Register(string alias, IFont font)
        {
            items.Add(font);
            aliases.Add(alias, font);
            loadQueue.Enqueue(font);
        }

        internal int GenerateNewId()
        {
            return items.Count;
        }

        #endregion Internal Methods
    }
}