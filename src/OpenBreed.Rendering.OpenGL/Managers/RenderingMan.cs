using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Tools;
using OpenBreed.Core;
using OpenBreed.Core.Abstractions.Events;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Managers;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace OpenBreed.Rendering.OpenGL.Managers
{
    public class RenderingMan : IRenderingMan
    {
        #region Private Fields

        private readonly MovingAverage fpsAverage = new MovingAverage(samplesCount: 60);
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        public RenderingMan(IEventsMan eventsMan)
        {
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));

            this.eventsMan.Subscribe<WindowUpdateEvent>((e) => OnUpdateFrame(e.Dt));
        }

        #endregion Public Constructors

        #region Public Properties

        public float Fps => fpsAverage.Value;

        #endregion Public Properties

        #region Private Methods

        private void OnUpdateFrame(float dt)
        {
            fpsAverage.Update(1.0f / dt);
        }

        #endregion Private Methods
    }
}