using OpenBreed.Rendering.OpenGL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Shaders
{
    internal class TexturedWithPaletteShader : Shader
    {
        #region Public Constructors

        public TexturedWithPaletteShader() : base("Shaders/textured.vert", "Shaders/texturedWithPalette.frag")
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public int texture0 { get; private set; }
        public int aColor { get; private set; }
        public int model { get; private set; }
        public int view { get; private set; }
        public int projection { get; private set; }
        public int maskIndex { get; private set; }
        public int palette { get; private set; }

        #endregion Public Properties
    }
}