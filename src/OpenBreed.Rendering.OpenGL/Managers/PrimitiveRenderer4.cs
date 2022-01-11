using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class PrimitiveRenderer4 : IPrimitiveRenderer
    {
        #region Private Fields

        private const float BRIGHTNESS_Z_LEVEL = 50.0f;

        private readonly float[] _unitBoxVertices =
        {
             0.99f, 0.99f, 0.0f, // Bottom-left vertex
             0.99f, 0.01f, 0.0f, // Bottom-right vertex
             0.01f, 0.01f, 0.0f,  // Top vertex
             0.01f, 0.99f, 0.0f  // Top vertex
        };

        private readonly uint[] _unitBoxIndices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private readonly uint[] _lineLoopIndices =
        {
            0, 1, 2, 3
        };
        private int quadVao;
        int spriteVbo;

        private float[] spriteVertices =
        { 
            // pos            // tex
            0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 0.0f, 0.0f, 0.0f,

            0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
            1.0f, 1.0f, 0.0f, 1.0f, 1.0f,
            1.0f, 0.0f, 0.0f, 1.0f, 0.0f
        };

        private Shader _defaultshader;

        private Matrix4 _view;
        private Matrix4 _projection;

        private Stack<Matrix4> modelMatrixStack = new Stack<Matrix4>();
        private int _unitBoxVbo;
        private int _unitBoxVao;
        private int _unitBoxEbo;
        private int _lineLoopEbo;
        private Shader _spriteShader;

        #endregion Private Fields

        #region Public Constructors

        public PrimitiveRenderer4()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void PushMatrix()
        {
            modelMatrixStack.Push(_view);
            //GL.GL.PushMatrix();
        }

        public void PopMatrix()
        {
            _view = modelMatrixStack.Pop();
            //GL.GL.PopMatrix();
        }

        public void MultMatrix(Matrix4 transform)
        {
            _view = transform * _view;
            //GL.GL.MultMatrix(ref transform);
        }

        public void Translate(Vector3 vec)
        {
            _view = Matrix4.CreateTranslation(vec) * _view;
        }

        public void Translate(float x, float y, float z) => Translate(new Vector3(x, y, z));

        public void DrawUnitRectangle()
        {
            RenderTools.DrawUnitRectangle();
        }

        public void DrawUnitRectangle(Matrix4 model, Color4 color) => DrawRect(model, PrimitiveType.LineLoop, _lineLoopEbo, _lineLoopIndices.Length, color);

        public void DrawRectangle(Box2 clipBox)
        {
            RenderTools.DrawRectangle(clipBox);
        }

        public void DrawRectangle(Box2 clipBox, Color4 color)
        {
        }

        public void DrawBox(Box2 clipBox, Color4 color)
        {
            var w = clipBox.Size.X;
            var h = clipBox.Size.Y;
            var pos = new Vector3(clipBox.Min.X, clipBox.Min.Y, 0.0f);

            var model = Matrix4.CreateTranslation(pos);
            model = Matrix4.CreateTranslation(-w / 2.0f, -h / 2.0f, 0.0f) * model;
            model = Matrix4.CreateScale(w, h, 1.0f) * model;
            model = model * Matrix4.CreateTranslation(w / 2.0f, h / 2.0f, 0.0f);

            DrawRect(model, PrimitiveType.Triangles, _unitBoxEbo, _unitBoxIndices.Length, color);

            //RenderTools.DrawBox(clipBox);
        }

        public void DrawUnitBox()
        {
            RenderTools.DrawUnitBox();
        }

        public void DrawSprite(ITexture texture, Vector3 pos, Vector2 size)
        {
            //texture = demoTexture;

            ((Texture)texture).Use(TextureUnit.Texture0);
            _spriteShader.Use();

            var w = size.X;
            var h = size.Y;

            var model = Matrix4.CreateTranslation(pos);
            model = Matrix4.CreateTranslation(- w / 2.0f, - h / 2.0f, 0.0f) * model;
            model = Matrix4.CreateScale(w, h, 1.0f) * model;
            model = model * Matrix4.CreateTranslation(w / 2.0f, h / 2.0f, 0.0f);

            _spriteShader.SetMatrix4("model", model);
            _spriteShader.SetMatrix4("view", _view);
            _spriteShader.SetMatrix4("projection", _projection);
            //_spriteShader.SetVector3("spriteColor", new Vector3(1.0f, 1.0f, 0.0f));

            GL.BindVertexArray(quadVao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);
        }

        public void DrawSprite(ITexture texture, int vao, Vector3 pos, Vector2 size)
        {
            //texture = demoTexture;

            ((Texture)texture).Use(TextureUnit.Texture0);
            _spriteShader.Use();

            var model = Matrix4.CreateTranslation(pos);

            _spriteShader.SetMatrix4("model", model);
            _spriteShader.SetMatrix4("view", _view);
            _spriteShader.SetMatrix4("projection", _projection);
            //_spriteShader.SetVector3("spriteColor", new Vector3(1.0f, 1.0f, 0.0f));

            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);
        }

        private void DrawRect(Matrix4 model, PrimitiveType primitiveType, int ebo, int indicesCount, Color4 color)
        {
            _defaultshader.Use();
            _defaultshader.SetVector4("aColor", new Vector4(color.R, color.G, color.B, color.A));
            _defaultshader.SetMatrix4("model", model);
            _defaultshader.SetMatrix4("view", _view);
            _defaultshader.SetMatrix4("projection", _projection);

            GL.BindVertexArray(_unitBoxVao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.DrawElements(primitiveType, indicesCount, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

        public void DrawUnitBox(Matrix4 model, Color4 color) => DrawRect(model, PrimitiveType.Triangles, _unitBoxEbo, _unitBoxIndices.Length, color);

        public void DrawBrightnessBox(float brightness)
        {
            Color4 color;

            GL.Enable(EnableCap.Blend);
            if (brightness > 1.0)
            {
                GL.BlendFunc(BlendingFactor.DstColor, BlendingFactor.One);
                color = new Color4(brightness - 1, brightness - 1, brightness - 1, 1.0f);
            }
            else
            {
                GL.BlendFunc(BlendingFactor.Zero, BlendingFactor.SrcColor);
                color = new Color4(brightness, brightness, brightness, 1.0f);
            }

            Translate(0, 0, BRIGHTNESS_Z_LEVEL);
            DrawUnitBox(Matrix4.Identity, color);
            GL.Disable(EnableCap.Blend);
        }

        public void DrawTriangle()
        {
            //GL.Clear(ClearBufferMask.ColorBufferBit);

            //var model = Matrix4.Identity;

            //_shader.Use();

            //_shader.SetMatrix4("model", model);
            //_shader.SetMatrix4("view", _view);
            //_shader.SetMatrix4("projection", _projection);

            //GL.BindVertexArray(_vertexArrayObject);
            //GL.DrawArrays(PrimitiveType.LineLoop, 0, 3);
        }

        public void SetProjection(Matrix4 matrix)
        {
            _projection = matrix;
        }

        public void SetView(Matrix4 matrix)
        {
            _view = matrix;
        }

        #endregion Public Methods

        #region Protected Methods

        private Texture demoTexture;

        //public void SetupSprites()
        //{
        //    demoTexture = Texture4.LoadFromFile($"Shaders/container.png");

        //    _spriteShader = new Shader("Shaders/sprite.vert", "Shaders/sprite.frag");
        //    _spriteShader.Use();

        //    quadVao = GL.GenVertexArray();
        //    spriteVbo = GL.GenBuffer();

        //    GL.BindBuffer(BufferTarget.ArrayBuffer, spriteVbo);
        //    GL.BufferData(BufferTarget.ArrayBuffer, spriteVertices.Length * sizeof(float), spriteVertices, BufferUsageHint.StaticDraw);

        //    GL.BindVertexArray(quadVao);
        //    GL.EnableVertexAttribArray(0);
        //    var vertexLocation = _spriteShader.GetAttribLocation("aPosition");
        //    GL.EnableVertexAttribArray(vertexLocation);
        //    GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        //    var texCoordLocation = _spriteShader.GetAttribLocation("aTexCoord");
        //    GL.EnableVertexAttribArray(texCoordLocation);
        //    GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));


        //    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        //    GL.BindVertexArray(0);
        //}

        private void SetupSprites()
        {
            demoTexture = Texture.LoadFromFile($"Shaders/container.png");

            _spriteShader = new Shader("Shaders/sprite.vert", "Shaders/sprite.frag");
            _spriteShader.Use();

            quadVao = GL.GenVertexArray();
            spriteVbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, spriteVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, spriteVertices.Length * sizeof(float), spriteVertices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(quadVao);
            GL.EnableVertexAttribArray(0);
            var vertexLocation = _spriteShader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = _spriteShader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));


            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        private void SetupUnitBox()
        {   
            _defaultshader.Use();

            _unitBoxVbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _unitBoxVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _unitBoxVertices.Length * sizeof(float), _unitBoxVertices, BufferUsageHint.StaticDraw);

            _unitBoxEbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _unitBoxEbo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _unitBoxIndices.Length * sizeof(uint), _unitBoxIndices, BufferUsageHint.StaticDraw);

            _lineLoopEbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _lineLoopEbo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _lineLoopIndices.Length * sizeof(uint), _lineLoopIndices, BufferUsageHint.StaticDraw);

            _unitBoxVao = GL.GenVertexArray();

            GL.BindVertexArray(_unitBoxVao);
            var vertexLocation = _defaultshader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.BindVertexArray(0);
        }

        public void Load()
        {
            //GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            _defaultshader = new Shader("Shaders/shaderA.vert", "Shaders/shaderA.frag");
            _defaultshader.Use();

            SetupUnitBox();
            SetupSprites();

            _view = Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
        }

        public int CreateVertexArray(float[] vertices)
        {
            var vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            return vbo;
        }

        public int CreateVao(float[] vertexArray)
        {
            _spriteShader.Use();

            var newVao = GL.GenVertexArray();
            var newVbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, newVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexArray.Length * sizeof(float), vertexArray, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(newVao);
            GL.EnableVertexAttribArray(0);
            var vertexLocation = _spriteShader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = _spriteShader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            return newVao;
        }

        #endregion Protected Methods
    }
}