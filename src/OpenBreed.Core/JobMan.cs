using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core
{
    public interface IJob : IDisposable
    {
        #region Public Properties

        Action Complete { get; set; }

        #endregion Public Properties

        #region Public Methods

        void Execute();

        #endregion Public Methods
    }

    public class JobMan
    {
        #region Private Fields

        private Queue<IJob> queue = new Queue<IJob>();
        private Queue<IJob> completed = new Queue<IJob>();
        private IJob currentJob;

        #endregion Private Fields

        #region Public Constructors

        public JobMan(ICore core)
        {
            Core = core;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICore Core { get; }

        #endregion Public Properties

        #region Public Methods

        public void Equeue(IJob job)
        {
            queue.Enqueue(job);
        }

        public void Update()
        {
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

        #endregion Public Methods

        #region Private Methods

        private void OnComplete()
        {
            completed.Enqueue(currentJob);
            currentJob = null;
        }

        #endregion Private Methods
    }
}
