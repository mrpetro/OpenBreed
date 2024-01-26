using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class PrimitiveRenderer : IPrimitiveRenderer
    {
        #region Private Fields

        private const float BRIGHTNESS_Z_LEVEL = 50.0f;

        private const bool CLIPPING = true;
        private const int RENDER_MAX_DEPTH = 3;
        private IPalette currentPalette;
        private Stack<Matrix4> modelMatrixStack = new Stack<Matrix4>();
        private Shader nontexturedShader;
        private Stack<IPalette> paletteStack = new Stack<IPalette>();
        private Matrix4 projection;
        private Shader texturedShader;
        private Shader texturedWithPaletteShader;

        private int unitBoxFilledVao;
        private int unitRectangleFilledVao;
        private int unitCircleVao;
        private int unitCircleFilledVao;
        private int unitRectangleVao;
        private Matrix4 view;

        #endregion Private Fields

        #region Public Constructors

        public PrimitiveRenderer()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public IPosArrayBuilder CreatePosArray() => new PosArrayBuilder(this);

        public IPosTexCoordArrayBuilder CreatePosTexCoordArray() => new PosTexCoordArrayBuilder(this);

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

        public void DisableAlpha()
        {
            OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
            OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.AlphaTest);
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

        public void DrawRectangle(Vector2 center, Vector2 size, Color4 color, bool filled = false)
        {
            var model = Matrix4.CreateTranslation(center.X, center.Y, 0.0f);
            model = Matrix4.CreateScale(size.X, size.Y, 1.0f) * model;

            DrawUnitRectangle(model, color, filled);
        }

        public void DrawRectangle(Box2 rect, Color4 color, bool filled = false)
        {
            DrawRectangle(rect.Center, rect.Size, color, filled);
        }

        public void DrawNested(Box2 clipBox, int depth, float dt, Action<Box2, int, float> nestedRenderAction)
        {
            RenderBefore(clipBox, depth, dt);

            if (depth > RENDER_MAX_DEPTH)
                return;

            depth++;

            nestedRenderAction.Invoke(clipBox, depth, dt);

            RenderAfter(clipBox, depth, dt);
        }

        public void DrawCircle(Vector2 center, float radius, Color4 color, bool filled = false)
        {
            var model = Matrix4.CreateTranslation(center.X, center.Y, 0.0f);
            model = Matrix4.CreateScale(radius, radius, 1.0f) * model;

            DrawUnitCircle(model, color, filled);
        }

        public void DrawPoint(Vector2 pos, Color4 color, PointType type)
        {
            var w = 2.0f;
            var h = 2.0f;
            var model = Matrix4.CreateTranslation(pos.X, pos.Y, 0.0f);
            model = Matrix4.CreateScale(w, h, 1.0f) * model;

            switch (type)
            {
                case PointType.Rectangle:
                    DrawUnitRectangle(model, color, filled: true);
                    break;
                case PointType.Circle:
                    DrawUnitCircle(model, color, filled: true);
                    break;
                case PointType.Cross:
                case PointType.Ex:
                default:
                    throw new NotImplementedException($"Point type '{type}' not implemented.");
            }
        }

        public void DrawSprite(ITexture texture, int vao, Vector3 pos, Vector2 scale, Color4 color)
        {
            ((Texture)texture).Use(TextureUnit.Texture0);

            var model = Matrix4.CreateScale(scale.X, scale.Y, 1.0f) * Matrix4.CreateTranslation(pos);

            if (texture.DataMode == TextureDataMode.Rgba)
            {
                texturedShader.Use();
                texturedShader.SetMatrix4("model", model);
                texturedShader.SetMatrix4("view", view);
                texturedShader.SetMatrix4("projection", projection);
                texturedShader.SetVector4("aColor", ((Vector4)color));
            }
            else if (texture.DataMode == TextureDataMode.Index)
            {
                Debug.Assert(currentPalette is not null, "Palette is not set");

                texturedWithPaletteShader.Use();
                texturedWithPaletteShader.SetMatrix4("model", model);
                texturedWithPaletteShader.SetMatrix4("view", view);
                texturedWithPaletteShader.SetMatrix4("projection", projection);
                texturedWithPaletteShader.SetVector4("aColor", ((Vector4)color));
                texturedWithPaletteShader.SetUInt("maskIndex", (uint)texture.MaskIndex);

                texturedWithPaletteShader.SetVector4Array("palette", currentPalette.DirectData);
            }
            else
            {
                throw new Exception($"Data mode '{texture.DataMode}' is not implemented.");
            }

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

            GL.BindVertexArray(unitBoxFilledVao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);
        }

        public void DrawUnitCircle(Matrix4 model, Color4 color, bool filled = false)
        {
            nontexturedShader.Use();
            nontexturedShader.SetVector4("aColor", new Vector4(color.R, color.G, color.B, color.A));
            nontexturedShader.SetMatrix4("model", model);
            nontexturedShader.SetMatrix4("view", view);
            nontexturedShader.SetMatrix4("projection", projection);

            if (filled)
            {
                GL.BindVertexArray(unitCircleFilledVao);
                GL.DrawArrays(PrimitiveType.TriangleFan, 0, 18);
            }
            else
            {
                GL.BindVertexArray(unitCircleVao);
                GL.DrawArrays(PrimitiveType.LineLoop, 0, 16);
            }

            GL.BindVertexArray(0);
        }

        public void DrawUnitRectangle(Matrix4 model, Color4 color, bool filled = false)
        {
            nontexturedShader.Use();
            nontexturedShader.SetVector4("aColor", new Vector4(color.R, color.G, color.B, color.A));
            nontexturedShader.SetMatrix4("model", model);
            nontexturedShader.SetMatrix4("view", view);
            nontexturedShader.SetMatrix4("projection", projection);

            if (filled)
            {
                GL.BindVertexArray(unitRectangleFilledVao);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            }
            else
            {
                GL.BindVertexArray(unitRectangleVao);
                GL.DrawArrays(PrimitiveType.LineLoop, 0, 4);
            }

            GL.BindVertexArray(0);
        }

        public void EnableAlpha()
        {
            OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.AlphaTest);
            OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
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

        public void Load()
        {
            SetupShaders();
            SetupDefaultVertices();

            view = Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
        }

        public void MultMatrix(Matrix4 transform)
        {
            view = transform * view;
        }

        public void PopMatrix()
        {
            view = modelMatrixStack.Pop();
        }

        public void PopPalette()
        {
            currentPalette = paletteStack.Pop();
        }

        public void PushMatrix()
        {
            modelMatrixStack.Push(view);
        }

        public void PushPalette()
        {
            paletteStack.Push(currentPalette);
        }

        public void SetPalette(IPalette palette)
        {
            currentPalette = palette;
        }

        public void SetProjection(Matrix4 matrix)
        {
            projection = matrix;
        }

        public void SetView(Matrix4 matrix)
        {
            view = matrix;
        }

        public void Translate(Vector3 vec)
        {
            view = Matrix4.CreateTranslation(vec) * view;
        }

        public void Translate(float x, float y, float z) => Translate(new Vector3(x, y, z));

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

        private void RenderAfter(Box2 clipBox, int depth, float dt)
        {
            if (CLIPPING)
            {
                GL.ColorMask(false, false, false, false);
                GL.DepthMask(false);
                GL.StencilFunc(StencilFunction.Always, depth, depth);
                GL.StencilOp(StencilOp.Decr, StencilOp.Decr, StencilOp.Decr);

                // Draw black box
                DrawBox(clipBox, Color4.Black);

                GL.ColorMask(true, true, true, true);
                GL.DepthMask(true);

                if (depth == 1)
                    GL.Disable(EnableCap.StencilTest);
            }
        }

        private void RenderBefore(Box2 clipBox, int depth, float dt)
        {
            if (CLIPPING)
            {
                //Enable stencil buffer
                if (depth == 1)
                    GL.Enable(EnableCap.StencilTest);

                GL.ColorMask(false, false, false, false);
                GL.DepthMask(false);
                GL.StencilFunc(StencilFunction.Always, depth, depth);
                GL.StencilOp(StencilOp.Incr, StencilOp.Incr, StencilOp.Incr);

                // Draw black box
                DrawBox(clipBox, Color4.Black);

                GL.ColorMask(true, true, true, true);
                GL.DepthMask(true);
                GL.StencilFunc(StencilFunction.Equal, depth, depth);
                GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
            }
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

            unitBoxFilledVao = unitBoxBuilder.CreateVao();

            var unitRectangleBuilder = CreatePosArray();
            unitRectangleBuilder.AddVertex( 0.5f,  0.5f, 0.0f);
            unitRectangleBuilder.AddVertex( 0.5f, -0.5f, 0.0f);
            unitRectangleBuilder.AddVertex(-0.5f, -0.5f, 0.0f);
            unitRectangleBuilder.AddVertex(-0.5f,  0.5f, 0.0f);
            unitRectangleBuilder.AddLoopIndices(0, 1, 2, 3);

            unitRectangleVao = unitRectangleBuilder.CreateVao();

            unitRectangleBuilder.ClearLoopIndices();

            unitRectangleBuilder.AddTriangleIndices(0, 1, 3);
            unitRectangleBuilder.AddTriangleIndices(1, 2, 3);

            unitRectangleFilledVao = unitRectangleBuilder.CreateVao();

            var unitCircleBuilder = CreatePosArray();

            unitCircleBuilder.AddVertex(0.0f, 0.0f, 0.0f);

            for (int i = 0; i < 16; i++)
            {
                var step = (float)i / 16;

                var x = 0.5f * (float)Math.Cos(2 * Math.PI * step);
                var y = 0.5f * (float)Math.Sin(2 * Math.PI * step);

                unitCircleBuilder.AddVertex(x, y, 0.0f);
            }

            unitCircleBuilder.AddLoopIndices(Enumerable.Range(1, 16).ToArray());

            unitCircleVao = unitCircleBuilder.CreateVao();

            unitCircleBuilder.ClearLoopIndices();

            unitCircleBuilder.AddLoopIndices(Enumerable.Range(0, 17).Append(1).ToArray());

            unitCircleFilledVao = unitCircleBuilder.CreateVao();
        }

        private void SetupShaders()
        {
            texturedShader = new Shader("Shaders/textured.vert", "Shaders/textured.frag");
            texturedShader.Use();
            texturedWithPaletteShader = new Shader("Shaders/textured.vert", "Shaders/texturedWithPalette.frag");
            texturedWithPaletteShader.Use();
            nontexturedShader = new Shader("Shaders/nontextured.vert", "Shaders/nontextured.frag");
            nontexturedShader.Use();
        }

        #endregion Private Methods
    }
}