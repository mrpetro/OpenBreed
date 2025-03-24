using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBreed.Sandbox.Extensions
{
    public static class LoggerExtensions
    {
        #region Public Methods

        public static void Info(this ILogger logger, string message)
        {
            logger.LogInformation(message);
        }

        public static void Error(this ILogger logger, string message)
        {
            logger.LogError(message);
        }

        public static void Trace(this ILogger logger, string message)
        {
            logger.LogTrace(message);
        }

        public static void Warning(this ILogger logger, string message)
        {
            logger.LogWarning(message);
        }

        #endregion Public Methods
    }
}