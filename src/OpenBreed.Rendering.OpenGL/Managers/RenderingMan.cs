using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Tools;
using OpenBreed.Core;
using OpenBreed.Rendering.Interface;
using OpenBreed.Rendering.Interface.Events;
using OpenBreed.Rendering.Interface.Managers;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class RenderingMan : IRenderingMan
    {
        #region Private Fields

        private readonly MovingAverage fpsAverage = new MovingAverage(samplesCount: 60);

        #endregion Private Fields

        #region Public Constructors

        public RenderingMan()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public float Fps => fpsAverage.Value;

        #endregion Public Properties

        #region Public Methods

        public void Update(float dt)
        {
            fpsAverage.Update(1.0f / dt);
        }

        #endregion Public Methods
    }
}