using OpenBreed.Core.Abstractions;
using OpenBreed.Core.Abstractions.Events;
using OpenBreed.Core.Abstractions.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Managers
{
    /// <summary>
    /// Job manager class
    /// </summary>
    internal class JobsMan : IJobsMan
    {
        #region Private Fields

        private readonly List<IJob> running = new List<IJob>();
        private readonly List<IJob> completed = new List<IJob>();
        private readonly IEventsMan eventsMan;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Constructor which requires core object
        /// </summary>
        /// <param name="core">Reference to Core object</param>
        public JobsMan(IEventsMan eventsMan)
        {
            this.eventsMan = eventsMan ?? throw new ArgumentNullException(nameof(eventsMan));

            this.eventsMan.Subscribe<WindowUpdateEvent>((e) => OnUpdate(e.Dt));
        }

        #endregion Public Constructors

        #region Public Methods

        public IJobBuilder Create()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Execute job given in argument. Job will be executed immediately.
        /// </summary>
        /// <param name="job">Job to execute</param>
        public void Execute(IJob job)
        {
            job.Complete = OnComplete;
            job.Execute();
            running.Add(job);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnUpdate(float dt)
        {
            for (int i = 0; i < running.Count; i++)
                running[i].Update(dt);

            if (completed.Any())
            {
                for (int i = 0; i < completed.Count; i++)
                    completed[i].Dispose();

                completed.Clear();
            }
        }

        private void OnComplete(IJob job)
        {
            running.Remove(job);
            completed.Add(job);
        }

        #endregion Private Methods
    }
}