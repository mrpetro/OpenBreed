using OpenBreed.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenBreed.Core
{
    public class JobChain : IJob
    {
        #region Private Fields

        private Queue<IJob> queue = new Queue<IJob>();
        private Queue<IJob> completed = new Queue<IJob>();
        private IJob currentJob;

        #endregion Private Fields

        #region Public Constructors

        public JobChain()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public Action<IJob> Complete { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Equeue(IJob job)
        {
            queue.Enqueue(job);
        }

        public void Execute()
        {
        }

        public void Update(float dt)
        {
            if (currentJob != null)
                currentJob.Update(dt);

            while (completed.Any())
                completed.Dequeue().Dispose();

            if (!queue.Any())
                return;

            if (currentJob == null)
            {
                currentJob = queue.Dequeue();
                currentJob.Complete = OnComplete;
                currentJob.Execute();
            }
        }

        public void Dispose()
        {
        }

        #endregion Public Methods

        #region Private Methods

        private void OnComplete(IJob job)
        {
            completed.Enqueue(job);
            currentJob = null;
        }

        #endregion Private Methods
    }
}