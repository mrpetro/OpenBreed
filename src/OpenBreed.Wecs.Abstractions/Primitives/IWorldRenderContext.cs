using OpenBreed.Rendering.Abstractions;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Abstractions.Primitives
{
    public interface IWorldRenderContext
    {
        #region Public Properties

        IRenderView View { get; }
        int Depth { get; }
        float Dt { get; }
        Box2 ViewBox { get; }
        IWorld World { get; }

        #endregion Public Properties
    }
}