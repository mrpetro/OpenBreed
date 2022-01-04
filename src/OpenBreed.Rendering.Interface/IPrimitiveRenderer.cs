using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;

namespace OpenBreed.Rendering.Interface
{
    public enum MatrixMode
    {
        ModelView,
        Projection,
        Texture,
        Color
    }

    public interface IPrimitiveRenderer
    {
        #region Public Methods

        void PushMatrix();

        void PopMatrix();

        void MatrixMode(MatrixMode matrixMode);

        void DrawUnitRectangle();

        void DrawUnitRectangle(Color4 red);

        void DrawRectangle(Box2 clipBox); 
        void DrawRectangle(Box2 clipBox, Color4 color);

        void DrawBox(Box2 clipBox);

        void DrawUnitBox();

        void DrawUnitBox(Color4 color);
        void DrawBrightnessBox(float brightness);

        #endregion Public Methods
    }
}