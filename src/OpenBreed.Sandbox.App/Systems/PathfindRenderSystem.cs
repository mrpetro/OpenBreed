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
    [RequireEntityWith(typeof(PathfindRequestComponent))]
    public class PathfindRenderSystem : IRenderableSystem
    {
        #region Private Fields

        private readonly IPathfindingService pathfindingService;

        #endregion Private Fields

        #region Public Constructors

        public PathfindRenderSystem(IPathfindingService pathfindingService)
        {
            this.pathfindingService = pathfindingService ?? throw new ArgumentNullException(nameof(pathfindingService));
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IEnumerable<IEntity> entities, IWorldRenderContext context)
        {
            foreach (var entity in entities)
            {
                var requestCmp = entity.Get<PathfindRequestComponent>();

                if (!pathfindingService.TryGetJob(requestCmp.RequestId, out var job))
                {
                    continue;
                }

                Render(job, context.View, context.ViewBox);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void RenderWaypoint(IPathfindJob job, Vector2i pos, Vector2i dir, IRenderView view)
        {
            view.PushMatrix();
            view.Translate(new Vector3(pos.X * 16, pos.Y * 16, 0.0f));

            if (dir.X == -1 && dir.Y == 0)
            {
                view.Context.TileRenderer.Render(view, 0, Tiles.MoveLeft);
            }
            else if (dir.X == 1 && dir.Y == 0)
            {
                view.Context.TileRenderer.Render(view, 0, Tiles.MoveRight);
            }
            else if (dir.X == 0 && dir.Y == -1)
            {
                view.Context.TileRenderer.Render(view, 0, Tiles.MoveDown);
            }
            else if (dir.X == 0 && dir.Y == 1)
            {
                view.Context.TileRenderer.Render(view, 0, Tiles.MoveUp);
            }

            view.PopMatrix();
        }

        private void Render(IPathfindJob job, IRenderView view, Box2 clipBox)
        {
            var dataGrid = job.Topology;

            if (job.Status == PathfindStatus.Found)
            {
                GL.Enable(EnableCap.Texture2D);
                var waypoints = job.GetShortestPath();

                var previousWayPoint = job.Start;

                foreach (var waypoint in waypoints)
                {
                    var dir = waypoint - previousWayPoint;

                    RenderWaypoint(job, waypoint, dir, view);
                    previousWayPoint = waypoint;
                }

                GL.Disable(EnableCap.Texture2D);

                return;
            }


            var positions = job.GetWaypoints();
            foreach (var pair in positions)
            {
                RenderWaypoint(job, pair.Item1, pair.Item2, view);
            }

            foreach (var front in job.Fronts)
            {
                var frontPos = dataGrid.GetPosition(front.Id);

                view.PushMatrix();
                view.Translate(new Vector3(frontPos.X * 16, frontPos.Y * 16, 0.0f));

                view.Context.TileRenderer.Render(view, 0, Tiles.Front);
                view.PopMatrix();
            }

            GL.Disable(EnableCap.Texture2D);
        }

        #endregion Private Methods
    }
}