using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    internal class FontMan : IFontMan
    {
        #region Internal Fields

        internal readonly OpenGLModule Module;

        #endregion Internal Fields

        #region Private Fields

        private readonly List<IFont> items = new List<IFont>();
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
            var faBuilder = new FontAtlasBuilder(this);
            faBuilder.SetFontName(fontName);
            faBuilder.SetFontSize(fontSize);
            var newFontAtlas = new FontAtlas(faBuilder);
            items.Add(newFontAtlas);
            return newFontAtlas;
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