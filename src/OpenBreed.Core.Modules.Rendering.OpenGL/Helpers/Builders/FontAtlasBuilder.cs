using OpenBreed.Core.Modules.Rendering.Managers;
using OpenTK;
using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    public class FontAtlasBuilder
    {
        #region Internal Fields

        internal readonly List<Vector2> coords = new List<Vector2>();

        internal FontMan FontMan;

        #endregion Internal Fields

        #region Internal Constructors

        internal FontAtlasBuilder(FontMan fontMan)
        {
            FontMan = fontMan;
        }

        #endregion Internal Constructors

        #region Internal Properties

        internal int FontSize { get; private set; }

        internal string FontName { get; private set; }

        #endregion Internal Properties

        #region Public Methods

        public IFont Build()
        {
            return new FontAtlas(this);
        }

        #endregion Public Methods

        #region Internal Methods

        internal int GetNewId()
        {
            return FontMan.GenerateNewId();
        }

        internal void SetFontName(string fontName)
        {
            FontName = fontName;
        }

        internal void SetFontSize(int fontSize)
        {
            FontSize = fontSize;
        }

        #endregion Internal Methods
    }
}