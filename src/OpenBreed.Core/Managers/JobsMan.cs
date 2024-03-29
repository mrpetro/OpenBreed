﻿using System;
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

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Constructor which requires core object
        /// </summary>
        /// <param name="core">Reference to Core object</param>
        public JobsMan()
        {
        }

        public IJobBuilder Create()
        {
            throw new NotImplementedException();
        }

        #endregion Public Constructors

        #region Public Properties

        #endregion Public Properties

        #region Public Methods

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

        /// <summary>
        /// Perform an update of this manager. This will update all running jobs.
        /// It will also dispose all completed jobs.
        /// </summary>
        /// <param name="dt">Time step</param>
        public void Update(float dt)
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

        #endregion Public Methods

        #region Private Methods

        private void OnComplete(IJob job)
        {
            running.Remove(job);
            completed.Add(job);
        }

        #endregion Private Methods
    }
}