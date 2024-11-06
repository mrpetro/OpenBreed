using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface
{
    public interface IDispatcher
    {
        #region Public Methods

        void Invoke(Action action);

        TResult Invoke<TResult>(Func<TResult> action);

        #endregion Public Methods
    }
}