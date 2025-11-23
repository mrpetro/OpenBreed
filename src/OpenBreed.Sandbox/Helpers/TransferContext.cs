using OpenBreed.Wecs.Entities;
using OpenBreed.Wecs.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Helpers
{
    internal class TransferContext
    {
        #region Private Fields

        private readonly Queue<Func<TransferContext, TransferContext>> jobQueue = new Queue<Func<TransferContext, TransferContext>>();

        #endregion Private Fields

        #region Public Properties

        public IEntity cameraEntity { get; set; }
        public IEntity actorEntity { get; set; }
        public string mapKey { get; set; }
        public int entryId { get; set; }
        public IWorld targetWorld { get; internal set; }

        #endregion Public Properties

        #region Public Methods

        public TransferContext Then(Func<TransferContext, TransferContext> function)
        {
            jobQueue.Enqueue(function);

            return this;
        }

        #endregion Public Methods

        #region Internal Methods

        internal void InvokeNextJob()
        {
            var job = jobQueue.Dequeue();

            job.Invoke(this);
        }

        #endregion Internal Methods
    }
}