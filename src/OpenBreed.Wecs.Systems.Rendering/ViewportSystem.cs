using OpenBreed.Core;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
using System.Collections.Generic;

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

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;
        private readonly IPrimitiveRenderer primitiveRenderer;
        private readonly IRenderingMan renderingMan;
        private readonly IViewClient viewClient;

        #endregion Private Fields

        #region Internal Constructors

        internal ViewportSystem(IEntityMan entityMan, IWorldMan worldMan, IPrimitiveRenderer primitiveRenderer, IRenderingMan renderingMan, IViewClient viewClient)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
            this.primitiveRenderer = primitiveRenderer;
            this.renderingMan = renderingMan;
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

        public Vector4 ClientToWorld(Vector4 coords, IEntity viewport)
        {
            var vpc = viewport.Get<ViewportComponent>();
            var pos = viewport.Get<PositionComponent>();

            var camera = entityMan.GetById(vpc.CameraEntityId);

            var x = Matrix4.Identity;

            var screenX = viewClient.ClientTransform;
            x = Matrix4.Mult(screenX, x);

            var viewportX = GetViewportTransform(pos.Value, vpc.Size);
            x = Matrix4.Mult(viewportX, x);

            var cpos = camera.Get<PositionComponent>();
            var cmc = camera.Get<CameraComponent>();

            var cameraX = GetCameraTransform(vpc.ScalingType, vpc.Size, cpos.Value, cmc.Size);
            x = Matrix4.Mult(cameraX, x);

            x.Invert();

            return Vector4.TransformRow(coords, x);
        }

        /// <summary>
        /// Local transformation matrix of this viewport
        /// </summary>
        public Matrix4 GetViewportTransform(Vector2 pos, Vector2 size)
        {
            var transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(size.X, size.Y, 1.0f));
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation((int)pos.X, (int)pos.Y, 0.0f));
            return transform;
        }

        /// <summary>
        /// This will return camera tranformation matrix which includes aspect ratio correction
        /// </summary>
        /// <returns>Camera transformation matrix</returns>
        public Matrix4 GetCameraTransform(ViewportScalingType viewportScalingType, Vector2 viewportSize, Vector2 cameraPos, Vector2 cameraSize)
        {
            var viewportRatio = viewportSize.X / viewportSize.Y;
            var cameraRatio = cameraSize.X / cameraSize.Y;
            var vcRatio = viewportRatio / cameraRatio;

            var transform = Matrix4.Identity;
            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-(int)cameraPos.X, -(int)cameraPos.Y, 0.0f));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(1.0f / cameraSize.X, 1.0f / cameraSize.Y, 1.0f));

            switch (viewportScalingType)
            {
                case ViewportScalingType.None:
                    transform = Matrix4.Mult(transform, Matrix4.CreateScale(cameraSize.X / viewportSize.X, cameraSize.Y / viewportSize.Y, 1.0f));
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

        protected override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        protected override void OnAddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        protected override void OnRemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods

        private Box2 GetVisibleRectangle(Vector2 pos, Vector2 size)
        {
            var x = pos.X;
            var y = pos.Y;
            return new Box2(x - size.X / 2.0f, y - size.Y / 2.0f, x + size.X / 2.0f, y + size.Y / 2.0f);
        }

        /// <summary>
        /// Render this viewport content to the client
        /// </summary>
        /// <param name="dt">Time step</param>
        private void RenderViewport(IEntity vpe, Box2 clipBox, int depth, float dt)
        {
            var vpc = vpe.Get<ViewportComponent>();
            var viewportPos = vpe.Get<PositionComponent>().Value;
            var viewportScalingType = vpc.ScalingType;
            var viewportSize = vpc.Size;

            //Test viewport for clippling here
            if (viewportPos.X + viewportSize.X < clipBox.Min.X)
                return;

            if (viewportPos.X > clipBox.Max.X)
                return;

            if (viewportPos.Y + viewportSize.Y < clipBox.Min.Y)
                return;

            if (viewportPos.Y > clipBox.Max.Y)
                return;

            //Apply viewport transformation matrix
            var transform = GetViewportTransform(viewportPos, viewportSize);

            renderingMan.RenderViewport(vpc.DrawBorder, vpc.DrawBackgroud, vpc.BackgroundColor, transform, () => DrawCameraView(vpc.CameraEntityId, viewportSize, viewportScalingType, depth, dt));
        }

        private void DrawCameraView(int cameraEntityId, Vector2 viewportSize, ViewportScalingType viewportScalingType, int depth, float dt)
        {
            var camera = entityMan.GetById(cameraEntityId);

            if (camera != null && camera.WorldId != -1)
            {
                var cameraPos = camera.Get<PositionComponent>().Value;
                var cameraSize = camera.Get<CameraComponent>().Size;
                var cameraBrightness = camera.Get<CameraComponent>().Brightness;
                var cameraClipBox = GetVisibleRectangle(cameraPos, cameraSize);
                var cameraTransform = GetCameraTransform(viewportScalingType, viewportSize, cameraPos, cameraSize);

                var cameraWorld = worldMan.GetById(camera.WorldId);
                var worldRenderable = cameraWorld.GetModule<IRenderableBatch>();
                worldRenderable.Render(cameraTransform, cameraClipBox, depth, dt);

                //Draw camera effects
                primitiveRenderer.DrawBrightnessBox(cameraBrightness);
            }
        }

        #endregion Private Methods
    }
}