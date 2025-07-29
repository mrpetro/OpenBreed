using OpenBreed.Rendering.OpenGL.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Shaders
{
    internal class NontexturedShader : Shader
    {
        public NontexturedShader() : base("Shaders/nontextured.vert", "Shaders/nontextured.frag")
        {
        }

        public int aColor { get; private set; }
        public int model { get; private set; }
        public int view { get; private set; }
        public int projection { get; private set; }


    }
}
