using System;

namespace OpenBreed.Common.Logging
{
    /// <summary>
    /// LogMan class
    /// </summary>
    public class LogMan
    {
        #region Private members

        private static readonly LogMan _logMan = new LogMan();

        #endregion

        #region Constructor

        /// <summary>
        /// Private constructor
        /// </summary>
        private LogMan()
        {
        }

        #endregion

        #region Events

        public delegate void Message(LogType type, string msg);

        /// <summary>
        /// Rised when new message incomming
        /// </summary>
        public event Message MessageAdded;

        #endregion

        #region Public methods

        /// <summary>
        /// Gets instance to this object
        /// </summary>
        public static LogMan Instance
        {
            get { return _logMan; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Log debug message
        /// </summary>
        /// <param name="msg"></param>
        public void LogDebug(string msg)
        {
            var ev = MessageAdded;
            if (ev != null)
                ev(LogType.Debug, msg);
            Console.WriteLine("Debug: " + msg);
        }

        /// <summary>
        /// Log debug message
        /// </summary>
        /// <param name="msg"></param>
        public void LogDebug(Exception ex)
        {
            LogDebug(ex.Message);
        }

        /// <summary>
        /// Log message as info
        /// </summary>
        /// <param name="msg"></param>
        public void LogInfo(string msg)
        {
            var ev = MessageAdded;
            if (ev != null)
                ev(LogType.Info, msg);
        }

        /// <summary>
        /// Log message as warning
        /// </summary>
        /// <param name="msg"></param>
        public void Warning(string msg)
        {
            var ev = MessageAdded;
            if (ev != null)
                ev(LogType.Warning, msg);
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
            var ev = MessageAdded;
            if (ev != null)
                ev(LogType.Error, msg);
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
            var ev = MessageAdded;
            if (ev != null)
                ev(LogType.Critical, msg);
        }


        /// <summary>
        /// Log message as success
        /// </summary>
        /// <param name="msg"></param>
        public void Success(string msg)
        {
            var ev = MessageAdded;
            if (ev != null)
                ev(LogType.Success, msg);
        }

        #endregion
    }
}