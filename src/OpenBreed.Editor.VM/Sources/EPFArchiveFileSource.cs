using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenBreed.Editor.VM.Database.Sources;
using System.IO;
using EPF;

namespace OpenBreed.Editor.VM.Sources
{
    public class EPFArchiveFileSource : BaseSource
    {
        private readonly EPFArchive m_Archive;
        private readonly EPFArchiveEntry m_Entry;

        public EPFArchiveFileSource(SourcesHandler manager, EPFArchiveFileSourceDef sourceDef) :
            base(manager, sourceDef)
        {
            m_Archive = manager.GetArchive(manager.Editor.Settings.ExpandVariables(sourceDef.ArchivePath));
            m_Entry = m_Archive.FindEntry(sourceDef.Name);
        }

        protected override Stream Open()
        {
            return m_Entry.Open();
        }

        protected override void Close()
        {
            m_Entry.Dispose();

            base.Close();
        }
    }
}
