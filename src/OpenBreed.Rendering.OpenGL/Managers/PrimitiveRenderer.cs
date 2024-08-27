using OpenBreed.Core.Interface.Extensions;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
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
        private const int CIRCLE_PARTS_NO = 16;
        private const bool CLIPPING = true;
        private const int RENDER_MAX_DEPTH = 3;

        private Shader nontexturedShader;
        private Shader texturedShader;
        private Shader texturedWithPaletteShader;

        private int unitBoxFilledVao;
        private int unitRectangleFilledVao;
        private int unitCircleVao;
        private int unitCircleFilledVao;
        private int unitRectangleVao;
        private int unitLineVao;

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

        public void DrawBox(IRenderView view, Box2 box, Color4 color)
        {
            var w = box.Size.X;
            var h = box.Size.Y;
            var pos = new Vector3(box.Min.X, box.Min.Y, 0.0f);

            var model = Matrix4.CreateTranslation(pos);
            model = Matrix4.CreateTranslation(-w / 2.0f, -h / 2.0f, 0.0f) * model;
            model = Matrix4.CreateScale(w, h, 1.0f) * model;
            model = model * Matrix4.CreateTranslation(w / 2.0f, h / 2.0f, 0.0f);

            DrawUnitBox(view, model, color);
        }

        public void DrawBrightnessBox(IRenderView view, float brightness)
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

            view.Translate(0, 0, BRIGHTNESS_Z_LEVEL);
            DrawUnitBox(view, Matrix4.Identity, color);
            GL.Disable(EnableCap.Blend);
        }

        public void DrawRectangle(IRenderView view, Vector2 center, Vector2 size, Color4 color, bool filled = false)
        {
            var model = Matrix4.CreateTranslation(center.X, center.Y, 0.0f);
            model = Matrix4.CreateScale(size.X, size.Y, 1.0f) * model;

            DrawUnitRectangle(view, model, color, filled);
        }

        public void DrawRectangle(IRenderView view, Box2 rect, Color4 color, bool filled = false)
        {
            DrawRectangle(view, rect.Center, rect.Size, color, filled);
        }

        public void DrawNested(IRenderView view, Box2 clipBox, int depth, float dt, Action<Box2, int, float> nestedRenderAction)
        {
            RenderBefore(view, clipBox, depth, dt);

            if (depth > RENDER_MAX_DEPTH)
                return;

            depth++;

            nestedRenderAction.Invoke(clipBox, depth, dt);

            RenderAfter(view, clipBox, depth, dt);
        }

        public void DrawCircle(IRenderView view, Vector2 center, float radius, Color4 color, bool filled = false)
        {
            var model = Matrix4.CreateTranslation(center.X, center.Y, 0.0f);
            model = Matrix4.CreateScale(2.0f * radius, 2.0f * radius, 1.0f) * model;

            DrawUnitCircle(view, model, color, filled);
        }

        public void DrawLine(IRenderView view, Vector2 startPoint, Vector2 endPoint, Color4 color)
        {
            var model = Matrix4.CreateTranslation(startPoint.X, startPoint.Y, 0.0f);
            var uAxis = endPoint - startPoint;
            var angle = uAxis.CalculateAngle(Vector2.UnitX);
            var rotation = Matrix4.CreateFromAxisAngle(Vector3.UnitZ, angle); 

            model =  rotation * Matrix4.CreateScale(uAxis.X, uAxis.Y, 1.0f) * model;

            DrawUnitLine(view, model, color);
        }

        public void DrawPoint(IRenderView view, Vector2 pos, Color4 color, PointType type, float size = 2.0f)
        {
            var model = Matrix4.CreateTranslation(pos.X, pos.Y, 0.0f);
            model = Matrix4.CreateScale(size, size, 1.0f) * model;

            switch (type)
            {
                case PointType.Rectangle:
                    DrawUnitRectangle(view, model, color, filled: true);
                    break;

                case PointType.Circle:
                    DrawUnitCircle(view, model, color, filled: true);
                    break;

                case PointType.Cross:
                    DrawLine(view, new Vector2(pos.X - size, pos.Y), new Vector2(pos.X + size, pos.Y), color);
                    DrawLine(view, new Vector2(pos.X, pos.Y - size), new Vector2(pos.X, pos.Y + size), color);
                    break;
                case PointType.Ex:
                default:
                    throw new NotImplementedException($"Point type '{type}' not implemented.");
            }
        }

        public void DrawSprite(IRenderView view, ITexture texture, int vao, Vector3 pos, Vector2 scale, Color4 color)
        {
            ((Texture)texture).Use(TextureUnit.Texture0);

            var model = Matrix4.CreateScale(scale.X, scale.Y, 1.0f) * Matrix4.CreateTranslation(pos);

            if (texture.DataMode == TextureDataMode.Rgba)
            {
                texturedShader.Use();
                texturedShader.SetMatrix4("model", model);
                texturedShader.SetMatrix4("view", view.View);
                texturedShader.SetMatrix4("projection", view.Projection);
                texturedShader.SetVector4("aColor", ((Vector4)color));
            }
            else if (texture.DataMode == TextureDataMode.Index)
            {
                Debug.Assert(view.CurrentPalette is not null, "Palette is not set");

                texturedWithPaletteShader.Use();
                texturedWithPaletteShader.SetMatrix4("model", model);
                texturedWithPaletteShader.SetMatrix4("view", view.View);
                texturedWithPaletteShader.SetMatrix4("projection", view.Projection);
                texturedWithPaletteShader.SetVector4("aColor", ((Vector4)color));
                texturedWithPaletteShader.SetUInt("maskIndex", (uint)texture.MaskIndex);

                texturedWithPaletteShader.SetVector4Array("palette", view.CurrentPalette.DirectData);
            }
            else
            {
                throw new Exception($"Data mode '{texture.DataMode}' is not implemented.");
            }

            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);
        }

        public void DrawUnitBox(IRenderView view, Matrix4 model, Color4 color)
        {
            nontexturedShader.Use();
            nontexturedShader.SetVector4("aColor", new Vector4(color.R, color.G, color.B, color.A));
            nontexturedShader.SetMatrix4("model", model);
            nontexturedShader.SetMatrix4("view", view.View);
            nontexturedShader.SetMatrix4("projection", view.Projection);

            GL.BindVertexArray(unitBoxFilledVao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);
        }

        public void DrawUnitLine(IRenderView view, Matrix4 model, Color4 color)
        {
            nontexturedShader.Use();
            nontexturedShader.SetVector4("aColor", new Vector4(color.R, color.G, color.B, color.A));
            nontexturedShader.SetMatrix4("model", model);
            nontexturedShader.SetMatrix4("view", view.View);
            nontexturedShader.SetMatrix4("projection", view.Projection);

            GL.BindVertexArray(unitLineVao);
            GL.DrawArrays(PrimitiveType.Lines, 0, 2);
            GL.BindVertexArray(0);
        }

        public void DrawUnitCircle(IRenderView view, Matrix4 model, Color4 color, bool filled = false)
        {
            nontexturedShader.Use();
            nontexturedShader.SetVector4("aColor", new Vector4(color.R, color.G, color.B, color.A));
            nontexturedShader.SetMatrix4("model", model);
            nontexturedShader.SetMatrix4("view", view.View);
            nontexturedShader.SetMatrix4("projection", view.Projection);

            if (filled)
            {
                GL.BindVertexArray(unitCircleFilledVao);
                GL.DrawArrays(PrimitiveType.TriangleFan, 0, CIRCLE_PARTS_NO + 2);
            }
            else
            {
                GL.BindVertexArray(unitCircleVao);
                GL.DrawArrays(PrimitiveType.LineLoop, 0, CIRCLE_PARTS_NO);
            }

            GL.BindVertexArray(0);
        }

        public void DrawUnitRectangle(IRenderView view, Matrix4 model, Color4 color, bool filled = false)
        {
            nontexturedShader.Use();
            nontexturedShader.SetVector4("aColor", new Vector4(color.R, color.G, color.B, color.A));
            nontexturedShader.SetMatrix4("model", model);
            nontexturedShader.SetMatrix4("view", view.View);
            nontexturedShader.SetMatrix4("projection", view.Projection);

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

        public void Load()
        {
            SetupShaders();
            SetupDefaultVertices();
        }

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

        private void RenderAfter(IRenderView view, Box2 clipBox, int depth, float dt)
        {
            if (CLIPPING)
            {
                GL.ColorMask(false, false, false, false);
                GL.DepthMask(false);
                GL.StencilFunc(StencilFunction.Always, depth, depth);
                GL.StencilOp(StencilOp.Decr, StencilOp.Decr, StencilOp.Decr);

                // Draw black box
                DrawBox(view, clipBox, Color4.Black);

                GL.ColorMask(true, true, true, true);
                GL.DepthMask(true);

                if (depth == 1)
                    GL.Disable(EnableCap.StencilTest);
            }
        }

        private void RenderBefore(IRenderView view, Box2 clipBox, int depth, float dt)
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
                DrawBox(view, clipBox, Color4.Black);

                GL.ColorMask(true, true, true, true);
                GL.DepthMask(true);
                GL.StencilFunc(StencilFunction.Equal, depth, depth);
                GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
            }
        }

        private void SetupDefaultVertices()
        {
            SetupUnitLineVertices();

            SetupUnitBoxVertices();

            var unitRectangleBuilder = CreatePosArray();
            unitRectangleBuilder.AddVertex(0.5f, 0.5f, 0.0f);
            unitRectangleBuilder.AddVertex(0.5f, -0.5f, 0.0f);
            unitRectangleBuilder.AddVertex(-0.5f, -0.5f, 0.0f);
            unitRectangleBuilder.AddVertex(-0.5f, 0.5f, 0.0f);
            unitRectangleBuilder.AddLoopIndices(0, 1, 2, 3);

            unitRectangleVao = unitRectangleBuilder.CreateVao();

            unitRectangleBuilder.ClearLoopIndices();

            unitRectangleBuilder.AddTriangleIndices(0, 1, 3);
            unitRectangleBuilder.AddTriangleIndices(1, 2, 3);

            unitRectangleFilledVao = unitRectangleBuilder.CreateVao();

            var unitCircleBuilder = CreatePosArray();

            unitCircleBuilder.AddVertex(0.0f, 0.0f, 0.0f);

            for (int i = 0; i < CIRCLE_PARTS_NO; i++)
            {
                var step = (float)i / CIRCLE_PARTS_NO;

                var x = 0.5f * (float)Math.Cos(2 * Math.PI * step);
                var y = 0.5f * (float)Math.Sin(2 * Math.PI * step);

                unitCircleBuilder.AddVertex(x, y, 0.0f);
            }

            unitCircleBuilder.AddLoopIndices(Enumerable.Range(1, CIRCLE_PARTS_NO).ToArray());

            unitCircleVao = unitCircleBuilder.CreateVao();

            unitCircleBuilder.ClearLoopIndices();

            unitCircleBuilder.AddLoopIndices(Enumerable.Range(0, CIRCLE_PARTS_NO + 1).Append(1).ToArray());

            unitCircleFilledVao = unitCircleBuilder.CreateVao();
        }

        private void SetupUnitLineVertices()
        {
            var unitLineBuilder = CreatePosArray();
            unitLineBuilder.AddVertex(0.0f, 0.0f, 0.0f);
            unitLineBuilder.AddVertex(1.0f, 0.0f, 0.0f);
            unitLineBuilder.AddLoopIndices(0, 1);

            unitLineVao = unitLineBuilder.CreateVao();
        }

        private void SetupUnitBoxVertices()
        {
            var unitBoxBuilder = CreatePosArray();
            unitBoxBuilder.AddVertex(1.0f, 1.0f, 0.0f);
            unitBoxBuilder.AddVertex(1.0f, 0.0f, 0.0f);
            unitBoxBuilder.AddVertex(0.0f, 0.0f, 0.0f);
            unitBoxBuilder.AddVertex(0.0f, 1.0f, 0.0f);

            unitBoxBuilder.AddTriangleIndices(0, 1, 3);
            unitBoxBuilder.AddTriangleIndices(1, 2, 3);

            unitBoxFilledVao = unitBoxBuilder.CreateVao();
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