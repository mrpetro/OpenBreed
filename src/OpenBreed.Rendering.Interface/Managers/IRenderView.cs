using OpenBreed.Common.Interface.Drawing;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Interface.Managers
{
    public delegate Vector2i HostCoordinateSystemConverter(Vector2i point);

    public interface IWin
    {
        Box2 Body { get; }

        Vector2 Margin { get; }

        Color4 BorderColor { get; }

        Color4 ForegroundColor { get; }

        Vector2 Pos { get; set; }

        IList<IWin> Childs { get; }
    }

    public interface IRenderView
    {
        #region Public Events

        /// <summary>
        /// 
        /// </summary>
        event ViewResizeHandler Resized;

        /// <summary>
        /// Event occuring during view rendering.
        /// </summary>
        event ViewRenderHandler Rendering;

        /// <summary>
        /// Event occurs when cursor enters view area.
        /// </summary>
        event ViewCursorEnterHandler CursorEnter;

        /// <summary>
        /// Event occurs when cursor leaves view area.
        /// </summary>
        event ViewCursorLeaveHandler CursorLeave;

        /// <summary>
        /// Event occurs when cursor button is pressed on view area.
        /// </summary>
        event ViewCursorDownHandler CursorDown;

        /// <summary>
        /// Event occurs when cursor button is released on view area.
        /// </summary>
        event ViewCursorUpHandler CursorUp;

        /// <summary>
        /// Event occurs when cursor is moved on view area.
        /// </summary>
        event ViewCursorMoveHandler CursorMove;

        /// <summary>
        /// Event occurs when cursor wheel is changing on view area.
        /// </summary>
        event ViewCursorWheelHandler CursorWheel;

        /// <summary>
        /// Event occurs when text is being inputed.
        /// </summary>
        event ViewTextInputHandler TextInput;

        /// <summary>
        /// Event occurs when keyboard key is being pressed.
        /// </summary>
        event ViewKeyboardKeyHandler KeyDown;

        /// <summary>
        /// Event occurs when keyboard key is being released.
        /// </summary>
        event ViewKeyboardKeyHandler KeyUp;


        #endregion Public Events

        #region Public Properties

        int Id { get; }
        Box2i Box { get; }
        Matrix4 View { get; set; }
        Matrix4 Projection { get; }

        IPalette CurrentPalette { get; }

        /// <summary>
        /// Rendering context associated with this view.
        /// </summary>
        IRenderContext Context { get; }

        ViewResizeHandler Resizer { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Render
        /// </summary>
        /// <param name="drawBorder">Draw surrounding border</param>
        /// <param name="drawBackground">Draw background</param>
        /// <param name="backgroundColor">Background color</param>
        /// <param name="viewportTransform">Viewport transformation</param>
        /// <param name="func">Drawing function</param>
        void RenderViewport(bool drawBorder, bool drawBackground, Color4 backgroundColor, Matrix4 viewportTransform, Action func);

        void PushMatrix();

        void PopMatrix();

        void Translate(Vector3 pos);

        void Translate(Vector2 pos);

        void Translate(float x, float y, float z);

        void Scale(float x, float y);

        void Scale(float value);

        void MultMatrix(Matrix4 transform);

        void SetProjection(Matrix4 matrix4);

        Vector4 GetViewToWorldCoords(Vector2i point);
        Vector2i GetWorldToViewCoords(Vector2 point);
        Box2 GetViewToWorldCoords(Box2i box);
        Box2i GetWorldToViewCoords(Box2 box);
        Vector4 GetHostToWorldCoords(Vector2i point);
        Vector2i GetHostToViewCoords(Vector2i point);

        void SetPalette(IPalette palette);

        void PushPalette();

        void PopPalette();

        void EnableAlpha();

        void DisableAlpha();

        void Reset();

        void RenderWinScissor(IWin win);
        void RenderWinStencil(IWin win);


        #endregion Public Methods
    }
}