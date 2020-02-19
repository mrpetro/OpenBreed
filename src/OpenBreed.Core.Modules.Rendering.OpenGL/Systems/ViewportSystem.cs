using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenTK;
using OpenBreed.Core.Extensions;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    public class ViewportSystem : WorldSystem, ICommandExecutor, IRenderableSystem
    {
        private const float ZOOM_BASE = 1.0f / 512.0f;
        private const float BRIGHTNESS_Z_LEVEL = 50.0f;

        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private CommandHandler cmdHandler;

        #endregion Private Fields

        #region Public Constructors

        public ViewportSystem(ICore core) : base(core)
        {
            cmdHandler = new CommandHandler(this);

            Require<ViewportComponent>();
            Require<Position>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);
        }

        /// <summary>
        /// This will draw all tiles to viewport given in the parameter
        /// </summary>
        /// <param name="viewport">Viewport on which tiles will be drawn to</param>
        public void Render(IViewport viewport, float dt)
        {
        }

        /// <summary>
        /// Local transformation matrix of this viewport
        /// </summary>
        public Matrix4 GetTransform(Position pos, ViewportComponent vpc)
        {
            var transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(vpc.Width, vpc.Height, 1.0f));
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(pos.Value.X, pos.Value.Y, 0.0f));
            return transform;
        }

        /// <summary>
        /// This will return camera tranformation matrix which includes aspect ratio correction
        /// </summary>
        /// <returns>Camera transformation matrix</returns>
        public Matrix4 GetCameraTransform(IEntity camera)
        {
            var pos = camera.GetComponent<Position>();
            var cmc = camera.GetComponent<CameraComponent>();

            var transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-pos.Value.X, -pos.Value.Y, 0.0f));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(cmc.Zoom, cmc.Zoom, 1.0f));

            var ratio = Core.ClientRatio * 1.0f;// Ratio;
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(1.0f, ratio, 1.0f));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(ZOOM_BASE, ZOOM_BASE, 1.0f));
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(0.5f, 0.5f, 0.0f));

            return transform;
        }

        public void Render(float dt)
        {
            for (int i = 0; i < entities.Count; i++)
                Render(entities[i], dt);
        }

        /// <summary>
        /// Render this viewport content to the client
        /// </summary>
        /// <param name="dt">Time step</param>
        public void Render(IEntity entity, float dt)
        {
            var vpc = entity.GetComponent<ViewportComponent>();
            var pos = entity.GetComponent<Position>();


            GL.PushMatrix();

            //Apply viewport transformation matrix
            var transform = GetTransform(pos, vpc);
            GL.MultMatrix(ref transform);

            if (vpc.Clipping)
            {
                //Clear stencil buffer before drawing in it
                GL.Clear(ClearBufferMask.StencilBufferBit);
                //Enable stencil buffer
                GL.Enable(EnableCap.StencilTest);

                GL.StencilFunc(StencilFunction.Equal, 0x1, 0x1);
                GL.StencilOp(StencilOp.Replace, StencilOp.Replace, StencilOp.Replace);
                //Draw rectangle shape which will clip anything inside viewport
                GL.Color3(0.0f, 0.0f, 0.0f);

                DrawUnitQuad();

                GL.StencilFunc(StencilFunction.Equal, 0x1, 0x1);
                GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
            }

            if (vpc.DrawBackgroud)
                DrawBackground(vpc);

            var cameraEntity = Core.Entities.GetById(vpc.CameraEntityId);

            if(cameraEntity != null)
                DrawCameraView(dt, entity, cameraEntity);

            if (vpc.Clipping)
            {
                GL.Disable(EnableCap.StencilTest);
            }

            if (vpc.DrawBorder)
            {
                GL.Color4(1.0f, 0.0f, 0.0f, 1.0f);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(0, 1.0f, 0.0);
                GL.Vertex3(0, 0, 0.0);
                GL.Vertex3(1.0f, 0, 0.0);
                GL.Vertex3(1.0f, 1.0f, 0.0);
                GL.End();
            }

            GL.PopMatrix();
        }

        public Vector2 ViewportToWorldPoint(IEntity camera, Vector2 point)
        {
            var cameraT = GetCameraTransform(camera);
            cameraT.Invert();
            var pointLB = new Vector4(point.X, point.Y, 0.0f, 1.0f);
            pointLB = Vector4.Transform(pointLB, cameraT);

            return new Vector2(pointLB.X, pointLB.Y);
        }

        public void GetVisibleRectangle(IEntity camera, out float left, out float bottom, out float right, out float top)
        {
            var pointLB = new Vector2(0.0f, 0.0f);
            var pointRT = new Vector2(1.0f, 1.0f);
            pointLB = ViewportToWorldPoint(camera, pointLB);
            pointRT = ViewportToWorldPoint(camera, pointRT);
            //pointLB = Vector4.Transform(pointLB, transf);
            //pointRT = Vector4.Transform(pointRT, transf);

            left = pointLB.X;
            bottom = pointLB.Y;
            right = pointRT.X;
            top = pointRT.Y;
        }

        /// <summary>
        /// This will render world part currently visible by the camera into given viewport
        /// </summary>
        /// <param name="dt">Time step</param>
        private void DrawCameraView(float dt, IEntity viewport, IEntity camera)
        {
            try
            {
                GL.PushMatrix();

                //Apply camera transformation matrix
                var transform = GetCameraTransform(camera);
                GL.MultMatrix(ref transform);

                GetVisibleRectangle(camera, out float left, out float bottom, out float right, out float top);

                if (camera.World != null)
                    camera.World.Systems.OfType<ICameraSystem>().ForEach(item => item.Render(left,bottom, right, top, dt));
            }
            finally
            {
                GL.PopMatrix();
            }

            var cameraComponent = camera.GetComponent<CameraComponent>();

            //Draw camera effects
            DrawBrightnessEffect(cameraComponent.Brightness);
        }

        private void DrawBrightnessEffect(float brightness)
        {
            GL.Enable(EnableCap.Blend);
            if (brightness > 1.0)
            {
                GL.BlendFunc(BlendingFactor.DstColor, BlendingFactor.One);
                GL.Color3(brightness - 1, brightness - 1, brightness - 1);
            }
            else
            {
                GL.BlendFunc(BlendingFactor.Zero, BlendingFactor.SrcColor);
                GL.Color3(brightness, brightness, brightness);
            }

            GL.Translate(0, 0, BRIGHTNESS_Z_LEVEL);
            DrawUnitQuad();
            GL.Disable(EnableCap.Blend);
        }

        public static void DrawUnitQuad()
        {
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(0, 1.0f, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(1.0f, 0, 0);
            GL.Vertex3(1.0f, 1.0f, 0);
            GL.End();
        }

        private void DrawBackground(ViewportComponent vpc)
        {
            //Draw background for this viewport
            GL.Color4(vpc.BackgroundColor);
            DrawUnitQuad();
        }

        public bool EnqueueMsg(object sender, IEntityCommand msg)
        {
            return false;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RegisterEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        protected override void UnregisterEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods




        #endregion Private Methods
    }
}
