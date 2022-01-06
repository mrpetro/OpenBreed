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
             5.99f, 5.99f, 0.0f, // Bottom-left vertex
             5.99f, 0.01f, 0.0f, // Bottom-right vertex
             0.01f, 0.01f, 0.0f,  // Top vertex
             0.01f, 5.99f, 0.0f  // Top vertex
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

        private Shader _shader;

        private Matrix4 _view;
        private Matrix4 _projection;

        private Stack<Matrix4> modelMatrixStack = new Stack<Matrix4>();
        private int _unitBoxVbo;
        private int _unitBoxVao;
        private int _unitBoxEbo;
        private int _lineLoopEbo;

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
            _view = _view * transform;
            //GL.GL.MultMatrix(ref transform);
        }

        public void Translate(Vector3 vec)
        {
            _view = _view * Matrix4.CreateTranslation(vec);
        }

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

        public void DrawBox(Box2 clipBox)
        {
            //RenderTools.DrawBox(clipBox);
        }

        public void DrawUnitBox()
        {
            RenderTools.DrawUnitBox();
        }

        public void DrawSprite(int vbo, Matrix4 model)
        {
            DrawUnitBox(model, Color4.Green);
            return;

            _shader.SetVector4("aColor", new Vector4(1.0f, 0.0f, 1.0f, 1.0f));
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _view);
            _shader.SetMatrix4("projection", _projection);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.DrawArrays(PrimitiveType.LineLoop, 0, 4);
        }

        private void DrawRect(Matrix4 model, PrimitiveType primitiveType, int ebo, int indicesCount, Color4 color)
        {
            _shader.SetVector4("aColor", new Vector4(color.R, color.G, color.B, color.A));
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _view);
            _shader.SetMatrix4("projection", _projection);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.DrawElements(primitiveType, indicesCount, DrawElementsType.UnsignedInt, 0);
        }

        public void DrawUnitBox(Matrix4 model, Color4 color) => DrawRect(model, PrimitiveType.Triangles, _unitBoxEbo, _unitBoxIndices.Length, color);

        public void DrawBrightnessBox(float brightness)
        {
            //GL.GL.Enable(GL.EnableCap.Blend);
            //if (brightness > 1.0)
            //{
            //    GL.GL.BlendFunc(GL.BlendingFactor.DstColor, GL.BlendingFactor.One);
            //    GL.GL.Color3(brightness - 1, brightness - 1, brightness - 1);
            //}
            //else
            //{
            //    GL.GL.BlendFunc(GL.BlendingFactor.Zero, GL.BlendingFactor.SrcColor);
            //    GL.GL.Color3(brightness, brightness, brightness);
            //}

            //GL.GL.Translate(0, 0, BRIGHTNESS_Z_LEVEL);
            //DrawUnitBox();
            //GL.GL.Disable(GL.EnableCap.Blend);
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

        #endregion Public Methods

        #region Protected Methods

        private void SetupUnitBox()
        {
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

            //_shader.Use();

            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }

        public void Load()
        {
            //GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            _shader = new Shader("Shaders/shaderA.vert", "Shaders/shaderA.frag");
            _shader.Use();
            SetupUnitBox();

            _view = Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
        }

        public int CreateVertexArray(float[] vertices)
        {
            var vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            return vbo;
        }

        #endregion Protected Methods
    }
}