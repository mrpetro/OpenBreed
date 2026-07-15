using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Wecs.Rendering.Systems.Extensions
{
    public static class RenderViewExtensions
    {
        #region Public Methods

        /// <summary>
        /// Render the viewport
        /// </summary>
        /// <param name="drawBorder">Draw surrounding border</param>
        /// <param name="drawBackground">Draw background</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="viewportTransform">Viewport transformation</param>
        /// <param name="func">Drawing function</param>
        public static void RenderViewport(this IRenderView view, bool drawBorder, bool drawBackground, Color4<Rgba> backgroundColor, Matrix4 viewportTransform, Action func)
        {
            view.PushMatrix();

            try
            {
                view.MultMatrix(viewportTransform);

                if (drawBackground)
                    view.Context.Primitives.DrawUnitRectangle(
                        view,
                        Matrix4.CreateTranslation(0.5f, 0.5f, 0.0f),
                        backgroundColor,
                        filled: true);

                if (drawBorder)
                    view.Context.Primitives.DrawUnitRectangle(
                        view,
                        Matrix4.CreateTranslation(0.5f, 0.5f, 0.0f),
                        Color4.Red,
                        filled: false);

                func.Invoke();
            }
            finally
            {
                view.PopMatrix();
            }
        }

        public static void RenderWorld(this IRenderView view, IWorld world, int depth, Box2 viewBox, float dt)
        {
            var renderableSystems = world.Systems.OfType<IRenderableSystem>().ToArray();
            var renderContext = new WorldRenderContext(view, depth, dt, viewBox, world);
            for (int i = 0; i < renderableSystems.Length; i++)
            {
                var renderableSystem = renderableSystems[i];

                var entities = world.GetMatchingEntities(renderableSystem);

                renderableSystem.Render(entities, renderContext);
            }
        }

        #endregion Public Methods
    }
}