using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBreed.Common.Interface.Tools
{
    public static class ThreadTools
    {
        #region Private Fields

        private static int mainThreadId = int.MinValue;

        #endregion Private Fields

        #region Public Properties

        public static bool IsMainThread
        {
            get
            {
                if (mainThreadId == int.MinValue)
                {
                    throw new InvalidOperationException($"Forgot to call {nameof(ThreadTools)}.{nameof(Initialize)}()");
                }

                return Thread.CurrentThread.ManagedThreadId == mainThreadId;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public static void Initialize()
        {
            mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        #endregion Public Methods
    }
}