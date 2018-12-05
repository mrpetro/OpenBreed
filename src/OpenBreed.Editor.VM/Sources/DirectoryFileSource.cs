using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Database.Sources;

namespace OpenBreed.Editor.VM.Sources
{
    public class DirectoryFileSource : BaseSource
    {

        #region Private Fields

        private readonly string _directoryPath;

        #endregion Private Fields

        #region Public Constructors

        public DirectoryFileSource(SourcesHandler manager, DirectoryFileSourceDef sourceDef) :
            base(manager, sourceDef)
        {
            _directoryPath = manager.Editor.Settings.ExpandVariables(sourceDef.DirectoryPath);
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void Close()
        {
            Stream.Close();

            base.Close();
        }

        protected override Stream Open()
        {
            string filePath = Path.Combine(_directoryPath, Name);
            return File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
        }

        #endregion Protected Methods

    }
}
