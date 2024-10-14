using OpenBreed.Rendering.Interface.Managers;
using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Rendering.Interface
{
    public interface ISpriteRenderer
    {
        #region Public Methods

        void RenderBegin();

        void RenderEnd();
        void Render(IRenderView view, Vector3 pos3, Vector2 scale, Color4 color, int atlasId, int imageId, bool ignoreScale = false);

        #endregion Public Methods
    }
}