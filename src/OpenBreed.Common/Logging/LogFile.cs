using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Tools;

namespace OpenBreed.Common.Logging
{
    public class LogFile:  IDisposable
    {
         #region private members

        private StreamWriter m_Stream;

        #endregion

        #region Constructors

        /// <summary>
        /// Generic constructor
        /// </summary>
        public LogFile()
        {
            LogDebug = false;
            m_Stream = null;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Deinit();
        }

        #endregion

        #region Private methods

        private void LogMessage(LogType type, string msg)
        {
            if (m_Stream == null)
                return;
            if (!LogDebug && type == LogType.Debug)
                return;
            if (type == LogType.None)
                m_Stream.WriteLine(msg);
            else
                m_Stream.WriteLine(type + ": " + msg);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Indicates if debug info should be logged
        /// </summary>
        public bool LogDebug { set; get; }

        #endregion

        /// <summary>
        /// Finalizes stream logger
        /// </summary>
        public void Deinit()
        {
            if (m_Stream != null)
            {
                LogMan.Instance.MessageAdded -= LogMessage;
                m_Stream.Flush();
                m_Stream.Close();
                m_Stream.Dispose();
            }
            m_Stream = null;
        }

        /// <summary>
        /// Initializes stream logger
        /// </summary>
        /// <param name="filename"></param>
        public void Init(string filename)
        {
            if (m_Stream != null)
                throw new InvalidOperationException("Stream logger is already initialized!");

            m_Stream = File.CreateText(filename);
            m_Stream.AutoFlush = true;
            LogMan.Instance.MessageAdded += LogMessage;
        }

        static public string SetupLogFile(string directoryPath, string projectName)
        {
            var outName = projectName;
            if (string.IsNullOrEmpty(outName))
                throw new InvalidOperationException("Output name is not set!");

            var outDir = directoryPath;
            if (string.IsNullOrEmpty(outDir))
                throw new InvalidOperationException("Log Folder Dorectory is not set!");

            outName += "-" + Other.TimeNowForFilename() + ".log";
            var path = Path.GetFullPath(outDir + "\\" + outName);
            FileMan.Instance.CreateEmptyFile(path);
            return path;
        }

        
    }
}
