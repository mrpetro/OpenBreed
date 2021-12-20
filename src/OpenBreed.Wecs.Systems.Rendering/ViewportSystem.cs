using OpenBreed.Core;
using OpenBreed.Core.Extensions;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
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
    public class ViewportSystem : SystemBase, IRenderable
    {
        #region Private Fields

        private const float ZOOM_BASE = 1.0f / 512.0f;
        private const float BRIGHTNESS_Z_LEVEL = 50.0f;

        private readonly List<Entity> entities = new List<Entity>();
        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;
        private readonly IPrimitiveRenderer primitiveRenderer;
        private readonly IViewClient viewClient;

        #endregion Private Fields

        #region Internal Constructors

        internal ViewportSystem(IEntityMan entityMan, IWorldMan worldMan, IPrimitiveRenderer primitiveRenderer, IViewClient viewClient)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
            this.primitiveRenderer = primitiveRenderer;
            this.viewClient = viewClient;
            RequireEntityWith<ViewportComponent>();
            RequireEntityWith<PositionComponent>();
        }

        #endregion Internal Constructors

        #region Public Methods

        public override void Initialize(World world)
        {
            base.Initialize(world);

            world.GetModule<IRenderableBatch>().Add(this);
        }

        public Vector4 ClientToWorld(Vector4 coords, Entity viewport)
        {
            var vpc = viewport.Get<ViewportComponent>();
            var pos = viewport.Get<PositionComponent>();

            var camera = entityMan.GetById(vpc.CameraEntityId);

            var x = Matrix4.Identity;

            var screenX = viewClient.ClientTransform;
            x = Matrix4.Mult(screenX, x);

            var viewportX = GetViewportTransform(pos, vpc);
            x = Matrix4.Mult(viewportX, x);

            var cpos = camera.Get<PositionComponent>();
            var cmc = camera.Get<CameraComponent>();

            var cameraX = GetCameraTransform(vpc, cpos.Value, cmc.Size);
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
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation((int)pos.Value.X, (int)pos.Value.Y, 0.0f));
            return transform;
        }

        /// <summary>
        /// This will return camera tranformation matrix which includes aspect ratio correction
        /// </summary>
        /// <returns>Camera transformation matrix</returns>
        public Matrix4 GetCameraTransform(ViewportComponent vpc, Vector2 pos, Vector2 size)
        {
            var cameraRatio = size.X / size.Y;

            var transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-(int)pos.X, -(int)pos.Y, 0.0f));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(1.0f / size.X, 1.0f / size.Y, 1.0f));

            var vcRatio = vpc.Ratio / cameraRatio;

            switch (vpc.ScalingType)
            {
                case ViewportScalingType.None:
                    transform = Matrix4.Mult(transform, Matrix4.CreateScale(size.X / vpc.Width, size.Y / vpc.Height, 1.0f));
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
            for (int i = 0; i < entities.Count; i++)
                RenderViewport(entities[i], clipBox, depth, dt);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override bool ContainsEntity(Entity entity) => entities.Contains(entity);

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

        private Box2 GetVisibleRectangle(Vector2 pos, Vector2 size)
        {
            var x = pos.X;
            var y = pos.Y;
            return Box2.FromTLRB(y + size.Y / 2.0f, x - size.X / 2.0f, x + size.X / 2.0f, y - size.Y / 2.0f);
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



            //Apply viewport transformation matrix
            var transform = GetViewportTransform(pos, vpc);

            GL.PushMatrix();

            GL.MultMatrix(ref transform);

            if (vpc.DrawBackgroud)
                primitiveRenderer.DrawUnitBox(vpc.BackgroundColor);

            if (vpc.DrawBorder)
                primitiveRenderer.DrawUnitRectangle(Color4.Red);

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
            if (camera.WorldId == -1)
                return;

            var pos = camera.Get<PositionComponent>().Value;
            var size = camera.Get<CameraComponent>().Size;
            var clipBox = GetVisibleRectangle(pos, size);

            var brightness = camera.Get<CameraComponent>().Brightness;

            //Apply camera transformation matrix
            var transform = GetCameraTransform(vpc, pos, size);

            var world = worldMan.GetById(camera.WorldId);
            var renderable = world.GetModule<IRenderableBatch>();

            renderable.Render(transform, clipBox, depth, dt);

            //Draw camera effects
            DrawBrightnessEffect(brightness);
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

        #endregion Private Methods
    }
}