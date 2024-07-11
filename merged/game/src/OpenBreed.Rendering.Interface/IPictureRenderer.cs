using OpenTK;
using OpenTK.Mathematics;

namespace OpenBreed.Rendering.Interface
{
    public interface IPictureRenderer
    {
        #region Public Methods

        void RenderBegin();

        void RenderEnd();
        void Render(Vector3 pos3, Vector2 size, Color4 color, int imageId);

        #endregion Public Methods
    }
}