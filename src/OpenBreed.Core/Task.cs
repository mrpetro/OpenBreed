using OpenBreed.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Core
{
    public class Task : ITask
    {
        #region Private Fields

        private readonly Action<ITask> action;

        private ITask next;

        #endregion Private Fields

        #region Private Constructors

        private Task(Action<ITask> action)
        {
            this.action = action;
        }

        #endregion Private Constructors

        #region Public Methods

        public static ITask Create(Action<ITask> action)
        {
            return new Task(action);
        }

        public ITask Then(Action<ITask> action)
        {
            next = new Task(action);
            return next;
        }

        public void Finish()
        {
            next?.Start();
        }

        #endregion Public Methods

        #region Internal Methods

        public ITask Start()
        {
            action.Invoke(this);
            return this;
        }

        #endregion Internal Methods
    }
}