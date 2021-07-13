using OpenTK;

namespace OpenBreed.Rendering.Interface
{
    public interface IPrimitiveRenderer
    {
        #region Public Methods

        void DrawUnitRectangle();

        void DrawRectangle(Box2 clipBox);

        void DrawBox(Box2 clipBox);

        void DrawUnitBox();

        #endregion Public Methods
    }
}