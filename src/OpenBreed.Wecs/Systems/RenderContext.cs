using System;
using OpenBreed.Wecs.Systems;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Worlds
{
    public class RenderContext : IRenderContext
    {
        #region Public Constructors

        public RenderContext(
            int depth,
            float dt,
            Box2 viewBox,
            IWorld world)
        {
            Depth = depth;
            Dt = dt;
            ViewBox = viewBox;
            World = world;
        }

        #endregion Public Constructors

        #region Public Properties

        public int Depth { get; }
        public float Dt { get; }
        public Box2 ViewBox { get; }
        public IWorld World { get; }

        #endregion Public Properties
    }
}