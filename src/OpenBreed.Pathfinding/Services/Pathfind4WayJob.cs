using OpenBreed.Core.Abstractions;
using OpenBreed.Pathfinding.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Pathfinding.Services
{
    internal class PathfindFront : IPathfindFront
    {
        private int stepsLeft;

        public PathfindFront(int id, int stepsLeft)
        {
            Id = id;
            this.stepsLeft = stepsLeft;
        }

        public int Id { get; }
        public bool Step()
        {
            stepsLeft--;
            return stepsLeft == 0;
        }
    }

    internal class Pathfind4WayJob : IInternalPathfindJob
    {
        #region Private Fields

        private readonly Dictionary<int, int> cameFrom = new Dictionary<int, int>();

        private readonly List<PathfindFront> fronts = new List<PathfindFront>();

        #endregion Private Fields

        #region Public Constructors

        public Pathfind4WayJob(int id, PathfindRequest request)
        {
            Id = id;
            Topology = request.Topology;
            Tag = request.Tag;
            Reset(request);
        }

        #endregion Public Constructors

        #region Public Properties

        public int StartId { get; private set; }

        public int GoalId { get; private set; }

        public PathfindStatus Status { get; private set; } = PathfindStatus.NotStarted;

        public int Id { get; }

        public object Tag { get; }

        public ITopology Topology { get; }

        public IEnumerable<IPathfindFront> Fronts => fronts;

        public IReadOnlyDictionary<int, int> CameFrom => cameFrom;

        #endregion Public Properties

        #region Public Methods

        public void Run()
        {
            Status = PathfindStatus.Searching;
            fronts.Add(new PathfindFront(StartId, 1));
            cameFrom.Add(StartId, -1);
        }

        private readonly List<PathfindFront> newFronts = new List<PathfindFront>();

        public bool Step()
        {
            if (Status != PathfindStatus.Searching)
            {
                return false;
            }

            if (fronts.Count == 0 && newFronts.Count == 0)
            {
                Status = PathfindStatus.Failed;
                return true;
            }

            foreach (var front in fronts)
            {
                if (GoalId == front.Id)
                {
                    Status = PathfindStatus.Found;
                    return true;
                }

                if (!front.Step())
                {
                    newFronts.Add(front);
                    continue;
                }

                if (ExpandFront(front))
                {

                }
            }

            fronts.Clear();

            foreach (var front in newFronts)
            {
                    fronts.Add(front);
            }

            newFronts.Clear();

            return false;
        }

        public IEnumerable<int> GetShortestPath()
        {
            var id = GoalId;
            var waypoints = new List<int>();

            waypoints.Add(id);

            while (cameFrom.TryGetValue(id, out int prevId) && prevId != -1)
            {
                waypoints.Add(prevId);
                id = prevId;
            }

            return waypoints.Reverse<int>();
        }

        public void Reset(PathfindRequest request)
        {
            StartId = request.StartId;
            GoalId = request.GoalId;
            Status = PathfindStatus.NotStarted;
            cameFrom.Clear();
            fronts.Clear();
            newFronts.Clear();
        }

        #endregion Public Methods

        #region Private Methods

        private bool ExpandFront(PathfindFront front)
        {
            var frontId = front.Id;
            var survived = false;

            if (Topology.TryGetNeighborNodeId(frontId, 0, out int leftId))
            {
                if (CheckNeighbour(front, leftId, out PathfindFront newLeftFront))
                {
                    newFronts.Add(newLeftFront);
                    survived = true;
                }
            }

            if (Topology.TryGetNeighborNodeId(frontId, 1, out int upId))
            {
                if (CheckNeighbour(front, upId, out PathfindFront newUpFront))
                {
                    newFronts.Add(newUpFront);
                    survived = true;
                }
            }

            if (Topology.TryGetNeighborNodeId(frontId, 2, out int rightId))
            {
                if (CheckNeighbour(front, rightId, out PathfindFront newRightFront))
                {
                    newFronts.Add(newRightFront);
                    survived = true;
                }
            }

            if (Topology.TryGetNeighborNodeId(frontId, 3, out int downId))
            {
                if (CheckNeighbour(front, downId, out PathfindFront newDownFront))
                {
                    newFronts.Add(newDownFront);
                    survived = true;
                }
            }

            return survived;
        }

        private bool CheckNeighbour(PathfindFront front, int nextId, out PathfindFront newFront)
        {
            if (cameFrom.ContainsKey(nextId))
            {
                newFront = null;
                return false;
            }

            var weight = Topology.GetWeight(nextId);

            if (weight == 0)
            {
                newFront = null;
                return false;
            }

            cameFrom.Add(nextId, front.Id);
            newFront = new PathfindFront(nextId, 3 - weight);
            return true;
        }

        #endregion Private Methods

        #region Internal Classes

        internal class NodeState
        {
            #region Private Constructors

            public NodeState(int id, int progress)
            {
                Id = id;
                this.progress = progress;
            }

            #endregion Private Constructors

            #region Public Properties

            public int Id { get; }
            public int progress { get; }

            #endregion Public Properties
        }

        #endregion Internal Classes
    }
}