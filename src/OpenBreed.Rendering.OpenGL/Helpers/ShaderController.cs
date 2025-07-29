using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.OpenGL.Helpers
{
    // A simple class meant to help create shaders.
    public class ShaderController
    {
        #region Public Constructors

        public TShader LoadShader<TShader>() where TShader : Shader, new()
        {
            var shader = new TShader();

            // There are several different types of shaders, but the only two you need for basic rendering are the vertex and fragment shaders.
            // The vertex shader is responsible for moving around vertices, and uploading that data to the fragment shader.
            //   The vertex shader won't be too important here, but they'll be more important later.
            // The fragment shader is responsible for then converting the vertices to "fragments", which represent all the data OpenGL needs to draw a pixel.
            //   The fragment shader is what we'll be using the most here.

            // Load vertex shader and compile
            var shaderSource = File.ReadAllText(shader.VertPath);

            // GL.CreateShader will create an empty shader (obviously). The ShaderType enum denotes which type of shader will be created.
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            // Now, bind the GLSL source code
            GL.ShaderSource(vertexShader, shaderSource);

            // And then compile
            CompileShader(vertexShader);

            // We do the same for the fragment shader.
            shaderSource = File.ReadAllText(shader.FragPath);
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, shaderSource);
            CompileShader(fragmentShader);

            // These two shaders must then be merged into a shader program, which can then be used by OpenGL.
            // To do this, create a program...
            var handle = GL.CreateProgram();

            // Attach both shaders...
            GL.AttachShader(handle, vertexShader);
            GL.AttachShader(handle, fragmentShader);

            // And then link them together.
            LinkProgram(handle);

            // When the shader program is linked, it no longer needs the individual shaders attached to it; the compiled code is copied into the shader program.
            // Detach them, and then delete them.
            GL.DetachShader(handle, vertexShader);
            GL.DetachShader(handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            // The shader is now ready to go, but first, we're going to cache all the shader uniform locations.
            // Querying this from the shader is very slow, so we do it once on initialization and reuse those values
            // later.

            // First, we have to get the number of active uniforms in the shader.
            GL.GetProgram(handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
            GL.GetProgram(handle, GetProgramParameterName.ActiveUniformBlocks, out var numberOfUniformBlocks);

            // Next, allocate the dictionary to hold the locations.
            var uniformLocations = new Dictionary<string, int>();

            // Loop over all the uniforms,
            for (var i = 0; i < numberOfUniforms; i++)
            {
                // get the name of this uniform,
                var key = GL.GetActiveUniform(handle, i, out int size, out ActiveUniformType type);

                var arrayTokenIdx = key.IndexOf('[');
                if (arrayTokenIdx >= 1)
                    key = key.Substring(0, arrayTokenIdx);

                // get the location,
                var location = GL.GetUniformLocation(handle, key);

                // and then add it to the dictionary.
                uniformLocations.Add(key, location);
            }

            shader.Initialize(handle, uniformLocations);

            return shader;
        }

        #endregion Public Constructors

        #region Public Methods

        // A wrapper function that enables the shader program.
        public void Use(Shader shader)
        {
            GL.UseProgram(shader.Handle);
        }

        // The shader sources provided with this project use hardcoded layout(location)-s. If you want to do it dynamically,
        // you can omit the layout(location=X) lines in the vertex shader, and use this in VertexAttribPointer instead of the hardcoded values.
        public int GetAttribLocation(Shader shader, string attribName)
        {
            return GL.GetAttribLocation(shader.Handle, attribName);
        }

        /// <summary>
        /// Set a uniform int on this shader.
        /// </summary>
        /// <param name="name">The location of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetUInt(int location, uint data)
        {
            GL.Uniform1(location, data);
        }

        /// <summary>
        /// Set a uniform Matrix4 on this shader
        /// </summary>
        /// <param name="location">The location of the uniform</param>
        /// <param name="data">The data to set</param>
        /// <remarks>
        ///   <para>
        ///   The matrix is transposed before being sent to the shader.
        ///   </para>
        /// </remarks>
        public void SetMatrix4(int location, Matrix4 data)
        {
            GL.UniformMatrix4(location, true, ref data);
        }

        /// <summary>
        /// Set a uniform Vector3 on this shader.
        /// </summary>
        /// <param name="location">The location of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetVector4(int location, Vector4 data)
        {
            GL.Uniform4(location, data);
        }

        /// <summary>
        /// Set a uniform Vector4 on this shader.
        /// </summary>
        /// <param name="name">The location of the uniform</param>
        /// <param name="data">The data to set</param>
        public void SetVector4Array(int location, float[] data)
        {
            GL.Uniform4(location, data.Length / 4, data);
        }

        #endregion Public Methods

        #region Private Methods

        private static void CompileShader(int shader)
        {
            // Try to compile the shader
            GL.CompileShader(shader);

            // Check for compilation errors
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }

        private static void LinkProgram(int program)
        {
            // We link the program
            GL.LinkProgram(program);

            // Check for linking errors
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }

        #endregion Private Methods
    }
}