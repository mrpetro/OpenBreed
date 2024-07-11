using Microsoft.Extensions.Logging;
using OpenBreed.Common.Interface.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Common.Logging
{
    public class DefaultLoggerClient : ILoggerClient
    {
        #region Public Events

        public event Message MessageAdded;

        #endregion Public Events

        #region Public Methods

        public void OnMessage(LogLevel type, string msg)
        {
            MessageAdded?.Invoke(type, msg);
        }

        #endregion Public Methods
    }
}