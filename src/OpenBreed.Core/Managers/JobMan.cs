﻿using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core.Managers
{
    /// <summary>
    /// Job manager class
    /// </summary>
    public class JobMan
    {
        #region Private Fields

        private List<IJob> toExecute = new List<IJob>();
        private List<IJob> running = new List<IJob>();
        private List<IJob> completed = new List<IJob>();

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Constructor which requires core object
        /// </summary>
        /// <param name="core">Reference to Core object</param>
        public JobMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Reference to Core object
        /// </summary>
        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Execute job given in argument. Job will not be executed immediately,
        /// it will be executed next time this class Update will be called.
        /// </summary>
        /// <param name="job">Job to execute</param>
        public void Execute(IJob job)
        {
            toExecute.Add(job);
        }

        /// <summary>
        /// Perform an update of this manager. This will update all running jobs.
        /// It will also dispose all completed jobs and execute any pending ones.
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

            if (toExecute.Any())
            {
                for (int i = 0; i < toExecute.Count; i++)
                {
                    var job = toExecute[i];
                    job.Complete = OnComplete;
                    job.Execute();
                    running.Add(job);
                }

                toExecute.Clear();
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