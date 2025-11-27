using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBreed.Core.Abstractions
{
    public interface ITask
    {
        #region Public Methods

        void Finish();

        ITask Start();

        ITask Then(Action<ITask> action);

        #endregion Public Methods
    }
}