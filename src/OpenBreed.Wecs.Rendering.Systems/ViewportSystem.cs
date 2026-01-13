using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Wecs.Core.Components;
using OpenBreed.Wecs.Rendering.Components;
using OpenBreed.Wecs.Core.Systems.Categories;
using OpenBreed.Wecs.Rendering.Systems.Extensions;
using OpenBreed.Wecs.Rendering.Systems.Helpers;
using OpenBreed.Wecs.Worlds;
using OpenTK.Mathematics;
using System.Linq;

namespace OpenBreed.Wecs.Rendering.Systems
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
    [SystemCategory(CommonCategories.Rendering)]
    public class ViewportSystem : IMatchingSystem, IRenderableSystem
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IWorldMan worldMan;
        private readonly IPaletteMan paletteMan;
        private readonly IWindow viewClient;

        #endregion Private Fields

        #region Public Constructors

        public ViewportSystem(
            IEntityMan entityMan,
            IWorldMan worldMan,
            IPaletteMan paletteMan,
            IWindow viewClient)
        {
            this.entityMan = entityMan;
            this.worldMan = worldMan;
            this.paletteMan = paletteMan;
            this.viewClient = viewClient;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(Abstractions.Primitives.IWorldRenderContext context)
        {
            var entities = context.World.GetMatchingEntities(this);

            foreach (var entity in entities)
            {
                RenderViewport(context.View, entity, context.ViewBox, context.Depth, context.Dt);
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Render this viewport content to the client
        /// </summary>
        /// <param name="dt">Time step</param>
        private void RenderViewport(OpenBreed.Rendering.Abstractions.IRenderView view, IEntity vpe, Box2 clipBox, int depth, float dt)
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

            view.RenderViewport(vpc.DrawBorder, vpc.DrawBackgroud, vpc.BackgroundColor, transform, () => DrawCameraView(view, vpc.CameraEntityId, viewportSize, viewportScalingType, depth, dt));
        }

        private void DrawCameraView(OpenBreed.Rendering.Abstractions.IRenderView view, int cameraEntityId, Vector2 viewportSize, ViewportScalingType viewportScalingType, int depth, float dt)
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
                    view.PushPalette();
                }

                view.PushMatrix();

                try
                {
                    if (palette is not null) view.SetPalette(palette);
                    view.MultMatrix(cameraTransform);

                    void OnRenderFrame(Box2 viewBox, int depth, float dt)
                    {
                        var renderable = cameraWorld.Systems.OfType<IRenderableSystem>().ToArray();
                        var renderContext = new WorldRenderContext(view, depth, dt, viewBox, cameraWorld);
                        for (int i = 0; i < renderable.Length; i++)
                        {
                            renderable[i].Render(renderContext);
                        }
                    }

                    view.Context.Primitives.DrawNested(view, cameraClipBox, depth, dt, OnRenderFrame);
                }
                finally
                {
                    view.PopMatrix();

                    if (palette is not null)
                    {
                        view.PopPalette();
                    }
                }

                //Draw camera effects
                view.Context.Primitives.DrawBrightnessBox(view, cameraBrightness);
            }
        }

        #endregion Private Methods
    }
}