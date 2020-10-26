using System;

namespace OpenBreed.Common.Logging
{
    /// <summary>
    /// DefaultLogger class
    /// </summary>
    public class DefaultLogger : ILogger
    {
        #region Public Constructors

        public DefaultLogger()
        {
        }

        #endregion Public Constructors

        #region Public Events

        /// <summary>
        /// Rised when new message incomming
        /// </summary>
        public event Message MessageAdded;

        #endregion Public Events

        #region Public Methods

        /// <summary>
        /// Log debug message
        /// </summary>
        /// <param name="msg"></param>
        public void Debug(string msg)
        {
            MessageAdded?.Invoke(LogType.Debug, msg);
            Console.WriteLine("Debug: " + msg);
        }

        /// <summary>
        /// Log debug message
        /// </summary>
        /// <param name="msg"></param>
        public void Debug(Exception ex)
        {
            Debug(ex.Message);
        }

        /// <summary>
        /// Log message as info
        /// </summary>
        /// <param name="msg"></param>
        public void Info(string msg)
        {
            MessageAdded?.Invoke(LogType.Info, msg);
        }

        /// <summary>
        /// Log message as warning
        /// </summary>
        /// <param name="msg"></param>
        public void Warning(string msg)
        {
            MessageAdded?.Invoke(LogType.Warning, msg);
        }

        /// <summary>
        /// Log message as error
        /// </summary>
        /// <param name="ex"></param>
        public void Error(Exception ex)
        {
            Error(ex.Message);
        }

        /// <summary>
        /// Log message as error
        /// </summary>
        /// <param name="msg"></param>
        public void Error(string msg)
        {
            MessageAdded?.Invoke(LogType.Error, msg);
        }

        /// <summary>
        /// Log message as critical
        /// </summary>
        /// <param name="ex"></param>
        public void Critical(Exception ex)
        {
            Critical(ex.Message);
        }

        /// <summary>
        /// Log message as critical
        /// </summary>
        /// <param name="msg"></param>
        public void Critical(string msg)
        {
            MessageAdded?.Invoke(LogType.Critical, msg);
        }

        /// <summary>
        /// Log message as success
        /// </summary>
        /// <param name="msg"></param>
        public void Success(string msg)
        {
            MessageAdded?.Invoke(LogType.Success, msg);
        }

        #endregion Public Methods
    }
}