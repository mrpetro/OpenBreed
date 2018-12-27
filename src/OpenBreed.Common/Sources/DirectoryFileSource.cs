using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenBreed.Common.Database.Items.Sources;

namespace OpenBreed.Common.Sources
{
    public class DirectoryFileSource : SourceBase
    {

        #region Private Fields

        public string DirectoryPath { get; }

        #endregion Private Fields

        #region Public Constructors

        public DirectoryFileSource(SourcesRepository manager, string directoryPath, string name) :
            base(manager, name)
        {
            DirectoryPath = directoryPath;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void Close()
        {
            Stream.Close();

            base.Close();
        }

        protected override Stream CreateStream()
        {
            string filePath = Path.Combine(SourcesRepository.ExpandVariables(DirectoryPath), Name);
            return File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
        }

        #endregion Protected Methods

    }
}
