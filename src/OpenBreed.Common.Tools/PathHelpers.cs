using System;
using System.IO;

namespace OpenBreed.Common.Tools
{
    /// <summary>
    /// File manager class
    /// </summary>
    public class PathHelpers
    {
        #region Private Fields

        /// <summary>
        /// Keeps path to temporary directory
        /// </summary>
        private static string path;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Creates directory if doesn't exist
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;
            if (Directory.Exists(path))
                return;
            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Creates empty file
        /// </summary>
        /// <param name="filename"></param>
        public static void CreateEmptyFile(string filename)
        {
            var fullpath = Path.GetFullPath(filename);
            var dirpath = Path.GetDirectoryName(fullpath);
            CreateDirectory(dirpath);
            using (var stream = File.Create(fullpath))
                stream.Close();
        }

        /// <summary>
        /// Sets name of temporary directory
        /// </summary>
        /// <returns></returns>
        public static void SetTempDirectory(string dirPath)
        {
            path = string.IsNullOrEmpty(dirPath) ?
                Path.GetTempPath() :
                Path.GetFullPath(dirPath);
        }

        /// <summary>
        /// Deletes file
        /// </summary>
        /// <param name="filename"></param>
        public void DeleteFile(string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename);
        }

        /// <summary>
        /// Gets current time
        /// </summary>
        /// <returns></returns>
        public static string TimeNowForFilename()
        {
            return DateTime.Now.ToString("yyMMdd-HHmmss");
        }

        /// <summary>
        /// Generates unique name for temporary file
        /// </summary>
        /// <returns></returns>
        public string GetTempFileName()
        {
            string name;
            do
            {
                name = path + @"\" + Path.GetRandomFileName() + @".tmp";
            } while (File.Exists(name));
            return name;
        }

        #endregion Public Methods
    }
}