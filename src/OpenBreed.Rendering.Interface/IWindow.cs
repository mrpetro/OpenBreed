using Microsoft.Extensions.Logging;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace OpenBreed.Rendering.Interface
{
    public interface IWindow
    {
        #region Public Properties

        /// <summary>
        /// Rendering context of this window
        /// </summary>
        IRenderContext Context { get; }

        /// <summary>
        /// Client display transformation matrix
        /// </summary>
        Matrix4 ClientTransform { get; }

        /// <summary>
        /// Client display aspect ratio
        /// </summary>
        float ClientRatio { get; }

        /// <summary>
        /// Client display rectangle
        /// </summary>
        Box2i ClientRectangle { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Exits the application
        /// TODO: Show not be part of this inteface
        /// </summary>
        void Exit();

        /// <summary>
        /// Start running main aplication loop
        /// TODO: Show not be part of this inteface
        /// </summary>
        void Run();

        #endregion Public Methods
    }
}