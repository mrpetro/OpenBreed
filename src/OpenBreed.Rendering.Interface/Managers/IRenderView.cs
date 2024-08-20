using OpenBreed.Common.Interface.Drawing;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Rendering.Interface.Managers
{
    public interface IRenderView
    {
        #region Public Events

        public event ResizeDelegate Resized;

        #endregion Public Events

        #region Public Properties

        Box2i Box { get; }
        Matrix4 View { get; set; }
        Matrix4 Projection { get; }

        IPalette CurrentPalette { get; }

        /// <summary>
        /// Rendering context associated with this view.
        /// </summary>
        IRenderContext Context { get; }

        ResizeDelegate Resizer { get; set; }

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

        void Translate(float x, float y, float z);

        void MultMatrix(Matrix4 transform);

        void SetProjection(Matrix4 matrix4);

        Vector4 GetCoordinates(Vector4 coords);

        Vector4 GetScreenToWorldCoords(Vector4 coords);

        void SetPalette(IPalette palette);

        void PushPalette();

        void PopPalette();

        void EnableAlpha();

        void DisableAlpha();

        void Reset();

        #endregion Public Methods
    }
}