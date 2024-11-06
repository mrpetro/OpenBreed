using OpenBreed.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace OpenBreed.Editor.UI.Wpf
{
    internal class UIDispatcher : IDispatcher
    {
        #region Private Fields

        private readonly Dispatcher dispatcher;

        #endregion Private Fields

        #region Public Constructors

        public UIDispatcher(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Invoke(Action callback)
        {
            dispatcher.Invoke(callback);
        }

        public TResult Invoke<TResult>(Func<TResult> callback)
        {
            return dispatcher.Invoke(callback);
        }

        #endregion Public Methods
    }
}