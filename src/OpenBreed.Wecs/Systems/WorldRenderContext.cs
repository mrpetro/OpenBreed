using System;
using OpenBreed.Rendering.Interface.Managers;
using OpenBreed.Wecs.Systems;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Worlds
{
    public class WorldRenderContext : IWorldRenderContext
    {
        #region Public Constructors

        public WorldRenderContext(
            Rendering.Interface.Managers.IRenderView view,
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

        public Rendering.Interface.Managers.IRenderView View { get; }
        public int Depth { get; }
        public float Dt { get; }
        public Box2 ViewBox { get; }
        public IWorld World { get; }

        #endregion Public Properties
    }
}