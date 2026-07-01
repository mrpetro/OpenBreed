using OpenBreed.Common.Interface.Drawing;
using OpenBreed.Pathfinding.Abstractions.Services;
using OpenBreed.Rendering.Abstractions.Extensions;
using OpenBreed.Sandbox.App.Components;
using OpenBreed.Sandbox.App.Constants;
using OpenBreed.Wecs.Core.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.App.Systems
{
    [RequireEntityWith(
        typeof(WaypointFollowerComponent),
        typeof(PositionComponent))]
    public class WaypointRenderSystem : IRenderableSystem
    {
        #region Public Constructors

        public WaypointRenderSystem()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public void Render(IEnumerable<IEntity> entities, IWorldRenderContext context)
        {
            foreach (var entity in entities)
            {
                Render(entity, context.View, context.ViewBox);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void RenderWaypoint(Vector2 pos, Vector2 dir, IRenderView view)
        {
            view.PushMatrix();
            view.Translate(new Vector3(pos.X, pos.Y, 0.0f));
            view.Translate(new Vector3(-8, -8, 0.0f));

            if (dir.X < 0.0f)
            {
                if (dir.Y < 0.0f)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveDownLeft);
                }
                else if (dir.Y == 0)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveLeft);
                }
                else if (dir.Y > 0.0f)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveLeftUp);
                }
            }
            else if (dir.X == 0)
            {
                if (dir.Y < 0.0f)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveDown);
                }
                else if (dir.Y == 0)
                {
                }
                else if (dir.Y > 0.0f)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveUp);
                }
            }
            else if (dir.X > 0.0f)
            {
                if (dir.Y < 0.0f)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveRightDown);
                }
                else if (dir.Y == 0)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveRight);
                }
                else if (dir.Y > 0.0f)
                {
                    view.Context.TileRenderer.Render(view, 0, Tiles.MoveUpRight);
                }
            }

            view.PopMatrix();
        }

        private void Render(IEntity entity, IRenderView view, Box2 clipBox)
        {
            var position = entity.Get<PositionComponent>();
            var follower = entity.Get<WaypointFollowerComponent>();

            if (follower.Waypoints is null || !follower.Waypoints.Any())
            {
                return;
            }

            var previousWaypoint = follower.Waypoints.First();

            foreach (var waypoint in follower.Waypoints)
            {
                var dir = waypoint - previousWaypoint;

                RenderWaypoint(waypoint, dir, view);

                previousWaypoint = waypoint;
            }
        }

        #endregion Private Methods
    }
}