using OpenBreed.Core.Abstractions.Managers;
using OpenBreed.Pathfinding.Abstractions.Events;
using OpenBreed.Pathfinding.Abstractions.Services;
using System;
using System.Collections.Generic;

namespace OpenBreed.Pathfinding.Services
{
    internal class PathfindingService : IPathfindingService
    {
        #region Private Fields

        private readonly LinkedList<IInternalPathfindJob> jobQueue = new LinkedList<IInternalPathfindJob>();

        private readonly Dictionary<int, IInternalPathfindJob> jobLookup = new Dictionary<int, IInternalPathfindJob>();

        private readonly List<IInternalPathfindJob> workingJobs = new List<IInternalPathfindJob>();

        private readonly List<IInternalPathfindJob> jobsToFinish = new List<IInternalPathfindJob>();

        private readonly IEventsMan eventsMan;

        private int id = 0;

        #endregion Private Fields

        #region Public Constructors

        public PathfindingService(IEventsMan eventsMan)
        {
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));
        }

        #endregion Public Constructors

        #region Public Methods

        public int RequestPath(PathfindRequest request, int requestId = -1)
        {
            var job = default(IInternalPathfindJob);

            if (requestId != -1)
            {
                if (!jobLookup.TryGetValue(requestId, out job))
                {
                    throw new InvalidOperationException($"Expected existing job with request ID = {requestId}.");
                }

                jobQueue.Remove(job);
                workingJobs.Remove(job);
                job.Reset(request);
                jobQueue.AddLast(job);
                return requestId;
            }

            job = new PathfindJob4Way(id, request);

            jobQueue.AddLast(job);
            jobLookup.Add(id, job);
            return id++;
        }

        public bool TryGetJob(int requestId, out IPathfindJob job)
        {
            if (jobLookup.TryGetValue(requestId, out IInternalPathfindJob internalJob))
            {
                job = internalJob;
                return true;
            }

            job = default;
            return false;
        }

        public void Step()
        {
            if (jobQueue.Count > 0)
            {
                var job = jobQueue.First.Value;
                jobQueue.RemoveFirst();
                RunJob(job);
            }

            foreach (var job in workingJobs)
            {
                if (job.Step())
                {
                    jobsToFinish.Add(job);
                }
            }

            foreach (var job in jobsToFinish)
            {
                eventsMan.Raise(new PathfindJobComplete(job));
                jobLookup.Remove(job.Id);
            }

            jobsToFinish.Clear();
        }

        #endregion Public Methods

        #region Private Methods

        private void RunJob(IInternalPathfindJob job)
        {
            job.Run();
            workingJobs.Add(job);
        }

        #endregion Private Methods
    }
}