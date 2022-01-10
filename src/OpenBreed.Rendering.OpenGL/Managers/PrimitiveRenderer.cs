using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.OpenGL.Helpers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using GL = OpenTK.Graphics.OpenGL;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    internal class PrimitiveRenderer : IPrimitiveRenderer
    {
        #region Private Fields

        private const float BRIGHTNESS_Z_LEVEL = 50.0f;

        #endregion Private Fields

        #region Public Methods

        public void PushMatrix()
        {
            GL.GL.PushMatrix();
        }

        public void PopMatrix()
        {
            GL.GL.PopMatrix();
        }

        public void MatrixMode(MatrixMode matrixMode)
        {
            switch (matrixMode)
            {
                case Interface.MatrixMode.ModelView:
                    GL.GL.MatrixMode(GL.MatrixMode.Modelview);
                    break;
                case Interface.MatrixMode.Projection:
                    GL.GL.MatrixMode(GL.MatrixMode.Projection);
                    break;
                case Interface.MatrixMode.Texture:
                    GL.GL.MatrixMode(GL.MatrixMode.Texture);
                    break;
                case Interface.MatrixMode.Color:
                    GL.GL.MatrixMode(GL.MatrixMode.Color);
                    break;
                default:
                    break;
            }
        }

        public void DrawUnitRectangle()
        {
            RenderTools.DrawUnitRectangle();
        }

        public void DrawUnitRectangle(Matrix4 model, Color4 color)
        {
            GL.GL.Color4(color);
            RenderTools.DrawUnitRectangle();
        }

        public void DrawRectangle(Box2 clipBox)
        {
            RenderTools.DrawRectangle(clipBox);
        }

        public void DrawRectangle(Box2 clipBox, Color4 color)
        {
            GL.GL.Color4(color);
            RenderTools.DrawRectangle(clipBox);
        }

        public void DrawBox(Box2 clipBox, Color4 color)
        {
            RenderTools.DrawBox(clipBox);
        }

        public void DrawUnitBox()
        {
            RenderTools.DrawUnitBox();
        }

        public void DrawUnitBox(Matrix4 model, Color4 color)
        {
            GL.GL.Color4(color);
            RenderTools.DrawUnitBox();
        }

        public void DrawBrightnessBox(float brightness)
        {
            GL.GL.Enable(GL.EnableCap.Blend);
            if (brightness > 1.0)
            {
                GL.GL.BlendFunc(GL.BlendingFactor.DstColor, GL.BlendingFactor.One);
                GL.GL.Color3(brightness - 1, brightness - 1, brightness - 1);
            }
            else
            {
                GL.GL.BlendFunc(GL.BlendingFactor.Zero, GL.BlendingFactor.SrcColor);
                GL.GL.Color3(brightness, brightness, brightness);
            }

            GL.GL.Translate(0, 0, BRIGHTNESS_Z_LEVEL);
            DrawUnitBox();
            GL.GL.Disable(GL.EnableCap.Blend);
        }

        public void MultMatrix(Matrix4 transform)
        {
            GL.GL.MultMatrix(ref transform);
        }

        public void DrawTriangle()
        {
            throw new System.NotImplementedException();
        }

        public void SetProjection(Matrix4 matrix4)
        {
            throw new System.NotImplementedException();
        }

        public void Load()
        {
            throw new System.NotImplementedException();
        }

        public int CreateVertexArray(float[] vertices)
        {
            throw new System.NotImplementedException();
        }

        public void Translate(Vector3 pos)
        {
            GL.GL.Translate(pos);
        }

        public void DrawSprite(ITexture texture, Vector3 pos, Vector2 size)
        {
            throw new System.NotImplementedException();
        }

        public void SetView(Matrix4 matrix4)
        {
            throw new System.NotImplementedException();
        }

        public int CreateVao(float[] vertexArray)
        {
            throw new System.NotImplementedException();
        }

        public void DrawSprite(ITexture texture, int vao, Vector3 pos, Vector2 size)
        {
            throw new System.NotImplementedException();
        }

        #endregion Public Methods
    }
}