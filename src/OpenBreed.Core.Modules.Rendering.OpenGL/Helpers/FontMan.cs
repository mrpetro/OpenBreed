using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    public class FontMan : IFontMan
    {
        #region Private Fields

        private readonly List<IFont> items = new List<IFont>();
        private Dictionary<string, FontAtlas> fonts = new Dictionary<string, FontAtlas>();

        private OpenGLModule module;

        #endregion Private Fields

        #region Public Constructors

        public FontMan(OpenGLModule module)
        {
            this.module = module;
        }

        #endregion Public Constructors

        #region Public Methods

        public IFont GetById(int id)
        {
            return items[id];
        }

        public IFont Create(string fontName, int fontSize)
        {
            var newFontAtlas = new FontAtlas(items.Count, module.Textures, fontName, fontSize);
            items.Add(newFontAtlas);
            return newFontAtlas;
        }

        #endregion Public Methods
    }
}