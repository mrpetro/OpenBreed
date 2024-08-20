using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Worlds
{
    public interface IWorldRenderContext
    {
        #region Public Properties

        Rendering.Interface.Managers.IRenderView View { get; }
        int Depth { get; }
        float Dt { get; }
        Box2 ViewBox { get; }
        IWorld World { get; }

        #endregion Public Properties
    }
}