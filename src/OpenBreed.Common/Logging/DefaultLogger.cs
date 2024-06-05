using Microsoft.Extensions.Logging;
using OpenBreed.Common.Configs;
using OpenBreed.Common.Interface.Logging;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenBreed.Common.Logging
{
    /// <summary>
    /// DefaultLogger class
    /// </summary>
    public class DefaultLogger : ILogger
    {
        #region Private Fields

        private readonly ILoggerClient client;

        #endregion Private Fields

        #region Public Constructors

        public DefaultLogger(ILoggerClient client)
        {
            this.client = client;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            client.OnMessage(logLevel, formatter(state, exception));
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => default!;

        #endregion Public Methods
    }
}