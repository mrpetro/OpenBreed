using OpenBreed.Core.Abstractions;
using OpenBreed.Pathfinding.Abstractions;
using OpenBreed.Pathfinding.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Pathfinding.Services
{
    internal class Pathfind4WayJob : IInternalPathfindJob
    {
        #region Private Fields

        private const int directionsCount = 8;

        private readonly Dictionary<int, int> cameFrom = new Dictionary<int, int>();

        private readonly List<PathfindFront> fronts = new List<PathfindFront>();

        private readonly List<PathfindFront> newFronts = new List<PathfindFront>();

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
            fronts.Add(CreateFront(StartId, 1, 0));
            cameFrom.Add(StartId, -1);
        }

        private PathfindFront CreateFront(int id, float weight, float distance)
        {
            return new PathfindFront(id,  distance / weight);
        }

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

                if (!front.Step(smallestStep))
                {
                    newFronts.Add(front);
                    continue;
                }

                if (ExpandFront(front))
                {
                }
            }

            fronts.Clear();

            smallestStep = float.MaxValue;

            foreach (var front in newFronts)
            {
                smallestStep = Math.Min(smallestStep, front.StepLeft);
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

        private float smallestStep = float.MaxValue;

        private bool ExpandFront(PathfindFront front)
        {
            var frontId = front.Id;
            var survived = false;

            for (int i = 0; i < directionsCount; i++)
            {
                if (Topology.TryGetNeighborNodeId(frontId, i, out int nextId, out float distance))
                {
                    if (CheckNeighbour(front, nextId, distance, out PathfindFront newFront))
                    {
                        newFronts.Add(newFront);
                        survived = true;
                    }
                }
            }

            return survived;
        }

        private bool CheckNeighbour(PathfindFront front, int nextId, float distance, out PathfindFront newFront)
        {
            if (cameFrom.ContainsKey(nextId))
            {
                newFront = null;
                return false;
            }

            var weight = Topology.GetWeight(nextId);

            if (weight == 0.0f)
            {
                newFront = null;
                return false;
            }

            cameFrom.Add(nextId, front.Id);
            newFront = CreateFront(nextId, weight, distance);
            return true;
        }

        #endregion Private Methods

        #region Internal Classes

        internal class NodeState
        {
            #region Public Constructors

            public NodeState(int id, int progress)
            {
                Id = id;
                this.progress = progress;
            }

            #endregion Public Constructors

            #region Public Properties

            public int Id { get; }
            public int progress { get; }

            #endregion Public Properties
        }

        #endregion Internal Classes
    }
}