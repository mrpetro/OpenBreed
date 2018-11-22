using System;

namespace OpenBreed.Common.Logging
{
    /// <summary>
    /// LogMan class
    /// </summary>
    public class LogMan
    {
        #region Private members

        private static readonly LogMan m_LogMan = new LogMan();

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
            get { return m_LogMan; }
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
            Console.WriteLine("Info: " + msg);
        }

        /// <summary>
        /// Log message as warning
        /// </summary>
        /// <param name="msg"></param>
        public void LogWarning(string msg)
        {
            var ev = MessageAdded;
            if (ev != null)
                ev(LogType.Warning, msg);
            Console.WriteLine("Warning: " + msg);
        }

        /// <summary>
        /// Log message as warning
        /// </summary>
        /// <param name="ex"></param>
        public void LogWarning(Exception ex)
        {
            LogWarning(ex.Message);
        }

        /// <summary>
        /// Log message as error
        /// </summary>
        /// <param name="ex"></param>
        public void LogError(Exception ex)
        {
            LogError(ex.Message);
        }

        /// <summary>
        /// Log message as error
        /// </summary>
        /// <param name="msg"></param>
        public void LogError(string msg)
        {
            var ev = MessageAdded;
            if (ev != null)
                ev(LogType.Error, msg);
            Console.WriteLine("Error: " + msg);
        }

        /// <summary>
        /// Log message as success
        /// </summary>
        /// <param name="msg"></param>
        public void LogSuccess(string msg)
        {
            var ev = MessageAdded;
            if (ev != null)
                ev(LogType.Success, msg);
            Console.WriteLine("OK: " + msg);
        }

        /// <summary>
        /// Add msg to log file
        /// </summary>
        /// <param name="msg"></param>
        public void LogLine(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                var ev = MessageAdded;
                if (ev != null)
                    ev(LogType.None, msg);
            }
                
        }

        /// <summary>
        /// Add empty line to log file
        /// </summary>
        public void LogEmptyLine()
        {
            var ev = MessageAdded;
            if (ev != null)
                ev(LogType.None, "");
        }

        #endregion
    }
}