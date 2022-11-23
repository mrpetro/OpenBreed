using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Rendering.Interface
{
    public interface ISpriteRenderer
    {
        #region Public Methods

        void RenderBegin();

        void RenderEnd();
        void Render(Vector3 pos3, Vector2 size, Color4 color, int atlasId, int imageId);

        #endregion Public Methods
    }
}