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

        protected static string GetAttributeValue<TAttr>(Assembly assembly, Func<TAttr, string> resolveFunc, string defaultResult = null) where TAttr : Attribute
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(TAttr), false);
            if (attributes.Length > 0)
                return resolveFunc((TAttr)attributes[0]);
            else
                return defaultResult;
        }

        /// <summary>
        /// Get application product name
        /// </summary>
        public static string AppProductName { get { return GetAttributeValue<AssemblyProductAttribute>(Assembly.GetEntryAssembly(), a => a.Product); } }

        /// <summary>
        /// Gets the product data directory
        /// </summary>
        public static string AppProductDataDir { get { return Path.Combine(AppDataDir, AppProductName); } }

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