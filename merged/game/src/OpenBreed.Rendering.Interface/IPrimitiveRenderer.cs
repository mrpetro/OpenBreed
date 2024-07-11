using OpenBreed.Rendering.Interface.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace OpenBreed.Rendering.Interface
{
    public enum PointType
    {
        Rectangle,
        Circle,
        Cross,
        Ex
    }

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
        void Translate(float x, float y, float z);


        void DrawRectangle(Vector2 center, Vector2 size, Color4 color, bool filled = false);
        void DrawRectangle(Box2 clipBox, Color4 color, bool filled = false);
        void DrawCircle(Vector2 pos, float radius, Color4 color, bool filled = false);
        void DrawBox(Box2 clipBox, Color4 color);
        void DrawPoint(Vector2 pos, Color4 color, PointType type);

        void DrawNested(Box2 clipBox, int depth, float dt, Action<Box2, int, float> nestedRenderAction);

        void DrawUnitRectangle(Matrix4 model, Color4 red, bool filled = false);
        void DrawUnitBox(Matrix4 model, Color4 color);
        void DrawUnitCircle(Matrix4 model, Color4 color, bool filled = false);
        void DrawBrightnessBox(float brightness);
        void MultMatrix(Matrix4 transform);
        void SetProjection(Matrix4 matrix4);

        void EnableAlpha();
        void DisableAlpha();

        Vector4 GetScreenToWorldCoords(Vector4 coords);

        void Load();

        void DrawSprite(ITexture texture, int vao, Vector3 pos, Vector2 scale, Color4 color);

        void SetPalette(IPalette palette);
        void PushPalette();
        void PopPalette();
        void SetView(Matrix4 matrix4);

        IPosTexCoordArrayBuilder CreatePosTexCoordArray();
        IPosArrayBuilder CreatePosArray();

        #endregion Public Methods
    }
}