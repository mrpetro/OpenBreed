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
using System.Diagnostics.Eventing.Reader;
using System.Linq;


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

                //if (job.Status != PathfindStatus.Found)
                //{
                //    continue;
                //}

                Render(job, context.View, context.ViewBox);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void RenderWaypoint(IPathfindJob job, Vector2i pos, Vector2i dir, IRenderView view)
        {
            view.PushMatrix();
            view.Translate(new Vector3(pos.X * 16, pos.Y * 16, 0.0f));

            if (dir.X == -1)
            {
                if (dir.Y == -1)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveDownLeft);
                }
                else if (dir.Y == 0)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveLeft);
                }
                else if (dir.Y == 1)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveLeftUp);
                }
            }
            else if (dir.X == 0)
            {
                if (dir.Y == -1)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveDown);
                }
                else if (dir.Y == 0)
                {
                }
                else if (dir.Y == 1)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveUp);
                }
            }
            else if (dir.X == 1)
            {
                if (dir.Y == -1)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveRightDown);
                }
                else if (dir.Y == 0)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveRight);
                }
                else if (dir.Y == 1)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveUpRight);
                }
            }

            view.PopMatrix();
        }

        private void Render(IPathfindJob job, IRenderView view, Box2 clipBox)
        {
            if (job.Topology is not GridTopology gridTopology)
            {
                throw new InvalidOperationException($"Expected {typeof(GridTopology)}");
            }

            var dataGrid = gridTopology.DataGrid;

            if (job.Status == PathfindStatus.Found)
            {
                GL.Enable(EnableCap.Texture2D);

                var waypoints = job.GetShortestPath().Select((id) => dataGrid.GetIndex(id)).ToArray();

                var previousWayPoint = dataGrid.GetIndex(job.StartId);

                foreach (var waypoint in waypoints)
                {
                    var dir = waypoint - previousWayPoint;

                    RenderWaypoint(job, waypoint, dir, view);
                    previousWayPoint = waypoint;
                }

                GL.Disable(EnableCap.Texture2D);

                return;
            }

            return;


            var positions = GetWaypoints(job, dataGrid);

            foreach (var pair in positions)
            {
                RenderWaypoint(job, pair.Item1, pair.Item2, view);
            }

            foreach (var front in job.Fronts)
            {
                var frontPos = dataGrid.GetIndex(front.Id);

                view.PushMatrix();
                view.Translate(new Vector3(frontPos.X * 16, frontPos.Y * 16, 0.0f));

                view.Context.TileRenderer.Render(view, 0, Tiles.Front);
                view.PopMatrix();
            }

            GL.Disable(EnableCap.Texture2D);
        }

        public IEnumerable<(Vector2i, Vector2i)> GetWaypoints(IPathfindJob job, IDataGrid<CellData> dataGrid)
        {
            foreach (var pair in job.CameFrom)
            {
                var fromPos = dataGrid.GetIndex(pair.Key);
                var toPos = dataGrid.GetIndex(pair.Value);
                yield return (fromPos, Vector2i.Subtract(toPos, fromPos));
            }
        }

        #endregion Private Methods
    }
}