using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System.Collections.Generic;

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
        void Translate(Vector3 pos);

        void DrawUnitRectangle(Matrix4 model, Color4 red);

        void DrawRectangle(Box2 clipBox, Color4 color);

        void DrawBox(Box2 clipBox, Color4 color);

        void DrawUnitBox(Matrix4 model, Color4 color);
        void DrawBrightnessBox(float brightness);

        void MultMatrix(Matrix4 transform);
        void SetProjection(Matrix4 matrix4);
        void Load();

        void DrawSprite(ITexture texture, int vao, Vector3 pos, Vector2 size);
        void SetView(Matrix4 matrix4);

        IPosTexCoordArrayBuilder CreatePosTexCoordArray();
        IPosArrayBuilder CreatePosArray();

        #endregion Public Methods
    }
}