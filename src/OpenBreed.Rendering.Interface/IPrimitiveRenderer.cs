using OpenTK;
using OpenTK.Graphics;

namespace OpenBreed.Rendering.Interface
{
    public interface IPrimitiveRenderer
    {
        #region Public Methods

        void DrawUnitRectangle();

        void DrawUnitRectangle(Color4 red);

        void DrawRectangle(Box2 clipBox);

        void DrawBox(Box2 clipBox);

        void DrawUnitBox();

        void DrawUnitBox(Color4 color);

        #endregion Public Methods
    }
}