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

    public interface IPrimitiveRenderer
    {
        #region Public Methods

        void DrawRectangle(IRenderView view, Vector2 center, Vector2 size, Color4 color, bool filled = false);
        void DrawRectangle(IRenderView view, Box2 clipBox, Color4 color, bool filled = false);
        void DrawCircle(IRenderView view, Vector2 pos, float radius, Color4 color, bool filled = false);
        void DrawBox(IRenderView view, Box2 clipBox, Color4 color);
        void DrawPoint(IRenderView view, Vector2 pos, Color4 color, PointType type);

        void DrawNested(IRenderView view, Box2 clipBox, int depth, float dt, Action<Box2, int, float> nestedRenderAction);

        void DrawUnitRectangle(IRenderView view, Matrix4 model, Color4 red, bool filled = false);
        void DrawUnitBox(IRenderView view, Matrix4 model, Color4 color);
        void DrawUnitCircle(IRenderView view, Matrix4 model, Color4 color, bool filled = false);
        void DrawBrightnessBox(IRenderView view, float brightness);

        void Load();

        void DrawSprite(IRenderView view, ITexture texture, int vao, Vector3 pos, Vector2 scale, Color4 color);


        IPosTexCoordArrayBuilder CreatePosTexCoordArray();
        IPosArrayBuilder CreatePosArray();

        #endregion Public Methods
    }
}