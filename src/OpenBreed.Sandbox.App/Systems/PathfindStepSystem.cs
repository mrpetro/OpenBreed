using OpenBreed.Common.Interface.Mvc;
using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Input.Abstractions;
using OpenBreed.Input.Abstractions.Events;
using OpenBreed.Pathfinding.Abstractions.Services;
using OpenBreed.Rendering.Abstractions;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Rendering.OpenGL;
using OpenBreed.Sandbox.App.Components;
using OpenBreed.Sandbox.App.Constants;
using OpenBreed.Sandbox.App.Services;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace OpenBreed.Sandbox.App.Systems
{
    public class PathfindStepSystem : IEventSystem<ViewKeyDownEvent>
    {
        #region Private Fields

        private readonly IPathfindingService pathfindingService;

        #endregion Private Fields

        #region Public Constructors

        public PathfindStepSystem(IPathfindingService pathfindingService)
        {
            this.pathfindingService = pathfindingService ?? throw new ArgumentNullException(nameof(pathfindingService));
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(ViewKeyDownEvent e, IWorld world)
        {
            if (e.Key == Keys.Space)
            {
                pathfindingService.Step();
            }
        }

        #endregion Public Methods
    }
}