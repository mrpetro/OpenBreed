using System;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenBreed.Wecs.Systems;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Systems.Rendering.Primitives
{
    public class WorldRenderContext : IWorldRenderContext
    {
        #region Public Constructors

        public WorldRenderContext(
            IRenderView view,
            int depth,
            float dt,
            Box2 viewBox,
            IWorld world)
        {
            View = view;
            Depth = depth;
            Dt = dt;
            ViewBox = viewBox;
            World = world;
        }

        #endregion Public Constructors

        #region Public Properties

        public IRenderView View { get; }
        public int Depth { get; }
        public float Dt { get; }
        public Box2 ViewBox { get; }
        public IWorld World { get; }

        #endregion Public Properties
    }
}