using OpenBreed.Core.Abstractions;
using OpenBreed.Pathfinding.Abstractions.Services;
using OpenTK.Mathematics;
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

    internal class PathfindJob4Way : IInternalPathfindJob
    {


        #region Private Fields

        private readonly Dictionary<int, int> cameFrom = new Dictionary<int, int>();
        private readonly Dictionary<int, int> progress = new Dictionary<int, int>();

        private readonly List<PathfindFront> fronts = new List<PathfindFront>();

        private int startId;

        private int goalId;

        #endregion Private Fields

        #region Public Constructors

        public PathfindJob4Way(int id, PathfindRequest request)
        {
            Id = id;
            Topology = request.Topology;
            Tag = request.Tag;
            Reset(request);
        }

        #endregion Public Constructors

        #region Public Properties

        public Vector2i Start { get; private set; }

        public Vector2i Goal { get; private set; }

        public PathfindStatus Status { get; private set; } = PathfindStatus.NotStarted;

        public int Id { get; }

        public object Tag { get; }

        public ITopology Topology { get; }

        public IEnumerable<IPathfindFront> Fronts => fronts;

        #endregion Public Properties

        #region Public Methods

        public IEnumerable<(Vector2i, Vector2i)> GetWaypoints()
        {
            foreach (var pair in cameFrom)
            {
                var fromPos = Topology.GetPosition(pair.Key);
                var toPos = Topology.GetPosition(pair.Value);
                yield return (fromPos, Vector2i.Subtract(toPos, fromPos));
            }
        }

        public void Run()
        {
            Status = PathfindStatus.Searching;

            var startId = Topology.GetId(Start);
            fronts.Add(new PathfindFront(startId, 1));
            cameFrom.Add(startId, -1);
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
                if (goalId == front.Id)
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

            //var toRemove = new List<PathfindFront>();

            foreach (var front in newFronts)
            {
                //if (front.Step())
                //{
                    fronts.Add(front);
                    //toRemove.Add(front);
                //}
            }

            //foreach (var front in toRemove)
            //{
            //    newFronts.Remove(front);
            //}

            newFronts.Clear();

            return false;
        }

        public IEnumerable<Vector2i> GetShortestPath()
        {
            var id = goalId;
            var waypoints = new List<Vector2i>();

            var pos = Topology.GetPosition(id);
            waypoints.Add(pos);

            while (cameFrom.TryGetValue(id, out int prevId) && prevId != -1)
            {
                pos = Topology.GetPosition(prevId);
                waypoints.Add(pos);
                id = prevId;
            }

            return waypoints.Reverse<Vector2i>();
        }

        public void Reset(PathfindRequest request)
        {
            Start = request.Start;
            Goal = request.Goal;

            startId = Topology.GetId(Start);
            goalId = Topology.GetId(Goal);
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

            if (Topology.TryGetUpFromId(frontId, out int upId))
            {
                if (CheckNeighbour(front, upId, out PathfindFront newUpFront))
                {
                    newFronts.Add(newUpFront);
                    survived = true;
                }
            }

            if (Topology.TryGetDownFromId(frontId, out int downId))
            {
                if (CheckNeighbour(front, downId, out PathfindFront newDownFront))
                {
                    newFronts.Add(newDownFront);
                    survived = true;
                }
            }

            if (Topology.TryGetLeftFromId(frontId, out int leftId))
            {
                if (CheckNeighbour(front, leftId, out PathfindFront newLeftFront))
                {
                    newFronts.Add(newLeftFront);
                    survived = true;
                }
            }

            if (Topology.TryGetRightFromId(frontId, out int rightId))
            {
                if (CheckNeighbour(front, rightId, out PathfindFront newRightFront))
                {
                    newFronts.Add(newRightFront);
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

            var pos = Topology.GetPosition(nextId);
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