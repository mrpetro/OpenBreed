using OpenBreed.Core;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Attributes;
using OpenBreed.Wecs.Components.Common;
using OpenBreed.Wecs.Components.Rendering;
using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using OpenTK;
using OpenTK.Mathematics;
using System;
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
    [RequireEntityWith(
        typeof(ViewportComponent),
        typeof(PositionComponent))]
    public class ViewportSystem : MatchingSystemBase<ViewportSystem>, IRenderableSystem
    {
        #region Private Fields

        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;
        private readonly IPaletteMan paletteMan;
        private readonly IPrimitiveRenderer primitiveRenderer;
        private readonly IRenderingMan renderingMan;
        private readonly IViewClient viewClient;

        #endregion Private Fields

        #region Internal Constructors

        internal ViewportSystem(
            IEntityMan entityMan,
            IWorldMan worldMan,
            IPaletteMan paletteMan,
            IPrimitiveRenderer primitiveRenderer,
            IRenderingMan renderingMan,
            IViewClient viewClient)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
            this.paletteMan = paletteMan;
            this.primitiveRenderer = primitiveRenderer;
            this.renderingMan = renderingMan;
            this.viewClient = viewClient;
        }

        #endregion Internal Constructors

        #region Public Methods

        public void Render(IRenderContext context)
        {
            for (int i = 0; i < entities.Count; i++)
                RenderViewport(entities[i], context.ViewBox, context.Depth, context.Dt);
        }

        #endregion Public Methods

        #region Protected Methods

        public override bool ContainsEntity(IEntity entity) => entities.Contains(entity);

        public override void AddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        public override void RemoveEntity(IEntity entity)
        {
            entities.Remove(entity);
        }

        #endregion Protected Methods

        #region Private Methods



        /// <summary>
        /// Render this viewport content to the client
        /// </summary>
        /// <param name="dt">Time step</param>
        private void RenderViewport(IEntity vpe, Box2 clipBox, int depth, float dt)
        {
            var vpc = vpe.Get<ViewportComponent>();
            var viewportPos = vpe.Get<PositionComponent>().Value;
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

            var viewportScalingType = vpc.ScalingType;

            //Apply viewport transformation matrix
            var transform = TransformHelper.GetViewportTransform(viewportPos, viewportSize);

            renderingMan.RenderViewport(vpc.DrawBorder, vpc.DrawBackgroud, vpc.BackgroundColor, transform, () => DrawCameraView(vpc.CameraEntityId, viewportSize, viewportScalingType, depth, dt));
        }

        private void DrawCameraView(int cameraEntityId, Vector2 viewportSize, ViewportScalingType viewportScalingType, int depth, float dt)
        {
            var camera = entityMan.GetById(cameraEntityId);

            if (camera != null && camera.WorldId != -1)
            {
                var cameraPalette = camera.Get<PaletteComponent>();
                var cameraPos = camera.Get<PositionComponent>().Value;
                var cameraSize = camera.Get<CameraComponent>().Size;
                var cameraBrightness = camera.Get<CameraComponent>().Brightness;
                var cameraClipBox = TransformHelper.GetVisibleRectangle(cameraPos, cameraSize);
                var cameraTransform = TransformHelper.GetCameraTransform(viewportScalingType, viewportSize, cameraPos, cameraSize);

                var cameraWorld = worldMan.GetById(camera.WorldId);

                var palette = (cameraPalette.PaletteId != -1) ? paletteMan.GetById(cameraPalette.PaletteId) : null;

                if (palette is not null)
                {
                    primitiveRenderer.PushPalette();
                }

                primitiveRenderer.PushMatrix();

                try
                {
                    if (palette is not null) primitiveRenderer.SetPalette(palette);
                    primitiveRenderer.MultMatrix(cameraTransform);

                    void OnRenderFrame(Box2 viewBox, int depth, float dt)
                    {
                        var renderable = cameraWorld.Systems.OfType<IRenderableSystem>().ToArray();
                        var renderContext = new RenderContext(depth, dt, viewBox, cameraWorld);
                        for (int i = 0; i < renderable.Length; i++)
                        {
                            renderable[i].Render(renderContext);
                        }
                    }

                    primitiveRenderer.DrawNested(cameraClipBox, depth, dt, OnRenderFrame);
                }
                finally
                {
                    primitiveRenderer.PopMatrix();

                    if (palette is not null)
                    {
                        primitiveRenderer.PopPalette();
                    }
                }

                //Draw camera effects
                primitiveRenderer.DrawBrightnessBox(cameraBrightness);
            }
        }

        #endregion Private Methods
    }
}