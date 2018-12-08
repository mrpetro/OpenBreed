using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using EPF;
using OpenBreed.Common.Database.Items.Sources;

namespace OpenBreed.Editor.VM.Sources
{
    public class EPFArchiveFileSource : BaseSource
    {
        #region Private Fields

        private readonly EPFArchive _archive;
        private readonly EPFArchiveEntry _entry;

        #endregion Private Fields

        #region Public Constructors

        public EPFArchiveFileSource(SourcesHandler manager, EPFArchiveFileSourceDef sourceDef) :
            base(manager, sourceDef)
        {
            _archive = manager.GetArchive(manager.Editor.Settings.ExpandVariables(sourceDef.ArchivePath));
            _entry = _archive.FindEntry(sourceDef.Name);
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void Close()
        {
            _entry.Dispose();

            base.Close();
        }

        protected override Stream Open()
        {
            return _entry.Open();
        }

        #endregion Protected Methods
    }
}
