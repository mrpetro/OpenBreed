using OpenTK.Mathematics;

namespace OpenBreed.Wecs.Worlds
{
    public interface IRenderContext
    {
        #region Public Properties

        int Depth { get; }
        float Dt { get; }
        Box2 ViewBox { get; }
        IWorld World { get; }

        #endregion Public Properties
    }
}