using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class PrimitiveRenderer : IPrimitiveRenderer
    {
        #region Private Fields

        private const float BRIGHTNESS_Z_LEVEL = 50.0f;

        private Shader nontexturedShader;

        private Matrix4 view;
        private Matrix4 projection;

        private Stack<Matrix4> modelMatrixStack = new Stack<Matrix4>();

        private Shader texturedShader;

        private int unitBoxVao;
        private int unitRectVao;

        #endregion Private Fields

        #region Public Constructors

        public PrimitiveRenderer()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void PushMatrix()
        {
            modelMatrixStack.Push(view);
        }

        public void PopMatrix()
        {
            view = modelMatrixStack.Pop();
        }

        public void MultMatrix(Matrix4 transform)
        {
            view = transform * view;
        }

        public void Translate(Vector3 vec)
        {
            view = Matrix4.CreateTranslation(vec) * view;
        }

        public void Translate(float x, float y, float z) => Translate(new Vector3(x, y, z));

        public void DrawUnitRectangle(Matrix4 model, Color4 color)
        {
            nontexturedShader.Use();
            nontexturedShader.SetVector4("aColor", new Vector4(color.R, color.G, color.B, color.A));
            nontexturedShader.SetMatrix4("model", model);
            nontexturedShader.SetMatrix4("view", view);
            nontexturedShader.SetMatrix4("projection", projection);

            GL.BindVertexArray(unitRectVao);
            GL.DrawArrays(PrimitiveType.LineLoop, 0, 4);
            GL.BindVertexArray(0);
        }

        public void DrawRectangle(Box2 rect, Color4 color)
        {
            var w = rect.Size.X;
            var h = rect.Size.Y;
            var pos = new Vector3(rect.Min.X, rect.Min.Y, 0.0f);

            var model = Matrix4.CreateTranslation(pos);
            model = Matrix4.CreateTranslation(-w / 2.0f, -h / 2.0f, 0.0f) * model;
            model = Matrix4.CreateScale(w, h, 1.0f) * model;
            model = model * Matrix4.CreateTranslation(w / 2.0f, h / 2.0f, 0.0f);

            DrawUnitRectangle(model, color);
        }

        public void DrawPoint(Vector4 pos, Color4 color)
        {
            var w = 5.0f;
            var h = 5.0f;
            var pos3 = new Vector3(pos.X, pos.Y, pos.Z);

            var model = Matrix4.CreateTranslation(pos3);
            model = Matrix4.CreateTranslation(-w / 2.0f, -h / 2.0f, 0.0f) * model;
            model = Matrix4.CreateScale(w, h, 1.0f) * model;
            model = model * Matrix4.CreateTranslation(w / 2.0f, h / 2.0f, 0.0f);

            DrawUnitBox(model, color);
        }

        public void DrawBox(Box2 box, Color4 color)
        {
            var w = box.Size.X;
            var h = box.Size.Y;
            var pos = new Vector3(box.Min.X, box.Min.Y, 0.0f);

            var model = Matrix4.CreateTranslation(pos);
            model = Matrix4.CreateTranslation(-w / 2.0f, -h / 2.0f, 0.0f) * model;
            model = Matrix4.CreateScale(w, h, 1.0f) * model;
            model = model * Matrix4.CreateTranslation(w / 2.0f, h / 2.0f, 0.0f);

            DrawUnitBox(model, color);
        }

        public void DrawSprite(ITexture texture, int vao, Vector3 pos, Vector2 size, Color4 color)
        {
            ((Texture)texture).Use(TextureUnit.Texture0);
            texturedShader.Use();

            var model = Matrix4.CreateTranslation(pos);

            texturedShader.SetMatrix4("model", model);
            texturedShader.SetMatrix4("view", view);
            texturedShader.SetMatrix4("projection", projection);
            texturedShader.SetVector4("aColor", ((Vector4)color));//; new Vector4(1.0f, 0.0f, 0.0f, 1.0f)); ;

            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);
        }

        public void DrawUnitBox(Matrix4 model, Color4 color)
        {
            nontexturedShader.Use();
            nontexturedShader.SetVector4("aColor", new Vector4(color.R, color.G, color.B, color.A));
            nontexturedShader.SetMatrix4("model", model);
            nontexturedShader.SetMatrix4("view", view);
            nontexturedShader.SetMatrix4("projection", projection);

            GL.BindVertexArray(unitBoxVao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);
        }

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

        public void SetProjection(Matrix4 matrix)
        {
            projection = matrix;
        }

        public void SetView(Matrix4 matrix)
        {
            view = matrix;
        }

        public void Load()
        {
            SetupShaders();
            SetupDefaultVertices();

            view = Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
        }

        public int CreateVao(float[] vertexArray)
        {
            nontexturedShader.Use();

            var newVao = GL.GenVertexArray();
            var newVbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, newVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexArray.Length * sizeof(float), vertexArray, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(newVao);
            GL.EnableVertexAttribArray(0);
            var vertexLocation = nontexturedShader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            return newVao;
        }

        public Vector4 GetScreenToWorldCoords(Vector4 coords)
        {
            var mat = view * projection;
            mat.Invert();

            var coordsT = coords * mat;
            coordsT.W = 1.0f / coordsT.W;

            coordsT.X *= coordsT.W;
            coordsT.Y *= coordsT.W;
            coordsT.Z *= coordsT.W;

            return coordsT;
        }

        public IPosArrayBuilder CreatePosArray() => new PosArrayBuilder(this);

        public IPosTexCoordArrayBuilder CreatePosTexCoordArray() => new PosTexCoordArrayBuilder(this);

        #endregion Public Methods

        #region Internal Methods

        internal int CreateTexturedVao(float[] vertexArray)
        {
            texturedShader.Use();

            var newVao = GL.GenVertexArray();
            var newVbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, newVbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertexArray.Length * sizeof(float), vertexArray, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(newVao);
            GL.EnableVertexAttribArray(0);
            var vertexLocation = texturedShader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = texturedShader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            return newVao;
        }

        #endregion Internal Methods

        #region Private Methods

        private void SetupShaders()
        {
            texturedShader = new Shader("Shaders/textured.vert", "Shaders/textured.frag");
            texturedShader.Use();
            nontexturedShader = new Shader("Shaders/nontextured.vert", "Shaders/nontextured.frag");
            nontexturedShader.Use();
        }

        private void SetupDefaultVertices()
        {
            var unitBoxBuilder = CreatePosArray();
            unitBoxBuilder.AddVertex(1.0f, 1.0f, 0.0f);
            unitBoxBuilder.AddVertex(1.0f, 0.0f, 0.0f);
            unitBoxBuilder.AddVertex(0.0f, 0.0f, 0.0f);
            unitBoxBuilder.AddVertex(0.0f, 1.0f, 0.0f);

            unitBoxBuilder.AddTriangleIndices(0, 1, 3);
            unitBoxBuilder.AddTriangleIndices(1, 2, 3);

            unitBoxVao = unitBoxBuilder.CreateVao();

            var unitRectBuilder = CreatePosArray();
            unitRectBuilder.AddVertex(1.0f, 1.0f, 0.0f);
            unitRectBuilder.AddVertex(1.0f, 0.0f, 0.0f);
            unitRectBuilder.AddVertex(0.0f, 0.0f, 0.0f);
            unitRectBuilder.AddVertex(0.0f, 1.0f, 0.0f);
            unitRectBuilder.AddLoopIndices(0, 1, 2, 3);

            unitRectVao = unitRectBuilder.CreateVao();
        }

        #endregion Private Methods
    }
}