using System;

namespace OpenBreed.Common.Logging
{
    /// <summary>
    /// DefaultLogger class
    /// </summary>
    public class DefaultLogger : ILogger
    {
        public int DefaultChannel { get; }
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
        /// Log message of Verbose level to default channel
        /// </summary>
        /// <param name="message">Message text</param>
        public void Verbose(string message) => Verbose(DefaultChannel, message);

        /// <summary>
        /// Log message of Verbose level to channel with given number
        /// </summary>
        /// <param name="msg">Message text</param>
        public void Verbose(int channel, string message)
        {
            MessageAdded?.Invoke(LogLevel.Verbose, channel, message);
        }

        /// <summary>
        /// Log message of Info level to default channel
        /// </summary>
        /// <param name="message">Message text</param>
        public void Info(string message) => Info(DefaultChannel, message);

        /// <summary>
        /// Log message of Info level to channel with given number
        /// </summary>
        /// <param name="msg">Message text</param>
        public void Info(int channel, string message)
        {
            MessageAdded?.Invoke(LogLevel.Info, channel, message);
        }

        /// <summary>
        /// Log message of Warning level to default channel
        /// </summary>
        /// <param name="message">Message text</param>
        public void Warning(string message) => Warning(DefaultChannel, message);

        /// <summary>
        /// Log message of Warning level to channel with given number
        /// </summary>
        /// <param name="msg">Message text</param>
        public void Warning(int channel, string message)
        {
            MessageAdded?.Invoke(LogLevel.Warning, channel, message);
        }

        /// <summary>
        /// Log message of Error level to default channel
        /// </summary>
        /// <param name="message">Message text</param>
        public void Error(string message) => Error(DefaultChannel, message);

        /// <summary>
        /// Log message of Error level to channel with given number
        /// </summary>
        /// <param name="msg">Message text</param>
        public void Error(int channel, string message)
        {
            MessageAdded?.Invoke(LogLevel.Error, channel, message);
        }

        /// <summary>
        /// Log message of Critical level to default channel
        /// </summary>
        /// <param name="message">Message text</param>
        public void Critical(string message) => Critical(DefaultChannel, message);

        /// <summary>
        /// Log message of Critical level to channel with given number
        /// </summary>
        /// <param name="msg">Message text</param>
        public void Critical(int channel, string message)
        {
            MessageAdded?.Invoke(LogLevel.Critical, channel, message);
        }

        #endregion Public Methods
    }
}