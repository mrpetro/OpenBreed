using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Diagnostics;

namespace OpenBreed.Common
{
    internal class NativeMethods
    {
        [DllImport("user32.dll")]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    }

    public class ProgramTools
    {
        #region Private methods

        /// <summary>
        /// Gets directory where current app is installed
        /// </summary>
        /// <returns></returns>
        private static string GetInstallDirForCurrentApp()
        {
            string dir = AppDir;
            return dir + "\\";
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the application data directory
        /// </summary>
        public static string AppDataDir { get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); } }

        /// <summary>
        /// Gets application directory
        /// </summary>
        public static string AppDir { get { return System.AppDomain.CurrentDomain.BaseDirectory; } }

        /// <summary>
        /// Gets application executable name
        /// </summary>
        public static string AppExe { get { return System.AppDomain.CurrentDomain.FriendlyName; } }

        #endregion

        #region Public methods

        /// <summary>
        /// Initialise current app directory
        /// </summary>
        public static void InitialiseCurrentAppRuntimeDirectory()
        {
            // Gets instalation directory
            var dir = GetInstallDirForCurrentApp();

            // Throw exception if failed
            if (string.IsNullOrEmpty(dir))
                throw new DirectoryNotFoundException("Unable to locate current app directory");

            // Initialise
            var value = Environment.GetEnvironmentVariable("PATH");
            value += ";" + dir;
            Environment.SetEnvironmentVariable("PATH", value);
        }

        #endregion
    }
}