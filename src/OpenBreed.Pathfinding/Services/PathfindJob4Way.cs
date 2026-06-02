using OpenBreed.Core.Abstractions;
using OpenBreed.Pathfinding.Abstractions.Services;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Pathfinding.Services
{
    internal class PathfindJob4Way : IInternalPathfindJob
    {
        #region Private Fields

        private readonly Dictionary<int, int> cameFrom = new Dictionary<int, int>();

        private List<int> front = new List<int>();

        private int startId;

        private int goalId;

        #endregion Private Fields

        #region Public Constructors

        public PathfindJob4Way(int id, PathfindRequest request)
        {
            Id = id;
            Terain = request.Terain;
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
        public IPathfindTerain Terain { get; }
        public IReadOnlyList<int> Fronts => front;

        #endregion Public Properties

        #region Public Methods

        public IEnumerable<(Vector2i, Vector2i)> GetWaypoints()
        {
            foreach (var pair in cameFrom)
            {
                var fromPos = Terain.GetPosition(pair.Key);
                var toPos = Terain.GetPosition(pair.Value);
                yield return (fromPos, Vector2i.Subtract(toPos, fromPos));
            }
        }

        public void Run()
        {
            Status = PathfindStatus.Searching;

            var startId = Terain.GetId(Start);
            front.Add(startId);
            cameFrom.Add(startId, -1);
        }

        public bool Step()
        {
            if (Status != PathfindStatus.Searching)
            {
                return false;
            }

            if (front.Count == 0)
            {
                Status = PathfindStatus.Failed;
                return true;
            }

            var oldFront = front;
            front = new List<int>();

            foreach (var id in oldFront)
            {
                if (goalId == id)
                {
                    Status = PathfindStatus.Found;
                    return true;
                }

                ExpandFront(id);
            }

            return false;
        }

        public IEnumerable<Vector2i> GetShortestPath()
        {
            var id = goalId;
            var waypoints = new List<Vector2i>();

            var pos = Terain.GetPosition(id);
            waypoints.Add(pos);

            while (cameFrom.TryGetValue(id, out int prevId) && prevId != -1)
            {
                pos = Terain.GetPosition(prevId);
                waypoints.Add(pos);
                id = prevId;
            }

            return waypoints.Reverse<Vector2i>();
        }

        public void Reset(PathfindRequest request)
        {
            Start = request.Start;
            Goal = request.Goal;

            startId = Terain.GetId(Start);
            goalId = Terain.GetId(Goal);
            Status = PathfindStatus.NotStarted;
            cameFrom.Clear();
            front.Clear();
        }

        #endregion Public Methods

        #region Private Methods

        private void ExpandFront(int frontId)
        {
            if (Terain.TryGetUpFromId(frontId, out int upId))
            {
                CheckFront(frontId, upId);
            }

            if (Terain.TryGetDownFromId(frontId, out int downId))
            {
                CheckFront(frontId, downId);
            }

            if (Terain.TryGetLeftFromId(frontId, out int leftId))
            {
                CheckFront(frontId, leftId);
            }

            if (Terain.TryGetRightFromId(frontId, out int rightId))
            {
                CheckFront(frontId, rightId);
            }
        }

        private void CheckFront(int frontId, int nextId)
        {
            var pos = Terain.GetPosition(nextId);
            float value = Terain.GetWeight(nextId);

            if (value == 0)
            {
                return;
            }

            if (cameFrom.ContainsKey(nextId))
            {
                return;
            }

            cameFrom.Add(nextId, frontId);
            front.Add(nextId);
        }

        #endregion Private Methods
    }
}