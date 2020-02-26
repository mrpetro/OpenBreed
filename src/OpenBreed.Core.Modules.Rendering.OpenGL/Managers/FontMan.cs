using OpenBreed.Core.Managers;
using OpenBreed.Core.Modules.Rendering.Helpers;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Managers
{
    internal class FontMan : IFontMan
    {
        #region Internal Fields

        internal readonly OpenGLModule Module;

        #endregion Internal Fields

        #region Private Fields

        private readonly List<IFont> items = new List<IFont>();
        private readonly Dictionary<string, IFont> aliases = new Dictionary<string, IFont>();
        private Dictionary<string, FontAtlas> fonts = new Dictionary<string, FontAtlas>();

        #endregion Private Fields

        #region Internal Constructors

        internal FontMan(OpenGLModule module)
        {
            Module = module;
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
            var faBuilder = new FontAtlasBuilder(this);
            faBuilder.SetFontName(fontName);
            faBuilder.SetFontSize(fontSize);

            var alias = $"Fonts/{fontName}/{fontSize}";
            IFont result;
            if (aliases.TryGetValue(alias, out result))
                return result;

            result = new FontAtlas(faBuilder);
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