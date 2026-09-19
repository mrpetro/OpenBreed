using Microsoft.Extensions.Logging;
using OpenTK;
using OpenTK.Input;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace OpenBreed.Rendering.Abstractions
{
    public interface IWindow
    {
        #region Public Properties

        /// <summary>
        /// Rendering context of this window
        /// </summary>
        IRenderContext Context { get; }

        #endregion Public Properties
    }
}