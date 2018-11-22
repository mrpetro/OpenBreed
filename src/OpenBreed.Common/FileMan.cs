using System.IO;

namespace OpenBreed.Common
{
    /// <summary>
    /// TempFileManager
    /// </summary>
    public class FileMan
    {
        #region Private members

        /// <summary>
        /// Keeps reference to this object
        /// </summary>
        private static FileMan instance = new FileMan();

        /// <summary>
        /// Keeps path to temporary directory
        /// </summary>
        private string path;

        #endregion

        #region Constructors

        /// <summary>
        /// Generic private constructor
        /// </summary>
        private FileMan()
        {
            SetTempDirectory(null);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets instance of this object
        /// </summary>
        public static FileMan Instance { get { return instance; } }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates directory if doesn't exist
        /// </summary>
        /// <param name="path"></param>
        public void CreateDirectory(string path)
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
        public void CreateEmptyFile(string filename)
        {
            var fullpath = Path.GetFullPath(filename);
            var dirpath = Path.GetDirectoryName(fullpath);
            CreateDirectory(dirpath);
            using (var stream = File.Create(fullpath))
                stream.Close();
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

        /// <summary>
        /// Sets name of temporary directory
        /// </summary>
        /// <returns></returns>
        public void SetTempDirectory(string dirPath)
        {
            this.path = string.IsNullOrEmpty(dirPath) ?
                Path.GetTempPath() :
                Path.GetFullPath(dirPath);
        }

        #endregion
    }
}
