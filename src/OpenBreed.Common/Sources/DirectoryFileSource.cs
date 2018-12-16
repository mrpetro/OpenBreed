using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Database.Items.Sources;

namespace OpenBreed.Common.Sources
{
    public class DirectoryFileSource : BaseSource
    {

        #region Private Fields

        private readonly string _directoryPath;

        #endregion Private Fields

        #region Public Constructors

        public DirectoryFileSource(SourceMan manager, DirectoryFileSourceDef sourceDef) :
            base(manager, sourceDef)
        {
            _directoryPath = manager.ExpandVariables(sourceDef.DirectoryPath);
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
