using OpenBreed.Core;
using OpenBreed.Core.Extensions;
using OpenBreed.Core.Managers;
using OpenBreed.Rendering.Interface;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Systems.Rendering.Commands;
using OpenBreed.Wecs.Systems.Rendering.Events;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Wecs.Systems.Rendering
{
    /// <summary>
    /// Viewport system for rendering cameras FOV (Field of view) in viewports
    /// Related components:
    /// - ViewportComponent
    /// - CameraComponent
    /// - Position
    /// </summary>
    public class ViewportSystem : SystemBase, IRenderableSystem
    {
        #region Private Fields

        private const int RENDER_MAX_DEPTH = 3;
        private const float ZOOM_BASE = 1.0f / 512.0f;
        private const float BRIGHTNESS_Z_LEVEL = 50.0f;

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IEntityMan entityMan;
        private readonly IPrimitiveRenderer primitiveRenderer;
        private readonly ICoreClient windowClient;

        #endregion Private Fields

        #region Internal Constructors

        internal ViewportSystem(IEntityMan entityMan, IPrimitiveRenderer primitiveRenderer, ICoreClient windowClient)
        {
            this.entityMan = entityMan;
            this.primitiveRenderer = primitiveRenderer;
            this.windowClient = windowClient;
            Require<ViewportComponent>();
            Require<PositionComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public static void RegisterHandlers(ICommandsMan commands)
        {
            commands.Register<ViewportResizeCommand>(HandleViewportResizeCommand);
        }

        public Vector4 ClientToWorld(Vector4 coords, Entity viewport)
        {
            var vpc = viewport.Get<ViewportComponent>();
            var pos = viewport.Get<PositionComponent>();

            var camera = entityMan.GetById(vpc.CameraEntityId);

            var x = Matrix4.Identity;

            var screenX = windowClient.ClientTransform;
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
        public Matrix4 GetCameraTransform(ViewportComponent vpc, Entity camera)
        {
            var pos = camera.Get<PositionComponent>();
            var cmc = camera.Get<CameraComponent>();

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
            if (depth > RENDER_MAX_DEPTH)
                return;

            depth++;

            for (int i = 0; i < entities.Count; i++)
                RenderViewport(entities[i], clipBox, depth, dt);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnAddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(Entity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private static bool HandleViewportResizeCommand(ICore core, ViewportResizeCommand cmd)
        {
            var toResize = core.GetManager<IEntityMan>().GetById(cmd.EntityId);

            if (toResize != null)
            {
                var vpc = toResize.Get<ViewportComponent>();

                if (vpc.Width == cmd.Width && vpc.Height == cmd.Height)
                    return true;

                vpc.Width = cmd.Width;
                vpc.Height = cmd.Height;

                toResize.RaiseEvent(new ViewportResizedEventArgs(vpc.Width, vpc.Height));
            }

            return true;
        }

        private void GetVisibleRectangle(Entity camera, Matrix4 cameraT, out Box2 viewBox)
        {
            var pos = camera.Get<PositionComponent>();
            var cmc = camera.Get<CameraComponent>();
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
        private void RenderViewport(Entity vpe, Box2 clipBox, int depth, float dt)
        {
            var vpc = vpe.Get<ViewportComponent>();
            var pos = vpe.Get<PositionComponent>();

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
                primitiveRenderer.DrawUnitRectangle();
            }

            var cameraEntity = entityMan.GetById(vpc.CameraEntityId);

            if (cameraEntity != null)
                DrawCameraView(depth, dt, vpc, cameraEntity);

            GL.PopMatrix();
        }

        /// <summary>
        /// This will render world part currently visible by the camera into given viewport
        /// </summary>
        /// <param name="dt">Time step</param>
        private void DrawCameraView(int depth, float dt, ViewportComponent vpc, Entity camera)
        {
            try
            {
                GL.PushMatrix();

                //Apply camera transformation matrix
                var transform = GetCameraTransform(vpc, camera);

                GL.MultMatrix(ref transform);

                GetVisibleRectangle(camera, transform, out Box2 clipBox);

                GL.Color4(Color4.LightBlue);
                primitiveRenderer.DrawRectangle(clipBox);

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
                    primitiveRenderer.DrawBox(clipBox);

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
                    primitiveRenderer.DrawBox(clipBox);

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

            var cameraComponent = camera.Get<CameraComponent>();

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
            primitiveRenderer.DrawUnitBox();
            GL.Disable(EnableCap.Blend);
        }

        private void DrawBackground(ViewportComponent vpc)
        {
            //Draw background for this viewport
            GL.Color4(vpc.BackgroundColor);
            primitiveRenderer.DrawUnitBox();
        }

        #endregion Private Methods
    }
}