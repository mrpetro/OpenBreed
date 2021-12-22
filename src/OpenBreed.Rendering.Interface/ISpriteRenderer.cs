using OpenTK;

namespace OpenBreed.Rendering.Interface
{
    public interface ISpriteRenderer
    {
        #region Public Methods

        void RenderBegin();

        void RenderEnd();
        void Render(Vector3 pos3, int atlasId, int imageId);

        #endregion Public Methods
    }
}