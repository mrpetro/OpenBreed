using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Rendering.OpenGL.Builders;
using OpenBreed.Rendering.OpenGL.Helpers;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class FontMan : IFontMan
    {
        #region Internal Fields

        private readonly ITextureMan textureMan;

        #endregion Internal Fields

        #region Private Fields

        private readonly List<IFont> items = new List<IFont>();
        private readonly Dictionary<string, IFont> aliases = new Dictionary<string, IFont>();
        private Dictionary<string, FontAtlas> fonts = new Dictionary<string, FontAtlas>();

        #endregion Private Fields

        #region Internal Constructors

        internal FontMan(ITextureMan textureMan)
        {
            this.textureMan = textureMan;
        }

        #endregion Internal Constructors

        #region Public Methods

        public IFont GetById(int id)
        {
            return items[id];
        }

        public IFont Create(string fontName, int fontSize)
        {
            fontName = fontName.Trim().ToLower();

            var alias = $"Fonts/{fontName}/{fontSize}";
            IFont result;
            if (aliases.TryGetValue(alias, out result))
                return result;

            var faBuilder = new FontAtlasBuilder(this, textureMan);
            faBuilder.SetFontName(fontName);
            faBuilder.SetFontSize(fontSize);

            result = faBuilder.Build();
            items.Add(result);
            aliases.Add(alias, result);
            return result;
        }

        #endregion Public Methods

        #region Internal Methods

        internal int GenerateNewId()
        {
            return items.Count;
        }

        #endregion Internal Methods
    }
}