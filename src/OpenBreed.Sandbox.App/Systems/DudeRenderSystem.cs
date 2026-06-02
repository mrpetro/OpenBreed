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
    [RequireEntityWithTag("Dude")]
    public class DudeRenderSystem : IRenderableSystem
    {
        private readonly IPathfindingService pathfindingService;
        #region Public Constructors

        public DudeRenderSystem(IPathfindingService pathfindingService)
        {
            this.pathfindingService = pathfindingService ?? throw new ArgumentNullException(nameof(pathfindingService));
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IEnumerable<IEntity> entities, IWorldRenderContext context)
        {
            GL.Enable(EnableCap.Texture2D);

            foreach (var entity in entities)
            {
                RenderDude(entity, context.View, context.ViewBox);
            }

            GL.Disable(EnableCap.Texture2D);
        }

        #endregion Public Methods

        #region Private Methods

        private void RenderDude(IEntity entity, IRenderView view, Box2 clipBox)
        {
            var position = entity.Get<MapPositionComponent>();
            var pathfindRequestCmp = entity.TryGet<PathfindRequestComponent>();

            view.PushMatrix();
            view.Translate(new Vector3(position.Value.X * 16, position.Value.Y * 16, 0.0f));
            view.Context.TileRenderer.Render(view, 0, Tiles.Dude);
            view.PopMatrix();

            if (pathfindRequestCmp is null)
            {
                return;
            }

            if (!pathfindingService.TryGetJob(pathfindRequestCmp.RequestId, out var job))
            {
                return;
            }

            view.PushMatrix();
            view.Translate(new Vector3(job.Goal.X * 16, job.Goal.Y * 16, 0.0f));
            view.Context.TileRenderer.Render(view, 0, Tiles.Target);
            view.PopMatrix();
        }

        #endregion Private Methods
    }
}