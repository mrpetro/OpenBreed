using Microsoft.Extensions.Hosting;
using OpenBreed.Core.Abstractions;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Pathfinding.Abstractions.Services
{

    public interface IPathfindingService
    {
        #region Public Methods

        int RequestPath(PathfindRequest request, int requestId = -1);

        void Step();

        bool TryGetJob(int requestId, out IPathfindJob job);

        #endregion Public Methods
    }

    public struct PathfindRequest
    {
        #region Public Fields

        public Vector2i Start;
        public Vector2i Goal;
        public ITopology Topology;
        public object Tag;

        #endregion Public Fields
    }
}