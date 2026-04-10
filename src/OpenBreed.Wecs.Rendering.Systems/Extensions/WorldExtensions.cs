using OpenBreed.Rendering.Abstractions;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Rendering.Systems.Extensions
{
    public static class WorldExtensions
    {
        #region Public Methods

        public static void CreateView(this IWorld world, IRenderContext renderContext)
        {
            var view = renderContext.CreateView();
            world.AddToView(view);
        }

        public static void AddToView(this IWorld world, IRenderView renderView)
        {
            renderView.Rendering += (s, dt) =>
            {
                renderView.RenderWorld(world, 0, new Box2(renderView.Box.Min, renderView.Box.Max), dt);
            };
        }

        #endregion Public Methods
    }
}