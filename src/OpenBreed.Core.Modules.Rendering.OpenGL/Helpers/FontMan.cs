using System.Collections.Generic;

namespace OpenBreed.Core.Modules.Rendering.Helpers
{
    public class FontMan
    {
        #region Private Fields

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

        public FontAtlas Create(string fontName, int fontSize)
        {


            //bitmap.Save($"D:\\{fontName}-{fontSize}.png", ImageFormat.Png);


            return new FontAtlas(module.Textures, fontName, fontSize);
        }

        #endregion Public Methods
    }
}