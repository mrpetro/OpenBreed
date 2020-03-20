using OpenBreed.Core.Commands;
using OpenBreed.Core.Common;
using OpenBreed.Core.Common.Systems.Components;
using OpenBreed.Core.Entities;
using OpenBreed.Core.Extensions;
using OpenBreed.Core.Helpers;
using OpenBreed.Core.Modules.Physics.Events;
using OpenBreed.Core.Modules.Rendering.Commands;
using OpenBreed.Core.Modules.Rendering.Components;
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

        public Vector4 ClientToWorld(Vector4 coords, IEntity viewport)
        {
            var vpc = viewport.GetComponent<ViewportComponent>();
            var pos = viewport.GetComponent<PositionComponent>();

            var camera = Core.Entities.GetById(vpc.CameraEntityId);

            var x = Matrix4.Identity;

            var screenX = Core.ClientTransform;
            x = Matrix4.Mult(screenX, x);

            var viewportX = GetViewportTransform(pos, vpc);
            x = Matrix4.Mult(viewportX, x);

            var cameraX = GetCameraTransform(vpc, camera);
            x = Matrix4.Mult(cameraX, x);

            x.Invert();

            return Vector4.Transform(coords, x);
        }

        /// <summary>
        /// Local transformation matrix of this viewport
        /// </summary>
        public Matrix4 GetViewportTransform(PositionComponent pos, ViewportComponent vpc)
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

            var vcRatio = vpc.Ratio / cmc.Ratio;

            switch (vpc.ScalingType)
            {
                case ViewportScalingType.None:
                    transform = Matrix4.Mult(transform, Matrix4.CreateScale(cmc.Width / vpc.Width, cmc.Height / vpc.Height, 1.0f));
                    break;

                case ViewportScalingType.FitHeightPreserveAspectRatio:
                    transform = Matrix4.Mult(transform, Matrix4.CreateScale(1.0f / vcRatio, 1.0f, 1.0f));
                    break;

                case ViewportScalingType.FitWidthPreserveAspectRatio:
                    transform = Matrix4.Mult(transform, Matrix4.CreateScale(1.0f, vcRatio, 1.0f));
                    break;

                case ViewportScalingType.FitBothPreserveAspectRatio:
                    if (vcRatio >= 1)
                        transform = Matrix4.Mult(transform, Matrix4.CreateScale(1.0f / vcRatio, 1.0f, 1.0f));
                    else
                        transform = Matrix4.Mult(transform, Matrix4.CreateScale(1.0f, vcRatio, 1.0f));
                    break;

                case ViewportScalingType.FitBothIgnoreAspectRatio:
                default:
                    break;
            }

            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(0.5f, 0.5f, 0.0f));

            return transform;
        }

        public void Render(Box2 clipBox, int depth, float dt)
        {
            cmdHandler.ExecuteEnqueued();

            if (depth > RENDER_MAX_DEPTH)
                return;

            depth++;

            for (int i = 0; i < entities.Count; i++)
                RenderViewport(entities[i], clipBox, depth, dt);
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

                toResize.RaiseEvent(new ViewportResizedEventArgs(vpc.Width, vpc.Height));
            }

            return true;
        }

        private void GetVisibleRectangle(IEntity camera, Matrix4 cameraT, out Box2 viewBox)
        {
            var pos = camera.GetComponent<PositionComponent>();
            var cmc = camera.GetComponent<CameraComponent>();
            var x = pos.Value.X;
            var y = pos.Value.Y;

            viewBox = Box2.FromTLRB(y + cmc.Height / 2.0f, x - cmc.Width / 2.0f, x + cmc.Width / 2.0f, y - cmc.Height / 2.0f);

            //var x = pos.Value.X * 0.5f;
            //var y = pos.Value.Y * 0.5f;
            ////var x = 0.0f;
            ////var y = 0.0f;

            //var pointLB = new Vector4(0.0f, 0.0f, 0.0f, -0.5f);
            //var pointRT = new Vector4(-0.5f, -0.5f, 0.0f, -0.5f);
            //pointLB = Vector4.Transform(pointLB, cameraT);
            //pointRT = Vector4.Transform(pointRT, cameraT);
            //cmc
            //viewBox = Box2.FromTLRB(y - pointRT.Y, x - pointLB.X , x - pointRT.X, y - pointLB.Y);
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
            var transform = GetViewportTransform(pos, vpc);
            GL.MultMatrix(ref transform);


            if (vpc.DrawBackgroud)
                DrawBackground(vpc);

            if (vpc.DrawBorder)
            {
                GL.Color4(1.0f, 0.0f, 0.0f, 1.0f);
                RenderTools.DrawUnitRectangle();
            }

            var cameraEntity = Core.Entities.GetById(vpc.CameraEntityId);

            if (cameraEntity != null)
                DrawCameraView(depth, dt, vpc, cameraEntity);

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

                GetVisibleRectangle(camera, transform, out Box2 clipBox);

                GL.Color4(Color4.LightBlue);
                RenderTools.DrawRectangle(clipBox);

                if (vpc.Clipping)
                {
                    //Enable stencil buffer
                    if (depth == 1)
                        GL.Enable(EnableCap.StencilTest);

                    GL.ColorMask(false, false, false, false);
                    GL.DepthMask(false);
                    GL.StencilFunc(StencilFunction.Always, depth, depth);
                    GL.StencilOp(StencilOp.Incr, StencilOp.Incr, StencilOp.Incr);

                    // Draw black box
                    GL.Color4(Color4.Black);
                    RenderTools.DrawBox(clipBox);

                    GL.ColorMask(true, true, true, true);
                    GL.DepthMask(true);
                    GL.StencilFunc(StencilFunction.Equal, depth, depth);
                    GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Keep);
                }

                if (camera.World != null)
                    camera.World.Systems.OfType<IRenderableSystem>().ForEach(item => item.Render(clipBox, depth, dt));

                if (vpc.Clipping)
                {
                    GL.ColorMask(false, false, false, false);
                    GL.DepthMask(false);
                    GL.StencilFunc(StencilFunction.Always, depth, depth);
                    GL.StencilOp(StencilOp.Decr, StencilOp.Decr, StencilOp.Decr);

                    // Draw black box
                    GL.Color4(Color4.Black);
                    RenderTools.DrawBox(clipBox);

                    GL.ColorMask(true, true, true, true);
                    GL.DepthMask(true);

                    if (depth == 1)
                        GL.Disable(EnableCap.StencilTest);
                }
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
            RenderTools.DrawUnitBox();
            GL.Disable(EnableCap.Blend);
        }

        private void DrawBackground(ViewportComponent vpc)
        {
            //Draw background for this viewport
            GL.Color4(vpc.BackgroundColor);
            RenderTools.DrawUnitBox();
        }

        #endregion Private Methods
    }
}