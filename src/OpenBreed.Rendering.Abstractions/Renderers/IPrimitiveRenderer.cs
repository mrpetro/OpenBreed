using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenBreed.Rendering.Abstractions.Renderers
{
    public enum PointType
    {
        Rectangle,
        RectangleFilled,
        Circle,
        CircleFilled,
        Cross,
        Ex
    }

    public interface IPrimitiveRenderer
    {
        #region Public Methods

        void DrawRectangle(IRenderView view, Vector2 center, Vector2 size, Color4 color, bool filled = false);
        void DrawRectangle(IRenderView view, Box2 rectangle, Color4 color, bool filled = false);
        void DrawCircle(IRenderView view, Vector2 pos, float radius, Color4 color, bool filled = false);
        void DrawBox(IRenderView view, Box2 clipBox, Color4 color);
        void DrawTriangle(IRenderView view, Vector2 p1, Vector2 p2, Vector2 p3, Color4 color, bool filled = false);
        void DrawPoint(IRenderView view, Vector2 pos, Color4 color, PointType type, float size = 2.0f, bool ignoreScale = false);
        void DrawPoints(IRenderView view, IReadOnlyList<Vector2> points, Color4 color, PointType type, float size = 2.0f, bool ignoreScale = false);
        void DrawLine(IRenderView view, Vector2 startPoint, Vector2 endPoint, Color4 color);
        void DrawLines(IRenderView view, IReadOnlyList<Vector2> points, Color4 color);
        
        void DrawClipped(IRenderView view, Box2i clipBox, Action<Box2i> nestedRenderAction);

        void DrawNested(IRenderView view, Box2 clipBox, int depth, float dt, Action<Box2, int, float> nestedRenderAction);

        void DrawUnitRectangle(IRenderView view, Matrix4 model, Color4 red, bool filled = false);
        void DrawUnitBox(IRenderView view, Matrix4 model, Color4 color);
        void DrawUnitCircle(IRenderView view, Matrix4 model, Color4 color, bool filled = false);
        void DrawUnitLine(IRenderView view, Matrix4 model, Color4 color);
        void DrawBrightnessBox(IRenderView view, float brightness);

        void Load();

        void SetTextureShader(IRenderView view, ITexture texture, Matrix4 model, Color4 color);

        IPosTexCoordArrayBuilder CreatePosTexCoordArray();
        IPosArrayBuilder CreatePosArray();

        #endregion Public Methods
    }
}