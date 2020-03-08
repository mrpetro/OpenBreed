using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Extensions;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
using OpenBreed.Core.Modules.Rendering.Events;
using OpenBreed.Core.Modules.Rendering.Helpers;
using OpenBreed.Core.Systems;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Modules.Rendering.Systems
{
    /// <summary>
    /// Viewport system for rendering cameras FOV (Field of view) in viewports
    /// Related components:
    /// - ViewportComponent
    /// - CameraComponent
    /// - Position
    /// </summary>
    public class ViewportSystem : WorldSystem, ICommandExecutor, IRenderableSystem
    {
        #region Private Fields

        private const int RENDER_MAX_DEPTH = 3;
        private const float ZOOM_BASE = 1.0f / 512.0f;
        private const float BRIGHTNESS_Z_LEVEL = 50.0f;

        private readonly List<IEntity> entities = new List<IEntity>();
        private CommandHandler cmdHandler;

        #endregion Private Fields

        #region Public Constructors

        public ViewportSystem(ICore core) : base(core)
        {
            cmdHandler = new CommandHandler(this);

            Require<ViewportComponent>();
            Require<PositionComponent>();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World.RegisterHandler(ViewportResizeCommand.TYPE, cmdHandler);
        }

        public override bool ExecuteCommand(object sender, ICommand cmd)
        {
            switch (cmd.Type)
            {
                case ViewportResizeCommand.TYPE:
                    return HandleViewportResizeCommand(sender, (ViewportResizeCommand)cmd);

                default:
                    return false;
            }
        }

        private bool HandleViewportResizeCommand(object sender, ViewportResizeCommand cmd)
        {
            var toResize = entities.FirstOrDefault(item => item.Id == cmd.EntityId);

            if (toResize != null)
            {
                var vpc = toResize.GetComponent<ViewportComponent>();

                if (vpc.Width == cmd.Width && vpc.Height == cmd.Height)
                    return true;

                vpc.Width = cmd.Width;
                vpc.Height = cmd.Height;

                toResize.RaiseEvent(GfxEventTypes.VIEWPORT_RESIZED, new ViewportResizedEventArgs(vpc.Width, vpc.Height));
            }

            return true;
        }

        /// <summary>
        /// Local transformation matrix of this viewport
        /// </summary>
        public Matrix4 GetTransform(PositionComponent pos, ViewportComponent vpc)
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
        public Matrix4 GetCameraTransform(ViewportComponent vpc, IEntity camera)
        {
            var pos = camera.GetComponent<PositionComponent>();
            var cmc = camera.GetComponent<CameraComponent>();

            var transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-pos.Value.X, -pos.Value.Y, 0.0f));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(1.0f / cmc.Width, 1.0f / cmc.Height, 1.0f));

            transform = Matrix4.Mult(transform, Matrix4.CreateScale(cmc.Width / vpc.Width, cmc.Height / vpc.Height, 1.0f));
            //transform = Matrix4.Mult(transform, Matrix4.CreateScale(ZOOM_BASE, ZOOM_BASE, 1.0f));
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(0.5f, 0.5f, 0.0f));

            return transform;
        }

        public void Render(Box2 viewBox, int depth, float dt)
        {
            cmdHandler.ExecuteEnqueued();

            if (depth > RENDER_MAX_DEPTH)
                return;

            depth++;

            for (int i = 0; i < entities.Count; i++)
                RenderViewport(entities[i], viewBox, depth, dt);
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

        private void GetVisibleRectangle(Matrix4 cameraT, out Box2 viewBox)
        {
            var pointLB = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            var pointRT = new Vector4(1.0f, 1.0f, 0.0f, 1.0f);
            pointLB = Vector4.Transform(pointLB, cameraT);
            pointRT = Vector4.Transform(pointRT, cameraT);
            viewBox = Box2.FromTLRB(pointRT.Y, pointLB.X, pointRT.X, pointLB.Y);
        }

        /// <summary>
        /// Render this viewport content to the client
        /// </summary>
        /// <param name="dt">Time step</param>
        private void RenderViewport(IEntity vpe, Box2 clipBox, int depth, float dt)
        {
            var vpc = vpe.GetComponent<ViewportComponent>();
            var pos = vpe.GetComponent<PositionComponent>();

            //Test viewport for clippling here
            if (pos.Value.X + vpc.Width < clipBox.Left)
                return;

            if (pos.Value.X > clipBox.Right)
                return;

            if (pos.Value.Y + vpc.Height < clipBox.Bottom)
                return;

            if (pos.Value.Y > clipBox.Top)
                return;

            GL.PushMatrix();

            //Apply viewport transformation matrix
            var transform = GetTransform(pos, vpc);
            GL.MultMatrix(ref transform);

            //TODO: Fix clipping for viewport in viewport scenarios
            if (vpc.Clipping)
            {
                //Enable stencil buffer
                if (depth == 1)
                    GL.Enable(EnableCap.StencilTest);


                GL.ColorMask(false, false, false, false);
                GL.DepthMask(false);
                GL.StencilFunc(StencilFunction.Always, depth, depth);
                GL.StencilOp(StencilOp.Incr, StencilOp.Incr, StencilOp.Incr);

                // Draw rectangle
                GL.Color3(0.0f, 0.0f, 0.0f);
                RenderTools.DrawUnitQuad();

                GL.ColorMask(true, true, true, true);
                GL.DepthMask(true);
                GL.StencilFunc(StencilFunction.Equal, depth, depth);
                GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
            }

            if (vpc.DrawBackgroud)
                DrawBackground(vpc);

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

            var cameraEntity = Core.Entities.GetById(vpc.CameraEntityId);

            if (cameraEntity != null)
                DrawCameraView(depth, dt, vpc, cameraEntity);

            if (vpc.Clipping)
            {
                GL.ColorMask(false, false, false, false);
                GL.DepthMask(false);
                GL.StencilFunc(StencilFunction.Always, depth, depth);
                GL.StencilOp(StencilOp.Decr, StencilOp.Decr, StencilOp.Decr);

                // Draw rectangle
                GL.Color3(0.0f, 0.0f, 0.0f);
                RenderTools.DrawUnitQuad();

                GL.ColorMask(true, true, true, true);
                GL.DepthMask(true);

                if (depth == 1)
                    GL.Disable(EnableCap.StencilTest);
            }

            GL.PopMatrix();
        }

        /// <summary>
        /// This will render world part currently visible by the camera into given viewport
        /// </summary>
        /// <param name="dt">Time step</param>
        private void DrawCameraView(int depth, float dt, ViewportComponent vpc, IEntity camera)
        {
            try
            {
                GL.PushMatrix();

                //Apply camera transformation matrix
                var transform = GetCameraTransform(vpc, camera);

                GL.MultMatrix(ref transform);

                var cameraT = transform.Inverted();

                GetVisibleRectangle(cameraT, out Box2 clipBox);
            
                RenderTools.DrawBox(clipBox, Color4.LightBlue);

                if (camera.World != null)
                    camera.World.Systems.OfType<IRenderableSystem>().ForEach(item => item.Render(clipBox, depth, dt));
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
            RenderTools.DrawUnitQuad();
            GL.Disable(EnableCap.Blend);
        }

        private void DrawBackground(ViewportComponent vpc)
        {
            //Draw background for this viewport
            GL.Color4(vpc.BackgroundColor);
            RenderTools.DrawUnitQuad();
        }

        #endregion Private Methods
    }
}