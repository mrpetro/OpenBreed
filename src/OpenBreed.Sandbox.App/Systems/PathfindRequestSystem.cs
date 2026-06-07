using OpenBreed.Pathfinding.Abstractions.Services;
using OpenBreed.Sandbox.App.Components;
using OpenBreed.Sandbox.App.Services;
using OpenBreed.Wecs.Abstractions.Events;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OpenBreed.Sandbox.App.Systems
{
    public class PathfindRequestSystem : IEventSystem<ViewCursorDownEvent>
    {
        #region Private Fields

        private readonly IEntityMan entityMan;
        private readonly IPathfindingService pathfindingService;

        #endregion Private Fields

        #region Public Constructors

        public PathfindRequestSystem(IEntityMan entityMan, IPathfindingService pathfindingService)
        {
            this.entityMan = entityMan ?? throw new ArgumentNullException(nameof(entityMan));
            this.pathfindingService = pathfindingService ?? throw new ArgumentNullException(nameof(pathfindingService));
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnEvent(ViewCursorDownEvent e, IWorld world)
        {
            if (e.Key != CursorKey.Left)
            {
                return;
            }

            var cursorPos4 = e.Position;

            var indexPos = new Vector2i((int)(cursorPos4.X / 16), (int)(cursorPos4.Y / 16));

            var mapEntity = entityMan.GetByTag("Map").First();
            var dudeEntity = entityMan.GetByTag("Dude").First();

            var mapCmp = mapEntity.Get<MapComponent>();
            var dudePositionCmp = dudeEntity.Get<MapPositionComponent>();
            var pathfindRequestCmp = dudeEntity.TryGet<PathfindRequestComponent>();

            var requestId = -1;
            var pathfindRequest = new PathfindRequest()
            {
                Start = dudePositionCmp.Value,
                Goal = indexPos,
                Topology = new GridTopology(mapCmp.Grid),
                Tag = dudeEntity
            };

            if (pathfindRequestCmp is not null)
            {
                requestId = pathfindRequestCmp.RequestId;
            }

            requestId = pathfindingService.RequestPath(pathfindRequest, requestId);

            if (pathfindRequestCmp is null)
            {
                dudeEntity.Remove<PathFollowRequestComponent>();
                dudeEntity.Set(new PathfindRequestComponent(requestId));
            }

            return;
        }

        #endregion Public Methods
    }
}